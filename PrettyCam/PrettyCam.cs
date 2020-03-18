using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera))]
public class PrettyCam : MonoBehaviour
{

    public Camera TargetCamera;
    
    public bool DisableRoll = true;
    
    public bool ApplySmoothing = true;
    [Range(0f, 16f)] public float LerpSpeed = 8f;
    
    public float JumpThreshold = 1f;
    
    public bool AllowKeyboardToggle = false;
    public KeyCode ToggleKey = KeyCode.Pause;

    private Camera _myCamera;
    
    private void Awake()
    {
        if (TargetCamera == null) TargetCamera = Camera.main;
        if (TargetCamera == null) {
            Debug.LogWarning("Pretty Cam couldn't find a Main Camera in the scene and none was applied - disabling!");
            gameObject.SetActive(false);
        }

        _myCamera = GetComponent<Camera>();
        _myCamera.nearClipPlane = TargetCamera.nearClipPlane;
        _myCamera.farClipPlane = TargetCamera.farClipPlane;
        _myCamera.clearFlags = TargetCamera.clearFlags;
        _myCamera.backgroundColor = TargetCamera.backgroundColor;
        _myCamera.orthographic = TargetCamera.orthographic;
    }
    
    private void Update()
    {
        var targetTransform = TargetCamera.transform;
        var myTransform = transform;
        
        //toggle
        if (AllowKeyboardToggle && Input.GetKeyDown(ToggleKey)) {
            _myCamera.enabled = !_myCamera.enabled;
            
            //when enabling the camera, instantly set position and rotation
            if (_myCamera.enabled) {
                UpdatePositionRotation(targetTransform, myTransform, false);
            }
        }

        //no need to update position etc if the camera isn't on
        if (!_myCamera.enabled) return;

        UpdatePositionRotation(targetTransform, myTransform, ApplySmoothing);
    }

    private void UpdatePositionRotation(Transform source, Transform target, bool smooth)
    {
        var sourcePos = source.position;
        var sourceRot = source.rotation;
        
        //we instantly jump to the target position if we are further than JumpThreshold
        var squareDistance = (target.position - sourcePos).sqrMagnitude;
        target.position = smooth && squareDistance < JumpThreshold * JumpThreshold
            ? Vector3.Lerp(target.position, sourcePos, Time.deltaTime * LerpSpeed)
            : sourcePos;

        target.rotation = smooth 
            ? Quaternion.Lerp(target.rotation, sourceRot, Time.deltaTime * LerpSpeed) 
            : sourceRot;

        if (!DisableRoll) return;
        var currentEulers = target.eulerAngles;
        target.eulerAngles = new Vector3(currentEulers.x, currentEulers.y, 0f);
    }
}
