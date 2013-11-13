using UnityEngine;
using System.Collections;

public class ChargingStation : MonoBehaviour {
	
	public float SpotLightTime = 2f;
	public float PointLightTime = 1f;
	
	//private float SwitchLightTime;
	
	private GameObject PointLight;
	private GameObject SpotLight;
	/*
	private float SmallRange;
	private float SmallIntensity;
	private float LargeRange;
	private float LargeIntensity;*/
	
	// Use this for initialization
	void Start () {
		PointLight = transform.FindChild("Point light").gameObject;
		SpotLight = transform.FindChild("Spot light").gameObject;
		
		PointLight.SetActive(true);
		SpotLight.SetActive(false);		

		StartCoroutine(SwitchToSpotLight(PointLightTime));
	}
	
	
	IEnumerator SwitchToSpotLight(float time){
		yield return new WaitForSeconds(time);
		PointLight.SetActive(false);
		SpotLight.SetActive(true);
		StartCoroutine(SwitchToPointLight(SpotLightTime));
	}
	
	IEnumerator SwitchToPointLight(float time){
		yield return new WaitForSeconds(time);
		PointLight.SetActive(true);
		SpotLight.SetActive(false);
		StartCoroutine(SwitchToSpotLight(PointLightTime));		
	}
}
