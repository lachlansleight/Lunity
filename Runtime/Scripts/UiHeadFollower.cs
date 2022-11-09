using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public class UiHeadFollower : MonoBehaviour
    {
        public float TargetDistance = 2f;
        public bool UseFixedHeightOffset = true;
        [Range(-2f, 2f)] public float FixedHeightOffset = -0.3f;
        [Space(10)]
        public bool SmoothPosition = true;
        [Range(0f, 20f)] public float PositionSmoothing = 18f;
        [Space(10)]
        public bool SmoothRotation = false;
        [Range(0f, 20f)] public float RotationSmoothing = 18f;
        public bool WorldUp = true;

        [Space(10)]
        public Transform TargetTransform;
        public bool AutoSearch = true;

        private bool _autoSearching = false;

        public void Update()
        {
            if (_autoSearching) return;
            
            if (TargetTransform == null) {
                TargetTransform = Camera.main.transform;
            }

            if (TargetTransform == null) {
                if (AutoSearch) {
                    StartCoroutine(DoAutoSearch());
                } else {
                    Debug.LogError(
                        "Failed to get transform from MainCamera. Try setting it explicitly, or enabling auto-search. Disabling UiHeadFollower",
                        this);
                    enabled = false;
                }
                return;
            }

            GetTargetPositionRotation(out var targetPosition, out var targetRotation);
            transform.position = SmoothPosition
                ? Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * (20.5f - PositionSmoothing))
                : targetPosition;
            transform.rotation = SmoothRotation
                ? Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * (20.5f - RotationSmoothing))
                : targetRotation;
        }

        /// Immediately moves the object to the target position and rotation, ignoring smoothing
        public void JumpToTarget()
        {
            GetTargetPositionRotation(out var targetPosition, out var targetRotation);
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }

        private void GetTargetPositionRotation(out Vector3 targetPosition, out Quaternion targetRotation)
        {
            var forward = TargetTransform.forward;
            if (UseFixedHeightOffset) {
                forward.y = 0f;
                forward.Normalize();
            }

            targetPosition = TargetTransform.position + forward * TargetDistance;
            if (UseFixedHeightOffset) {
                targetPosition.y = TargetTransform.position.y + FixedHeightOffset;
            }

            targetRotation = Quaternion.LookRotation(transform.position - TargetTransform.position,
                WorldUp ? Vector3.up : TargetTransform.up);
        }

        private IEnumerator DoAutoSearch()
        {
            _autoSearching = true;
            while (TargetTransform == null) {
                var cam = Camera.main;
                if (cam) {
                    TargetTransform = cam.transform;
                    _autoSearching = false;
                }

                yield return null;
            }
        }
    }
}