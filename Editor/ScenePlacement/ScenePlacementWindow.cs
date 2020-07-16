using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lunity
{
    public class ScenePlacementWindow : EditorWindow
    {

        private static ScenePlacementWindow _window;
        private static ScenePlacementConfig _config;

        private int _currentSet;

        private Collider _targetCollider;
        private List<GameObject> _prefabOptions;
        private Transform _parentTransform;
        private bool _randomiseRotationY;
        private bool _randomiseScale;
        private float _randomiseScaleAmount;

        private bool _needsChoice;
        private GameObject _prefabChoice;
        private Quaternion _nextRotation;
        private Vector3 _nextScale;

        private Mesh _previewMesh;
        private Material _previewMaterial;

        [MenuItem("Lunity/Scene Placement")]
        static void ShowWindow()
        {
            _window = (ScenePlacementWindow) GetWindow(typeof(ScenePlacementWindow));
            _window.titleContent = new GUIContent("LunityPlacement");
            _window.Show();
        }

        void OnGUI()
        {
            if (_config == null) _config = ScenePlacementConfig.GetAssetInstance();
            
            EditorGUILayout.LabelField("Lunity Scene Placement", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (!_config.HasSets()) {
                if (GUILayout.Button("Add a Prefab Set to Begin!")) {
                    _config.AddPrefabSet();
                    EditorUtility.SetDirty(_config);
                }
            } else {
                if (_currentSet >= _config.PrefabSets.Count) _currentSet = _config.PrefabSets.Count - 1;
                if (_currentSet < 0) _currentSet = 0;
                
                EditorGUI.BeginChangeCheck();
                _config.PrefabSets[_currentSet].Name = EditorGUILayout.TextField("Set Name", _config.PrefabSets[_currentSet].Name);
                if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(_config);
                EditorGUILayout.BeginHorizontal();
                if(_config.PrefabSets.Count > 1) {
                    if (GUILayout.Button("<")) {
                        _currentSet--;
                        if (_currentSet < 0) _currentSet = _config.PrefabSets.Count - 1;
                    }
                }

                if (GUILayout.Button("Add New Set")) {
                    _config.AddPrefabSet();
                    _currentSet = _config.PrefabSets.Count - 1;
                    EditorUtility.SetDirty(_config);
                }

                if (GUILayout.Button("Delete Set")) {
                    _config.RemovePrefabSet(_currentSet);
                    if (_currentSet >= _config.PrefabSets.Count) _currentSet = _config.PrefabSets.Count - 1;
                    EditorUtility.SetDirty(_config);
                }

                if (_config.PrefabSets.Count > 1) {
                    if (GUILayout.Button(">")) {
                        _currentSet++;
                        if (_currentSet >= _config.PrefabSets.Count) _currentSet = 0;
                    }
                }

                EditorGUILayout.EndHorizontal();

                if (_config.HasSets()) {
                    EditorGUILayout.Space();
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                        if (_config.PrefabSets[_currentSet].Options.Count == 0)
                            EditorGUILayout.LabelField("Add a Prefab to Begin!");
                        else {
                            for (var i = 0; i < _config.PrefabSets[_currentSet].Options.Count; i++) {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUI.BeginChangeCheck();
                                _config.PrefabSets[_currentSet].Options[i] =
                                    (GameObject) EditorGUILayout.ObjectField(_config.PrefabSets[_currentSet].Options[i],typeof(GameObject), false);
                                if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(_config);

                                if (GUILayout.Button("X")) {
                                    _config.PrefabSets[_currentSet].Options.RemoveAt(i);
                                    EditorUtility.SetDirty(_config);
                                }

                                EditorGUILayout.EndHorizontal();
                            }
                        }

                        if (GUILayout.Button("Add Prefab")) {
                            _config.PrefabSets[_currentSet].Options.Add(null);
                            EditorUtility.SetDirty(_config);
                        }
                    }

                    EditorGUILayout.Space();
                }
            }
            
            EditorGUILayout.Space();

            /*
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                if(_prefabOptions == null || _prefabOptions.Count == 0) EditorGUILayout.LabelField("No prefabs added yet");
                else {
                    for (var i = 0; i < _prefabOptions.Count; i++) {
                        EditorGUILayout.BeginHorizontal();
                        _prefabOptions[i] = (GameObject) EditorGUILayout.ObjectField(_prefabOptions[i], typeof(GameObject), false);
                        if (GUILayout.Button("X")) {
                            _prefabOptions.RemoveAt(i);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }

                if (GUILayout.Button("Add Prefab")) {
                    if (_prefabOptions == null) _prefabOptions = new List<GameObject>();
                    _prefabOptions.Add(null);
                }
            }
            */

            _targetCollider = (Collider)EditorGUILayout.ObjectField("Target Collider (optional)", _targetCollider, typeof(Collider), true);
            _parentTransform = (Transform) EditorGUILayout.ObjectField("Parent Transform", _parentTransform, typeof(Transform), true);
            _randomiseRotationY = EditorGUILayout.Toggle("Randomise Y Rotation", _randomiseRotationY);
            _randomiseScale = EditorGUILayout.Toggle("Randomise Scale", _randomiseScale);
            if (_randomiseScale) {
                _randomiseScaleAmount = EditorGUILayout.Slider("Randomise Scale Amount", _randomiseScaleAmount, 0f, 1f);
            }
        }

        private void OnEnable() {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
            
        }
 
        private void OnDestroy() {
            SceneView.duringSceneGui -= OnSceneGUI;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
 
        private void OnSceneGUI(SceneView sceneView)
        {
            if (_config == null) _config = ScenePlacementConfig.GetAssetInstance();
            if(_window == null) _window = (ScenePlacementWindow) GetWindow(typeof(ScenePlacementWindow));
            _window.Repaint();

            var e = Event.current;
            var modifiersHeld = (e.shift && e.control);
            var buttonClicked = (e.type == EventType.MouseDown && e.button == 1);
            var wheel = modifiersHeld && e.type == EventType.ScrollWheel ? (int)Mathf.Sign(e.delta.y) : 0;

            if (wheel != 0) {
                var pre = _currentSet;
                _currentSet += wheel;
                if (_currentSet < 0) _currentSet = _config.PrefabSets.Count - 1;
                if (_currentSet >= _config.PrefabSets.Count) _currentSet = 0;
                
                if (pre != _currentSet) _needsChoice = true;
            }

            if (modifiersHeld && _needsChoice) {
                NextChoice();
            }
            
            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            bool didHit;
            RaycastHit hit;
            didHit = _targetCollider == null ? Physics.Raycast(ray, out hit) : _targetCollider.Raycast(ray, out hit, float.MaxValue);
            
            if (modifiersHeld && buttonClicked && _prefabChoice != null) {

                if (didHit) {
                    CreateObject(_prefabChoice, hit.point, _nextRotation, _nextScale, _parentTransform);
                    _needsChoice = true;
                }
            }

            if (!modifiersHeld) {
                _needsChoice = true;
                return;
            }

            if (_prefabChoice != null) {
                Graphics.DrawMesh(_previewMesh, Matrix4x4.TRS(hit.point, _nextRotation, _nextScale), _previewMaterial, 0, sceneView.camera);
            }

            //apply cursor change
            //this comes last since it consumes the event to stop scene view rotation while keys held
            EditorGUIUtility.AddCursorRect(new Rect(0, 0, 5000, 5000), MouseCursor.ArrowPlus);
            if(e.type != EventType.Repaint && e.type != EventType.Layout) e.Use();
        }

        private void NextChoice()
        {
            _needsChoice = false;
            
            if (!_config.HasSets()) return;
            _prefabChoice = _config.PrefabSets[_currentSet].GetRandomPrefab();
            if (_prefabChoice == null) return;

            _previewMesh = _prefabChoice.GetComponent<MeshFilter>().sharedMesh;
            _previewMaterial = _prefabChoice.GetComponent<Renderer>().sharedMaterial;
            
            _nextRotation = _randomiseRotationY
                ? Quaternion.Euler(0f, Random.Range(0f, 360f), 0f)
                : Quaternion.identity;
            _nextScale = _randomiseScale
                ? Vector3.one * Random.Range(1f - _randomiseScaleAmount, 1f + _randomiseScaleAmount)
                : Vector3.one;
        }

        private void CreateObject(GameObject creationSource, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
        {
            var newObj = PrefabUtility.InstantiatePrefab(creationSource) as GameObject;
            if (newObj == null) return;

            newObj.name = creationSource.name;
            var tr = newObj.transform;
            tr.position = position;
            tr.rotation = rotation;
            tr.localScale = scale;
            if (parent != null) tr.SetParent(parent);

            Undo.RegisterCreatedObjectUndo(newObj, "Create object");
        }
    }
}