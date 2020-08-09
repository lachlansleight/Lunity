using UnityEngine;
 
/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// Modified from https://wiki.unity3d.com/index.php/Singleton
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// Check to see if we're about to be destroyed.
	private static bool _shuttingDown = false;
	private static object _lock = new object();
	private static T _instance;
	private static bool _preAccess;
 
	/// <summary>
	/// Access singleton instance through this propriety.
	/// </summary>
	public static T Instance
	{
		get
		{
			if (_shuttingDown && Application.isPlaying)
			{
				Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
				return null;
			}
 
			lock (_lock)
			{
				if (_instance != null) {
					_preAccess = true;
					return _instance;
				}
				
				// Search for existing instance.
				_instance = (T)FindObjectOfType(typeof(T));

				if (_instance == null) {
					var attempt = Resources.FindObjectsOfTypeAll<T>()[0];
					if (attempt != null) {
						Debug.LogError("Error - Singleton instance '" + typeof(T) + "' appears to be in the scene, but inactive. Singletons must begin as Active. Destroying and re-creating!");
					}
				}
				
				// Create new instance if one doesn't already exist.
				if (_instance != null) {
					_preAccess = true;
					return _instance;
				}
				
				// Need to create a new GameObject to attach the singleton to.
				var singletonObject = new GameObject();
				_instance = singletonObject.AddComponent<T>();
				singletonObject.name = typeof(T) + " (Singleton)";
 
				// Make instance persistent.
				DontDestroyOnLoad(singletonObject);

				_preAccess = true;
				return _instance;
			}
		}
	}

	public virtual void Awake()
	{
		
		if (_instance == null) {
			_instance = this as T;
			DontDestroyOnLoad ( gameObject );
			_shuttingDown = false;
		} else if(!_preAccess) {
			Debug.LogError("Error - there is already another initialized Singleton instance of type '" + typeof(T) + "' in the scene. Destroying this one");
			Destroy (gameObject);
		}
	}
 
	protected virtual void OnApplicationQuit()
	{
		_shuttingDown = true;
	}
 
 
	protected virtual void OnDestroy()
	{
		_shuttingDown = true;
	}
}