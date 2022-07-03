using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SeamlessAudioLooper : MonoBehaviour
{
    [Header("AudioSource Settings")]
    public AudioClip Clip;
    public AudioMixerGroup MixerGroup;
    [Range(0f, 1f)] public float Volume = 1f;
    [Range(0f, 1f)] public float SpatialBlend = 0f;
    [Range(0, 255)] public int Priority = 128;
    public bool PlayOnAwake = true;

    [Header("Loop Settings")]
    public float CrossfadeOverlap = 10f;
    [Range(0.01f, 0.5f), Tooltip("If the audio source is shorter than the provided CrossfadeOverlap")] public float FallbackCrossfadeOverlap = 0.5f;
    public bool DoEqualPowerCrossfade = true;
    public bool DoInitialFade;

    private AudioSource _a;
    private AudioSource _b;

    private bool _initialized;

    public bool isPlaying => _a.isPlaying || _b.isPlaying;
    
    public void Awake()
    {
        CreateSources();
        if (!_initialized) Initialize();
        if (PlayOnAwake) Play();
    }

    private void CreateSources()
    {
        if (_a == null) _a = CreateSource("SourceA");
        if (_b == null) _b = CreateSource("SourceB");
    }

    public void SetClip(AudioClip clip)
    {
        CreateSources();
        Clip = clip;
        if (!_initialized) Initialize();
        else {
            _a.clip = Clip;
            _b.clip = Clip;
        }
    }

    private AudioSource CreateSource(string n)
    {
        var source = new GameObject(gameObject.name + "_" + n);
        source.transform.parent = transform;
        source.SetActive(false);
        return source.AddComponent<AudioSource>();
    }

    public void Initialize()
    {
        _a.spatialBlend = SpatialBlend;
        if(MixerGroup != null) _a.outputAudioMixerGroup = MixerGroup;
        _a.clip = Clip;
        _a.playOnAwake = false;
        _a.priority = Priority;
        _a.gameObject.SetActive(true);
        
        _b.spatialBlend = SpatialBlend;
        if(MixerGroup != null) _b.outputAudioMixerGroup = MixerGroup;
        _b.clip = Clip;
        _b.playOnAwake = false;
        _b.priority = Priority;
        _b.gameObject.SetActive(true);

        _initialized = true;
    }

    public void Play()
    {
        if (DoInitialFade) StartCoroutine(DoInitialFadeRoutine());
        else {
            _a.Play();
            _a.volume = 1f;
            StartCoroutine(DoOverlapPlay());
        }
    }

    public void Pause()
    {
        StopAllCoroutines();
        _a.Pause();
        _b.Pause();
        _a.volume = 0f;
        _b.volume = 0f;
    }

    public void UnPause()
    {
        if (DoInitialFade) StartCoroutine(DoInitialFadeRoutine());
        else {
            _a.UnPause();
            _a.volume = 1f;
            StartCoroutine(DoOverlapPlay());
        }
    }

    public void Stop()
    {
        StopAllCoroutines();
        _a.Stop();
        _b.Stop();

        _a.volume = 0f;
        _b.volume = 0f;
    }

    public void Update()
    {
        _a.spatialBlend = SpatialBlend;
        _b.spatialBlend = SpatialBlend;
        _a.priority = Priority;
        _b.priority = Priority;
    }

    private IEnumerator DoInitialFadeRoutine()
    {
        _a.volume = 0f;
        _a.Play();
        var fadeTime = GetFadeTime();
        for (var i = 0f; i < 1f; i += Time.deltaTime / fadeTime) {
            _a.volume = Volume * i;
            yield return null;
        }

        StartCoroutine(DoOverlapPlay());
    }

    private IEnumerator DoOverlapPlay()
    {
        var fadeTime = GetFadeTime();
        
        //wait for A to be almost done
        while (_a.time < Clip.length - fadeTime) {
            _a.volume = Volume;
            yield return null;
        }

        //crossfade
        _b.volume = 0f;
        _b.Play();
        for (var i = 0f; i < 1f; i += Time.deltaTime / fadeTime) {
            _a.volume = Volume * (DoEqualPowerCrossfade ? LunityMath.GetCrossfade(i * 2f - 1f, true) : 1f - i);
            _b.volume = Volume * (DoEqualPowerCrossfade ? LunityMath.GetCrossfade(i * 2f - 1f, false) : i);
            yield return null;
        }
        
        //stop A
        _a.volume = 0f;
        _a.Stop();
        
        //wait for B to be almost done
        while (_b.time < Clip.length - fadeTime) {
            _b.volume = Volume;
            yield return null;
        }
        
        //crossfade
        _a.volume = 0f;
        _a.Play();
        for (var i = 0f; i < 1f; i += Time.deltaTime / fadeTime) {
            _b.volume = Volume * (DoEqualPowerCrossfade ? LunityMath.GetCrossfade(i * 2f - 1f, true) : 1f - i);
            _a.volume = Volume * (DoEqualPowerCrossfade ? LunityMath.GetCrossfade(i * 2f - 1f, false) : i);
            yield return null;
        }
        
        //stop A
        _b.volume = 0f;
        _b.Stop();
        
        //start the whole thing again
        StartCoroutine(DoOverlapPlay());
    }

    private float GetFadeTime() => CrossfadeOverlap < Clip.length ? CrossfadeOverlap : FallbackCrossfadeOverlap * Clip.length;
}
