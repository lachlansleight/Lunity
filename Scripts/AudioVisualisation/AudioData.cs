using UnityEngine;
using System.Collections;

namespace Lunity
{
	[ExecuteAlways, RequireComponent(typeof(SoundCapture))]
	public class AudioData : MonoBehaviour
	{

		public SoundCapture capture;
		[Range(0f, 1f)] public float bassVol;
		[Range(0f, 1f)] public float midVol;
		[Range(0f, 1f)] public float trebleVol;
		[Range(0f, 1f)] public float avgVol;

		[Range(0f, 10f)] public float globalMult = 1f;

		public float[] fft;

		public float maxSeenVolume = 0.0000001f;
		
		public void Update()
		{
			if (capture == null) capture = GetComponent<SoundCapture>();
			if (capture.BarData.Length <= 0) return;
			fft = new float[capture.BarData.Length];
			lock (capture.BarData) {
				for (int i = 0; i < capture.BarData.Length; i++) {

					var data = globalMult * capture.BarData[i];

					avgVol += data;

					if (i < capture.BarData.Length / 3) {
						bassVol += data;
					} else if (i < 2 * capture.BarData.Length / 3) {
						midVol += data;
					} else {
						trebleVol += data;
					}
				}

				avgVol /= (float) capture.BarData.Length;
				bassVol /= (float) (capture.BarData.Length / 3);
				midVol /= (float) (capture.BarData.Length / 3);
				trebleVol /= (float) (capture.BarData.Length / 3);

				/*
				avgVol *= globalMult;
				midVol *= globalMult;
				trebleVol *= globalMult;
				bassVol *= globalMult;
				*/

				if (avgVol > maxSeenVolume) maxSeenVolume = avgVol;

				avgVol /= maxSeenVolume;
				bassVol /= maxSeenVolume;
				midVol /= maxSeenVolume;
				trebleVol /= maxSeenVolume;

				for (int i = 0; i < fft.Length; i++) {
					fft[i] = globalMult * capture.BarData[i] / maxSeenVolume;
				}
			}
		}
	}
}