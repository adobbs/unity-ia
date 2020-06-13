using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour {

	[SerializeField] private Material sky;
	[SerializeField] private Light sun;

	private float _fullIntensity;
	private float _cloudValue = 0f;

	void Start() {
		_fullIntensity = sun.intensity;
	}

	void Update() {
		SetOvercast(_cloudValue);
		_cloudValue += 0.005f;
	}

	private void SetOvercast(float value) {
		sky.SetFloat("_Blend", value);
		sun.intensity = _fullIntensity - (_fullIntensity * value); 
	}
}
