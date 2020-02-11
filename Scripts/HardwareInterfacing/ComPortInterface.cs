using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Lunity
{
	public class ComPortInterface : MonoBehaviour
	{

		public class PortLoader
		{
			public SerialPort Port;
			public Action OnConnectSuccess;
			public Action OnConnectFail;

			public PortLoader(SerialPort port)
			{
				Port = port;
			}

			public void OpenPort()
			{
				try {
					Port.Open();
					
					OnConnectSuccess?.Invoke();
				} catch (IOException) {
					OnConnectFail.Invoke();
				}
			}
		}

		[Header("Properties")] 
		public int ComNumber;
		public int MaxComSearchNumber = 30;

		[Header("Events")] 
		public UnityEvent OnMessagesStart;
		public UnityEvent OnMessagesStop;
		public UnityStringEvent OnMessageReceive;
		public UnityStringEvent OnStatusMessageReceive;

		[Header("Status")] 
		public bool HasReading;
		public string LastMessage;
		
		private SerialPort _port;

		private string _buffer;

		private bool _waiting;
		private bool _portOpen;

		private int _playerPrefsStartPos;

		public void OnEnable()
		{
			_playerPrefsStartPos = PlayerPrefs.HasKey("COMSearchStart") ? PlayerPrefs.GetInt("COMSearchStart") : -1;
			if (PlayerPrefs.HasKey("MaxComPort")) MaxComSearchNumber = PlayerPrefs.GetInt("MaxComPort");
			if (_playerPrefsStartPos > 0) ComNumber = _playerPrefsStartPos;
			StartCoroutine(KeepTryingComPort());

			var names = SerialPort.GetPortNames();
			var debugOutput = "Found Serial Ports:";
			foreach (var name in names) debugOutput += $"\n{name}";
			Debug.Log(debugOutput);
		}

		public void OnDisable()
		{
			StopAllCoroutines();
			if (_port != null) {
				_port.Close();
				_port.Dispose();
			}
		}

		public void Update()
		{
			GetPortValues();
		}

		private IEnumerator KeepTryingComPort()
		{
			OnStatusMessageReceive?.Invoke("Looking for device on COM" + ComNumber + "...");

			_port = new SerialPort
			{
				PortName = "COM" + ComNumber,
				BaudRate = 9600
			};

			var portLoader = new PortLoader(_port);
			var portOpenThreadStarter = new ThreadStart(portLoader.OpenPort);
			portLoader.OnConnectSuccess += HandlePortConnectSuccess;
			portLoader.OnConnectFail += HandlePortConnectFail;
			var thread = new Thread(portOpenThreadStarter);

			_waiting = true;
			thread.Start();

			while (_waiting) {
				yield return null;
			}

			if (_portOpen) {
				StartCoroutine(TrySendConfirmationRequest());
			} else {
				yield return new WaitForSeconds(0.5f);
				ComNumber++;
				if (ComNumber > MaxComSearchNumber) ComNumber = 1;
				StartCoroutine(KeepTryingComPort());
			}
		}

		private void HandlePortConnectSuccess()
		{
			_waiting = false;
			_portOpen = true;
		}

		private void HandlePortConnectFail()
		{
			_waiting = false;
			_portOpen = false;
		}

		private IEnumerator TrySendConfirmationRequest()
		{
			while (!_port.IsOpen) {
				yield return null;
			}

			WriteToPort("?");
		}

		private IEnumerator RetryIfNoValues(float time)
		{
			yield return new WaitForSeconds(time);
			if (!HasReading) {
				ComNumber++;
				StartCoroutine(KeepTryingComPort());
			}
		}

		private void GetPortValues()
		{
			if (!_port.IsOpen) return;

			var portValue = "";
			try {
				portValue = _port.ReadExisting();
			} catch (IOException) {
				HasReading = false;
				StartCoroutine(KeepTryingComPort());

				OnMessagesStop?.Invoke();
				_buffer = "";
				return;
			}

			if (portValue.Length == 0) {
				return;
			}

			_buffer += portValue;
			if (!_buffer.Contains("|*") && !_buffer.Contains("*|")) return;
			var finalString = "";
			var withinBrackets = false;
			var i = _buffer.IndexOf("|*", StringComparison.Ordinal) + 2;
			while (i < _buffer.Length - 1) {
				if (_buffer[i] == '*' && _buffer[i+1] == '|') {
					withinBrackets = true;
					break;
				}

				finalString += _buffer[i];
				i++;
			}

			if (!withinBrackets) return;

			if (finalString == "!") {
				//this just means that we connected successfully
				if (!HasReading) {
					OnMessagesStart?.Invoke();
					if (_playerPrefsStartPos != ComNumber) PlayerPrefs.SetInt("COMSearchStart", ComNumber);
				}
				OnStatusMessageReceive?.Invoke("Connected sucessfully");

				HasReading = true;
			} else {
				LastMessage = finalString;
				OnMessageReceive?.Invoke(LastMessage);
				OnStatusMessageReceive?.Invoke("Got Message: '" + LastMessage + "'");
			}

			_buffer = "";
		}

		public void WriteToPort(string message)
		{
			if (!_port.IsOpen) return;
			
			_port.WriteLine(message);
				
		}
	}
}