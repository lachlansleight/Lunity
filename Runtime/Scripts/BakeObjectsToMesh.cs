using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lunity
{
    #if UNITY_EDITOR
    public class BakeObjectsToMesh
    {
        [MenuItem("Lunity/Bake Objects to Mesh")]
        public static void BakeObjects()
        {
            CreateObject(Selection.gameObjects, "Combined Object", true);
        }

        public static GameObject CreateObject(GameObject[] objects, string name = "Combined Object", bool recursivelyFindChildren = false)
        {
            var mesh = new Mesh();
            var meshes = new List<Mesh>();
            var transforms = new List<Matrix4x4>();
            var meshIndices = new List<int>();
            var materials = new List<Material>();

            if (recursivelyFindChildren) {
                var finalObjectList = new List<GameObject>();
                foreach (var obj in objects) {
                    finalObjectList.Add(obj);
                    var children = obj.GetComponentsInChildren<Transform>();
                    foreach (var child in children) finalObjectList.Add(child.gameObject);
                }
                objects = finalObjectList.ToArray();
            }

            foreach (var obj in objects) {
                var mf = obj.GetComponent<MeshFilter>();
                if (!mf) continue;
                meshes.Add(mf.sharedMesh);
                transforms.Add(obj.transform.localToWorldMatrix);
                var mat = obj.GetComponent<MeshRenderer>().sharedMaterial;
                if (materials.Contains(mat)) {
                    meshIndices.Add(materials.IndexOf(mat));
                } else {
                    materials.Add(mat);
                    meshIndices.Add(materials.Count - 1);
                }
            }

            var combineInstance = new Dictionary<int, List<CombineInstance>>();
            for (var i = 0; i < meshes.Count; i++) {
                if (!combineInstance.ContainsKey(meshIndices[i])) {
                    combineInstance.Add(meshIndices[i], new List<CombineInstance>());
                }
                combineInstance[meshIndices[i]].Add(new CombineInstance
                {
                    mesh = meshes[i],
                    transform = transforms[i],
                });
            }
            
            //each of these meshes contains all the meshes that share a material
            var finalCombine = new List<CombineInstance>();
            for (var i = 0; i < combineInstance.Count; i++) {
                var submesh = new Mesh();
                submesh.CombineMeshes(combineInstance[i].ToArray(), true, true);
                finalCombine.Add(new CombineInstance()
                {
                    mesh = submesh,
                });
            }

            mesh.CombineMeshes(finalCombine.ToArray(), false, false);
            mesh.RecalculateBounds();
            mesh.Optimize();

            mesh.name = "Baked Mesh";

            var newObj = new GameObject(name);
            newObj.AddComponent<MeshFilter>().sharedMesh = mesh;
            newObj.AddComponent<MeshRenderer>().sharedMaterials = materials.ToArray();

            return newObj;
        }

        public static string CreateBake(GameObject[] objects, string name, bool saveAndRefresh = true)
        {
            var newObj = CreateObject(objects);
            var mesh = newObj.GetComponent<MeshFilter>().sharedMesh;

            var path = $"Assets/{name}.prefab";
            var prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(newObj, path, InteractionMode.UserAction);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            AssetDatabase.AddObjectToAsset(mesh, asset);

            if (saveAndRefresh) {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            return path;
        }

        [MenuItem("Lunity/Delete Rock or Water Covered Objects in Children")]
        public static void Temp()
        {
            var objects = new List<GameObject>();
            for (var i = 0; i < Selection.gameObjects[0].transform.childCount; i++) {
                objects.Add(Selection.gameObjects[0].transform.GetChild(i).gameObject);
            }

            Undo.RecordObject(Selection.gameObjects[0], "Deleted rock-covered objects");
            foreach (var obj in objects) {
                var ray = new Ray(obj.transform.position + Vector3.up * 100f, Vector3.down);
                if (Physics.Raycast(ray, out var hit, 200f,  -1)) {
                    var layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                    if (layerName == "Rocks" || layerName == "Water") {
                        Debug.DrawLine(ray.origin, obj.transform.position, Color.red, 10f);
                        Object.DestroyImmediate(obj);
                    } else {
                        //Debug.DrawLine(hit.point, hit.point + Vector3.up * 200f, Color.green, 10f);
                    }
                } else {
                    //Debug.DrawLine(obj.transform.position + Vector3.up * 100f, obj.transform.position, Color.red, 10f);
                }
            }
        }
    }
    #endif
}