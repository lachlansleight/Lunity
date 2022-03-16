using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T : MonoBehaviour
{
	public Transform Parent;
	public int Capacity;
	public int CurrentCount;
	public T[] Templates;
	public bool CreateWhenEmpty;

	private List<T> _pool;
	private List<T> _activeObjects;
	private List<T> _outgoingObjects;
	private int _emptyCreationIndex;
	private bool _created;

	public ObjectPool(Transform parent, int capacity, T template, bool createWhenEmpty = false)
	{
		Parent = parent;
		Capacity = capacity;
		Templates = new [] {template};
		CreateWhenEmpty = createWhenEmpty;
		CurrentCount = 0;

		_pool = new List<T>();
		_activeObjects = new List<T>();
		_outgoingObjects = new List<T>();
	}
	
	public ObjectPool(Transform parent, int capacity, T[] templates, bool createWhenEmpty = false)
	{
		Parent = parent;
		Capacity = capacity;
		Templates = templates;
		CreateWhenEmpty = createWhenEmpty;
		CurrentCount = 0;

		_pool = new List<T>();
		_activeObjects = new List<T>();
		_outgoingObjects = new List<T>();
	}
	
	public ObjectPool(Transform parent, int capacity, List<T> templates, bool createWhenEmpty = false)
	{
		Parent = parent;
		Capacity = capacity;
		Templates = templates.ToArray();
		CreateWhenEmpty = createWhenEmpty;
		CurrentCount = 0;

		_pool = new List<T>();
		_activeObjects = new List<T>();
		_outgoingObjects = new List<T>();
	}

	public void CreateObjects()
	{
		for (var i = 0; i < Capacity; i++) {
			var templateIndex = i % Templates.Length; //note - this could instead be randomized, or in order, or whatever
			var newObj = Object.Instantiate(Templates[templateIndex]);
			newObj.transform.SetParent(Parent);
			newObj.gameObject.SetActive(false);
			_pool.Add(newObj);
		}

		_created = true;
	}

	public T Instantiate()
	{
		if (!_created) throw new UnityException("You need to call CreateObjects before instantiating pool objects");
		
		if (_pool.Count == 0) {
			if (CreateWhenEmpty) {
				var newlyCreatedObject = Object.Instantiate(Templates[_emptyCreationIndex]);
				newlyCreatedObject.transform.SetParent(Parent);
				newlyCreatedObject.gameObject.SetActive(false);
				_pool.Add(newlyCreatedObject);
				
				Capacity++;

				_emptyCreationIndex = (_emptyCreationIndex + 1) % Templates.Length;
			} else throw new UnityException("Object Pool is empty!");
		}

		var newObj = _pool[0];
		_pool.RemoveAt(0);

		newObj.gameObject.SetActive(true);
		_activeObjects.Add(newObj);
		CurrentCount++;

		return newObj;
	}

	public T Instantiate(Vector3 position, Space space = Space.World)
	{
		var obj = Instantiate();
		var t = obj.transform;
		if (space == Space.World) {
			t.position = position;
		} else {
			t.localPosition = position;
		}
		return obj;
	}
	
	public T Instantiate(Vector3 position, Quaternion rotation, Space space = Space.World)
	{
		var obj = Instantiate();
		var t = obj.transform;
		if (space == Space.World) {
			t.position = position;
			t.rotation = rotation;
		} else {
			t.localPosition = position;
			t.localRotation = rotation;
		}
		return obj;
	}
	
	public T Instantiate(Vector3 position, Quaternion rotation, Vector3 scale, Space space = Space.World)
	{
		var obj = Instantiate();
		var t = obj.transform;
		if (space == Space.World) {
			t.position = position;
			t.rotation = rotation;
		} else {
			t.localPosition = position;
			t.localRotation = rotation;
		}
		t.localScale = scale;
		return obj;
	}

	public void Destroy(T obj)
	{
		if (!_created) throw new UnityException("You need to call CreateObjects before destroying pool objects");
		
		var index = _activeObjects.IndexOf(obj);
		if (index < 0) throw new UnityException("Object is inactive!");
		_pool.Add(obj);
		_activeObjects.RemoveAt(index);
		obj.gameObject.SetActive(false);
		CurrentCount--;
	}

	public void DestroyFirst()
	{
		if (!_created) throw new UnityException("You need to call CreateObjects before destroying pool objects");
		if(_activeObjects.Count == 0) throw new UnityException("No objects to destroy");
		
		var obj = _activeObjects[0];
		_pool.Add(obj);
		_activeObjects.RemoveAt(0);
		obj.gameObject.SetActive(false);
		CurrentCount--;
	}

	public Action Takeout(T obj)
	{
		if (!_created) throw new UnityException("You need to call CreateObjects before taking out pool objects");
		if(_activeObjects.Count == 0) throw new UnityException("No objects to takeout");
		
		var index = _activeObjects.IndexOf(obj);
		if(index < 0) throw new UnityException("Object is inactive!");
		_outgoingObjects.Add(obj);
		_activeObjects.RemoveAt(index);
		CurrentCount--;
		
		//returns a callback that can be used to conclude the outgoing process
		return () => {
			var outgoingIndex = _outgoingObjects.IndexOf(obj);
			if(outgoingIndex < 0) throw new UnityException("Object is no longer outgoing");
			_pool.Add(obj);
			_outgoingObjects.RemoveAt(outgoingIndex);
			obj.gameObject.SetActive(false);
		};
	}

	public Action TakeoutFirst(out T takeout)
	{
		if (!_created) throw new UnityException("You need to call CreateObjects before taking out pool objects");
		if(_activeObjects.Count == 0) throw new UnityException("No objects to takeout");

		var obj = _activeObjects[0];
		takeout = obj;
		
		_outgoingObjects.Add(obj);
		_activeObjects.RemoveAt(0);
		CurrentCount--;
		
		//returns a callback that can be used to conclude the outgoing process
		return () => {
			var outgoingIndex = _outgoingObjects.IndexOf(obj);
			if(outgoingIndex < 0) throw new UnityException("Object is no longer outgoing");
			_pool.Add(obj);
			_outgoingObjects.RemoveAt(outgoingIndex);
			obj.gameObject.SetActive(false);
		};
	}

	public List<T> GetActiveObjects() => _activeObjects;
}