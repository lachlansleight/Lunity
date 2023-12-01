using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lunity
{
    /// Simple class to manage crossfading between audio sources.
    /// By default, it will look for AudioSources on direct children, although sources can be manually provided
    public class AudioCrossfadeGroup : MonoBehaviour
    {
        public int CurrentIndex;
        public int TargetIndex;
        [Range(-1f, 1f)] public float Crossfade = -1f;

        [FormerlySerializedAs("_sources")]
        public List<AudioSource> Sources;
        
        public void Awake()
        {
            if (Sources == null || Sources.Count == 0) {
                Sources = new List<AudioSource>();
                for (var i = 0; i < transform.childCount; i++) {
                    var a = transform.GetChild(i).GetComponent<AudioSource>();
                    if (a != null) Sources.Add(a);
                }
            }
            if (Sources.Count == 0) {
                Debug.LogWarning("No AudioSources found on direct children of " + gameObject.name + " - disabling", this);
                enabled = false;
            }
        }

        public void Update()
        {
            if (CurrentIndex >= Sources.Count || TargetIndex >= Sources.Count) {
                Debug.LogError("Indices " + CurrentIndex + " -> " + TargetIndex + " out of range of audio source list (length " + Sources.Count + "). Disabling", this);
                enabled = false;
                return;
            }
            SetVolumes();
        }

        private void SetVolumes()
        {
            var matchingIndices = CurrentIndex == TargetIndex; 
            for (var i = 0; i < Sources.Count; i++) {
                if (i == CurrentIndex) Sources[i].volume = matchingIndices ? 1f : LunityMath.GetCrossfade(Crossfade, true);
                else if (i == TargetIndex) Sources[i].volume = matchingIndices ? 1f : LunityMath.GetCrossfade(Crossfade, false);
                else Sources[i].volume = 0f;
            }
        }
        
        [EditorButton]
        public void SwapIndices()
        {
            Crossfade *= -1f;
            (CurrentIndex, TargetIndex) = (TargetIndex, CurrentIndex);
            SetVolumes();
        }
    }
}
