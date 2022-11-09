using UnityEngine;
using System.Collections;
//using VRTools;

public class StarController : MonoBehaviour {

	[Range(0f, 1f)] public float DebugValue = 0f;

	public float minSpinRate = 10f;
	public float maxSpinRate = 1000f;

	public float minSphereSize = 0.005f;
	public float maxSphereSize = 0.04f;

	public Color minSphereCol;
	public Color maxSphereCol;

	public Transform spinner;
	public Transform sphere;
	public Material sphereMat;

	float spinnerRot = 0f;

	public string deviceName;
	public string axis;

	// Use this for initialization
	void Start () {
		sphereMat = sphere.GetComponent<Renderer>().sharedMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		//VRInputDevice device = VRInput.GetDevice(deviceName);
		//Note - this functionality needs to be replaced for this to work
		//       basically it just needs a float from 0-1 every frame to define the 'intensity' level
		//var value = Mathf.Clamp01(device.GetAxis(axis));
		var value = DebugValue;

		//spin
		spinnerRot += Mathf.Lerp(minSpinRate, maxSpinRate, Mathf.Pow(Mathf.Clamp01(value), 3f)) * Time.deltaTime;
		spinner.localRotation = Quaternion.Euler(spinnerRot, -90f, 90f);

		//grow sphere
		sphere.localScale = Vector3.one * Mathf.Lerp(minSphereSize, maxSphereSize, Mathf.Clamp01(value));

		//glow
		sphereMat.SetColor("_EmissionColor", Color.Lerp(minSphereCol, maxSphereCol, Mathf.Clamp01(value)));
	}
}
