using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lunity
{
    public static class LunityEditorUtils
    {
        public static void CreateAssetFolderHierarchy(string fullPath)
        {
            var folders = fullPath.Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries);
            var paths = new string[folders.Length];
            var parents = new string[folders.Length];
            
            var path = "Assets/";
            paths[0] = path;
            parents[0] = "";
            for (var i = 1; i < folders.Length; i++) {
                path += folders[i] + "/";
                paths[i] = path.TrimEnd('/');
                parents[i] = paths[i - 1].TrimEnd('/');
            }

            for (var i = 1; i < paths.Length; i++) {
                if (!AssetDatabase.IsValidFolder(paths[i])) {
                    AssetDatabase.CreateFolder(parents[i], folders[i]);
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}