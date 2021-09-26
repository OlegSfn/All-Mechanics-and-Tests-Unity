using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
	// Skyboxes 
	[SerializeField] private Material daySkybox, nightSkybox;

	// Params
	[SerializeField] private float dayDuration, nightDuration;
	[SerializeField] private Light sun, moon;

	[SerializeField] private AnimationCurve sunIntensityCurve, moonIntensityCurve;
	[SerializeField] private AnimationCurve skyboxChangeCurve;

	// vars
	[Range(0f, 1f)]
	public float timeOfDay = 0;
	private float defaultSunIntensity, defaultMoonIntensity;



	private void Awake()
	{
		defaultSunIntensity = sun.intensity;
		defaultMoonIntensity = moon.intensity;
	}

	private void Update()
	{
		timeOfDay += Time.deltaTime / dayDuration;
		if (timeOfDay >= 1) timeOfDay = 0;

		RenderSettings.skybox.Lerp(nightSkybox, daySkybox, skyboxChangeCurve.Evaluate(timeOfDay));
		RenderSettings.sun = skyboxChangeCurve.Evaluate(timeOfDay) > 0.1f ? sun : moon;
		Debug.Log(skyboxChangeCurve.Evaluate(timeOfDay));
		DynamicGI.UpdateEnvironment();

		sun.transform.localRotation = Quaternion.Euler(timeOfDay * 360f, -35, 0);
		moon.transform.localRotation = Quaternion.Euler(timeOfDay * 360f + 180f, -35, 0);

		sun.intensity = defaultSunIntensity * sunIntensityCurve.Evaluate(timeOfDay);
		moon.intensity = defaultMoonIntensity * moonIntensityCurve.Evaluate(timeOfDay);
	}
}
