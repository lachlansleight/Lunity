using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScenePlacementConfig : ScriptableObject
{

	[Serializable]
	public class PrefabOptionSet
	{
		public string Name;
		public List<GameObject> Options;

		public PrefabOptionSet(string name)
		{
			Name = name;
			Options = new List<GameObject>();
		}
		
		public GameObject GetRandomPrefab()
		{
			if (Options == null || Options.Count == 0) return null;
			var attempts = 0;
			GameObject selection = null;
			while (selection == null && attempts < 100) {
				selection = Options[Random.Range(0, Options.Count)];
				attempts++;
			}

			return selection;
		}
	}
	
	public List<PrefabOptionSet> PrefabSets;

	public bool HasSets()
	{
		return PrefabSets != null && PrefabSets.Count > 0;
	}

	public void AddPrefabSet()
	{
		if (PrefabSets == null) PrefabSets = new List<PrefabOptionSet>();
		PrefabSets.Add(new PrefabOptionSet("New Prefab Set"));
	}

	public void RemovePrefabSet(int set)
	{
		if (set < 0 || PrefabSets == null || set >= PrefabSets.Count) return;
		PrefabSets.RemoveAt(set);
	}
	
	#if UNITY_EDITOR
	public static ScenePlacementConfig GetAssetInstance()
	{
		var instance = AssetDatabase.LoadAssetAtPath<ScenePlacementConfig>("Assets/Lunity/ScenePlacement/Editor/ScenePlacementConfig.asset");
		if (instance != null) return instance;

		instance = CreateInstance<ScenePlacementConfig>();
		AssetDatabase.CreateAsset(instance, "Assets/Lunity/ScenePlacement/Editor/ScenePlacementConfig.asset");
		AssetDatabase.SaveAssets();
		return instance;
	}
	#endif
}
