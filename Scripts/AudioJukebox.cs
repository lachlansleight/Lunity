using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{

    public class AudioJukebox : MonoBehaviour
    {

        public AudioClip[] Clips;

        [Header("Settings")]
        public bool RandomizeOrder;
        [Space(10)]
        public float VolumeCenter = 1f;
        public bool RandomizeVolume;
        [Range(0f, 1f)] public float VolumeRandomizeAmount = 0.05f;
        [Space(10)]
        public float PitchCenter = 1f;
        public bool RandomizePitch;
        [Range(0f, 1f)] public float PitchRandomizeAmount = 0.05f;

        private List<AudioClip> _queue;
        private AudioSource _audioSource;

        public void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null) {
                _audioSource = GetComponentInChildren<AudioSource>();
            }

            _queue = new List<AudioClip>();
        }
        
        public void RebuildQueue()
        {
            _queue.Clear();
            var indices = new List<int>();
            for (var i = 0; i < Clips.Length; i++) {
                indices.Add(i);
            }

            while (indices.Count > 0) {
                var index = RandomizeOrder ? indices[Random.Range(0, indices.Count)] : indices[0];
                indices.Remove(index);
                _queue.Add(Clips[index]);
            }
        }
        
        public AudioClip NextClip()
        {
            if (_queue.Count == 0) RebuildQueue();
            var clip = _queue[0];
            _queue.RemoveAt(0);
            return clip;
        }

        public void Play()
        {
            var clip = NextClip();
            _audioSource.clip = clip;
            _audioSource.pitch = PitchCenter + (RandomizePitch 
                ? Random.Range(-PitchRandomizeAmount, PitchRandomizeAmount) 
                : 0f);
            _audioSource.volume = VolumeCenter + (RandomizeVolume 
                ? Random.Range(-VolumeRandomizeAmount, VolumeRandomizeAmount) 
                : 0f);
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }

        public float Volume => _audioSource.volume;
    }
}