using System;
using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;

namespace Lunity
{
    [Serializable] public class UnityBaseObjectEvent : UnityEvent<object> { }

    [Serializable] public class UnityBoolEvent : UnityEvent<bool> { }

    [Serializable] public class UnityByteEvent : UnityEvent<byte> { }

    [Serializable] public class UnityIntEvent : UnityEvent<int> { }

    [Serializable] public class UnityShortEvent : UnityEvent<short> { }

    [Serializable] public class UnityLongEvent : UnityEvent<long> { }

    [Serializable] public class UnityFloatEvent : UnityEvent<float> { }

    [Serializable] public class UnityDoubleEvent : UnityEvent<double> { }

    [Serializable] public class UnityCharEvent : UnityEvent<char> { }

    [Serializable] public class UnityStringEvent : UnityEvent<string> { }

    [Serializable] public class UnityVector2Event : UnityEvent<Vector2> { }

    [Serializable] public class UnityVector3Event : UnityEvent<Vector3> { }

    [Serializable] public class UnityVector4Event : UnityEvent<Vector4> { }

    [Serializable] public class UnityTransformEvent : UnityEvent<Transform> { }

    [Serializable] public class UnityGameObjectEvent : UnityEvent<GameObject> { }

    [Serializable] public class UnityMonoBehaviourEvent : UnityEvent<MonoBehaviour> { }

    [Serializable] public class UnityRigidbodyEvent : UnityEvent<Rigidbody> { }

    [Serializable] public class UnityColliderEvent : UnityEvent<Collider> { }

    [Serializable] public class UnityRendererEvent : UnityEvent<Renderer> { }

    [Serializable] public class UnityMeshFilterEvent : UnityEvent<MeshFilter> { }

    [Serializable] public class UnityObjectEvent : UnityEvent<Object> { }
}