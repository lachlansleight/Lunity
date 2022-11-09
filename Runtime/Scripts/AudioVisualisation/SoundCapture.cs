using UnityEngine;
using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using CSCore.CoreAudioAPI;
using WinformsVisualization.Visualization;
using CSCore.DSP;
using CSCore.Streams;

namespace Lunity
{

    // This file was modified from something I found like 10 years ago.
    // I'm sorry, original creator for not attributing it to you, I downloaded this when I was a teenager learning
    // to code and didn't know anything about open source software :'c

    [ExecuteAlways]
    public class SoundCapture : MonoBehaviour
    {
        // Use this for initialization

        [Range(3, 120)] public int FftBinCount = 30;

        [Range(0, 250)] public int Latency = 20;
        public FftSize FftSize = FftSize.Fft4096;
        [Range(20, 20000)] public int MinimumFrequency = 20;
        [Range(20, 20000)] public int MaximumFrequency = 10000;
        public bool UseAverage = true;
        public ScalingStrategy ScalingStrategy = ScalingStrategy.Linear;
        public bool UseLogScale = true;
        
        public float[] BarData;

        SpectrumBase _spectrum;
        WasapiCapture _capture;
        WaveWriter _writer;
        float[] _fftBuffer;
        SingleBlockNotificationStream _notificationSource;
        IWaveSource _finalSource;
        private byte[] _rawBuffer;

        private void Initialize()
        {
            if (_capture != null) Cleanup();
            
            // This uses the wasapi api to get any sound data played by the computer
            _capture = new WasapiLoopbackCapture(20);
            _capture.Initialize();
            var source = new SoundInSource(_capture).ToSampleSource();
            _capture.DataAvailable += Capture_DataAvailable;
            
            var notificationSource = new SingleBlockNotificationStream(source);
            notificationSource.SingleBlockRead += NotificationSource_SingleBlockRead;
            _finalSource = notificationSource.ToWaveSource();
            _rawBuffer = new byte[_finalSource.WaveFormat.BytesPerSecond / 2];

            // Actual fft data
            _fftBuffer = new float[(int) FftSize];
            _spectrum = new SpectrumBase()
            {
                SpectrumProvider = new BasicSpectrumProvider(
                    _capture.WaveFormat.Channels,
                    _capture.WaveFormat.SampleRate, 
                    FftSize
                ),
                UseAverage = UseAverage,
                IsXLogScale = UseLogScale,
                ScalingStrategy = ScalingStrategy,
                MinimumFrequency = MinimumFrequency,
                MaximumFrequency = MaximumFrequency,
                SpectrumResolution = FftBinCount,
                FftSize = FftSize,
            };

            _capture.Start();
        }

        [EditorButton]
        public void Reinitialize()
        {
            Cleanup();
            Initialize();
        }

        [EditorButton]
        public void LogDevices()
        {
            var devices = MMDeviceEnumerator.EnumerateDevices(DataFlow.All);
            for (var i = 0; i < devices.Count; i++) {
                var device = devices[i];
                Debug.Log($"Device {device.FriendlyName} ({device.DeviceID}) - {device.DataFlow}");
            }
        }
        
        void Update()
        {
            if (_capture == null) Initialize();

            _spectrum.SpectrumProvider.GetFftData(_fftBuffer, this);
            var values = _spectrum.GetSpectrumPoints(1d, _fftBuffer);
            BarData = values;

            // var x = 0f;
            // foreach (var point in values) {
            //     Debug.DrawLine(new Vector3(x, 0f, 0f), new Vector3(x, point + 0.01f, 0f), Color.white);
            //     x += 1f / values.Length;
            // }
            //
            // x = 0f;
            // foreach (var point in _fftBuffer) {
            //     Debug.DrawLine(new Vector3(x, 0f, 0.1f), new Vector3(x, point + 0.01f, 0.1f), Color.red);
            //     x += 1f / _fftBuffer.Length;
            // }
        }
        
        private void Capture_DataAvailable(object sender, DataAvailableEventArgs e)
        {
            while ((_finalSource.Read(_rawBuffer, 0, _rawBuffer.Length)) > 0) { }
        }

        private void NotificationSource_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {
            (_spectrum.SpectrumProvider as BasicSpectrumProvider).Add(e.Left, e.Right);
        }

        void OnDisable()
        {
            Cleanup();
        }

        public void Cleanup()
        {
            if (_capture == null) return;

            try {
                _capture.Stop();
            } catch {
                //ignore
            }

            try {
                _capture.Dispose();
            } catch {
                //ignore
            }
        }

        private void GetFftBins(float[] fft)
        {
            
        }
    }
}