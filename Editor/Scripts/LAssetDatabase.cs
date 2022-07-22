using UnityEngine;
using UnityEditor;

public class LAssetDatabase
{
	public static T[] FindAssetsOfType<T>() where T : UnityEngine.Object
	{
		var assetList = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
		var assets = new T[assetList.Length];
		for (var i = 0; i < assetList.Length; i++) {
			var path = AssetDatabase.GUIDToAssetPath(assetList[i]);
			assets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
		}
		return assets;
	}
}