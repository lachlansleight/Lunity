using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    [ExecuteAlways]
    public class BasicFader : MonoBehaviour
    {
        public bool FollowMainCamera;
        
        public Color TargetColor = Color.black;
        [Range(0f, 1f)] public float TargetAlpha = 0f;
        [Range(0.1f, 24f)] public float LerpSpeed = 4f;
        [Space(10)]
        [ReadOnly] public Color CurrentColor;
        [ReadOnly] public float CurrentAlpha;
        
        private Renderer _renderer;
        private Renderer Renderer
        {
            get
            {
                if (_renderer == null) _renderer = GetComponent<Renderer>();
                return _renderer;
            }
        }
        
        
        private Material _material;
        private Material Material
        {
            get
            {
                if (!Application.isPlaying) return Renderer.sharedMaterial;
                
                if (_material == null) {
                    _material = Renderer.material;
                }
                return _material;
            }
        }
        
        private Color _actualColor;

        private Transform _cameraTransform;
        private Transform CameraTransform
        {
            get
            {
                if (_cameraTransform == null) _cameraTransform = Camera.main.transform;
                return _cameraTransform;
            }
        }

        private static BasicFader _instance;

        private static BasicFader Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<BasicFader>();
                if (_instance == null) throw new UnityException("There is no BasicFader in the scene!");
                return _instance;
            } 
        }

        public void Awake()
        {
            if (FollowMainCamera) {
                if (CameraTransform == null) {
                    Debug.LogError("Error - no MainCamera found in the scene, turning FollowMainCamera off", this);
                    FollowMainCamera = false;
                }
            }
        }

        public void OnDestroy()
        {
            _instance = null;
        }

        public void OnRenderObject()
        {
            if (FollowMainCamera) {
                transform.position = CameraTransform.position;
            }
        }

        public void Update()
        {
            CurrentColor = Color.Lerp(CurrentColor, TargetColor, Time.deltaTime * LerpSpeed);
            CurrentAlpha = Mathf.Lerp(CurrentAlpha, TargetAlpha, Time.deltaTime * LerpSpeed);
            
            _actualColor = CurrentColor;
            _actualColor.a = CurrentAlpha;

            if (CurrentAlpha < 0.01f) {
                Renderer.enabled = false;
                return;
            }

            Renderer.enabled = true;
            Material.color = _actualColor;
        }

        public static void SetTargetColor(Color color)
        {
            Instance.TargetColor = color;
        }

        public static void SetColor(Color color)
        {
            Instance.TargetColor = color;
            Instance.CurrentColor = color;
        }
        
        public static void SetTargetAlpha(float alpha)
        {
            Instance.TargetAlpha = alpha;
        }

        public static void SetAlpha(float alpha)
        {
            Instance.TargetAlpha = alpha;
            Instance.CurrentAlpha = alpha;
        }

        public static float GetAlpha()
        {
            return Instance.CurrentAlpha;
        }

        public static void SetLerpSpeed(float lerpSpeed)
        {
            Instance.LerpSpeed = lerpSpeed;
        }
    }
}