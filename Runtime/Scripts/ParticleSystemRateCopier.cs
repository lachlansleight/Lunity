using UnityEngine;

namespace Lunity
{
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleSystemRateCopier : MonoBehaviour
	{
		[Tooltip("If not set, will try to get a particle system from (recursive) parent")] public ParticleSystem TargetOverride;
		public float RateMultiplier = 1f;
		private ParticleSystem.EmissionModule _system;
		private ParticleSystem.EmissionModule _parent;
		
		public void Awake()
		{
			_system = GetComponent<ParticleSystem>().emission;
			
			if (TargetOverride != null) {
				_parent = TargetOverride.emission;
				return;
			}
			//get parent recursively
			var parent = transform.parent;
			while (parent != null) {
				var p = parent.GetComponent<ParticleSystem>();
				if (p != null) {
					_parent = p.emission;
					break;
				}
				parent = parent.parent;
			}
		}

		public void Update()
		{
			_system.rateOverTime = new ParticleSystem.MinMaxCurve(_parent.rateOverTime.constant);
		}
	}
}