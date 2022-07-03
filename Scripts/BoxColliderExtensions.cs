using UnityEngine;

namespace Lunity
{
	public static class BoxColliderExtensions
	{
		public static bool ContainsPoint(this BoxCollider box, Vector3 p)
		{
			p = box.transform.InverseTransformPoint( p ) - box.center;
         
			var halfX = (box.size.x * 0.5f);
			var halfY = (box.size.y * 0.5f);
			var halfZ = (box.size.z * 0.5f);
			if (p.x <= -halfX) return false;
			if (p.x >= halfX) return false;
			if (p.y <= -halfY) return false;
			if (p.y >= halfY) return false;
			if (p.z <= -halfZ) return false;
			if (p.z >= halfZ) return false;
			
			return true;
		}
	}
}