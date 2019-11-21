using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSpinner : MonoBehaviour
{

    public int RingCount = 5;
    [Space(10)]
    public float MinSpeed = 50f;
    public float MaxSpeed = 150f;
    public bool AllowReverseSpeed = true;
    [Space(10)]
    [Range(0f, 1f)] public float MinSize = 0.3f;
    [Space(10)] 
    public Gradient RingColors = new Gradient();

    private Transform[] _rings;
    private float[] _speeds;

    public void Awake()
    {
        _rings = new Transform[RingCount];
        _speeds = new float[RingCount];
        _rings[0] = transform.Find("Ring");
        for (var i = 1; i < RingCount; i++) {
            _rings[i] = Instantiate(_rings[0], transform, false);
        }

        for (var i = 0; i < RingCount; i++) {
            var t = i / (RingCount - 1f);
            _rings[i].localScale = Vector3.one * Mathf.Lerp(1f, MinSize, t);
            _rings[i].GetComponent<Image>().color = RingColors.Evaluate(t);
        }

        GenerateSpeeds();
    }

    private void GenerateSpeeds()
    {
        for (var i = 0; i < _speeds.Length; i++) {
            _speeds[i] = Random.Range(MinSpeed, MaxSpeed) * (AllowReverseSpeed ? Mathf.Sign(Random.Range(-1f, 1f)) : 1f);
        }
    }

    private void Update()
    {
        for (var i = 0; i < _rings.Length; i++) {
            _rings[i].Rotate(0f, 0f, _speeds[i] * Time.deltaTime, Space.Self);
        }
    }
}
