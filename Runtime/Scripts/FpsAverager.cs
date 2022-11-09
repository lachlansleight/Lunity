using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
	public class FpsAverager : MonoBehaviour
	{
		[Serializable]
		public struct FpsReading
		{
			public float time;
			public float value;

			public FpsReading(float t, float v)
			{
				time = t;
				value = v;
			}
		}

		[ReadOnly]
		public float CurrentValue;

		[ReadOnly]
		public float OneSecondAverage;

		[ReadOnly]
		public float OneMinuteAverage;

		[ReadOnly]
		public float TotalAverage;

		[ReadOnly]
		public Vector3Int BufferCounts;

		public Action<float> OnSecondAverageReceived;
		public Action<float> OnMinuteAverageReceived;

		private List<FpsReading> _oneSecondBuffer;
		private List<FpsReading> _oneMinuteBuffer;
		private List<FpsReading> _totalBuffer;
		private bool _hasLists;

		private float _nextMinuteBufferAdd;
		private float _nextTotalBufferAdd;

		public void Update()
		{
			if (!_hasLists) SetupLists();
			CurrentValue = 1f / Time.unscaledDeltaTime;

			//remove all values in the one second buffer older than a second, then add the new value
			while (_oneSecondBuffer.Count > 0) {
				if (_oneSecondBuffer[0].time > Time.time - 1f) break;
				_oneSecondBuffer.RemoveAt(0);
			}

			_oneSecondBuffer.Add(new FpsReading(Time.time, CurrentValue));

			//if it's been a second since the last time, do the same for the minute buffer
			if (Time.time > _nextMinuteBufferAdd && _oneSecondBuffer.Count > 0) {
				while (_oneMinuteBuffer.Count > 0) {
					if (_oneMinuteBuffer[0].time > Time.time - 60f) break;
					_oneMinuteBuffer.RemoveAt(0);
				}

				OneSecondAverage = 0f;
				foreach (var record in _oneSecondBuffer) {
					OneSecondAverage += record.value / _oneSecondBuffer.Count;
				}

				_oneMinuteBuffer.Add(new FpsReading(Time.time, OneSecondAverage));
				OnSecondAverageReceived?.Invoke(OneSecondAverage);

				_nextMinuteBufferAdd += 1f;
			}

			//if it's been a minute since the last time, do the same for the full buffer - but never remove anything
			if (Time.time > _nextTotalBufferAdd && _oneMinuteBuffer.Count > 0) {

				OneMinuteAverage = 0f;
				foreach (var record in _oneMinuteBuffer) {
					OneMinuteAverage += record.value / _oneMinuteBuffer.Count;
				}

				_totalBuffer.Add(new FpsReading(Time.time, OneMinuteAverage));
				OnMinuteAverageReceived?.Invoke(OneMinuteAverage);

				_nextTotalBufferAdd += 60f;
			}

			//average out all readings to get the full average
			TotalAverage = 0f;
			foreach (var record in _totalBuffer) {
				TotalAverage += record.value / _totalBuffer.Count;
			}

			//finally, fall back averages if necessary
			if (TotalAverage <= 0f) TotalAverage = OneMinuteAverage;
			if (TotalAverage <= 0f) TotalAverage = OneSecondAverage;
			if (TotalAverage <= 0f) TotalAverage = CurrentValue;

			BufferCounts = new Vector3Int(_oneSecondBuffer.Count, _oneMinuteBuffer.Count, _totalBuffer.Count);
		}

		private void SetupLists()
		{
			_oneSecondBuffer = new List<FpsReading>();
			_oneMinuteBuffer = new List<FpsReading>();
			_totalBuffer = new List<FpsReading>();
			_hasLists = true;

			_nextMinuteBufferAdd = Time.time + 1f;
			_nextTotalBufferAdd = Time.time + 60f;
		}

		public float[] GetMinuteAverages()
		{
			var output = new float[_totalBuffer.Count];
			for (var i = 0; i < _totalBuffer.Count; i++) {
				output[i] = _totalBuffer[i].value;
			}
			return output;
		}
	}
}