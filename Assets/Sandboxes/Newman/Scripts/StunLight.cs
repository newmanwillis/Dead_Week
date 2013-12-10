using UnityEngine;
using System.Collections;

public class StunLight : MonoBehaviour {

	public float StayLightTime = 2;
	public float DimLightTime = 4;

	// Use this for initialization
	void Start () {
		StartCoroutine(DimLight());
	}
	

	IEnumerator DimLight(){

		yield return new WaitForSeconds(StayLightTime);

		float startIntensity = transform.light.intensity;
		float fullTime = Time.time + DimLightTime;
		
		while(Time.time < fullTime){
			float curTime = fullTime - Time.time;
			transform.light.intensity = Mathf.Lerp(startIntensity, 0, 1 - (curTime/DimLightTime));
			yield return null;
		}		

	}

}
