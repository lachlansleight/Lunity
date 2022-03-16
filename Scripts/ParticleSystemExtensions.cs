using UnityEngine;

namespace Lunity
{
	public static class ParticleSystemExtensions
	{
		public static void SetRate(this ParticleSystem system, float rateOverTime)
		{
			var em = system.emission;
			em.rateOverTime = new ParticleSystem.MinMaxCurve(rateOverTime);
		}
	}
}