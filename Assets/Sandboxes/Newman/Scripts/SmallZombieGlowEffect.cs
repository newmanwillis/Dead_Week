using UnityEngine;
using System.Collections;

public class SmallZombieGlowEffect : MonoBehaviour {

	private Vector3 LightPos;
	private float FirstLightZPos;
	private float SecondLightZPos;
	private float LerpTime = 0.8f;

	// Use this for initialization
	void Start () {
	
		FirstLightZPos = transform.position.z;
		SecondLightZPos = FirstLightZPos + 7;
		//SecondLightZPos.z += 5;

		StartCoroutine(MoveLightToSecondPos());
	}
	
	IEnumerator MoveLightToSecondPos(){

		float fullTime = Time.time + LerpTime;

		while(Time.time < fullTime){
			float curTime = fullTime - Time.time;
			LightPos = transform.position;

			LightPos.z = Mathf.Lerp(FirstLightZPos, SecondLightZPos, 1 - (curTime/LerpTime));
			transform.position = LightPos;
			yield return null;
		}
		yield return new WaitForSeconds(0.25f);
		StartCoroutine(MoveLightToFirstPos());
	}


	IEnumerator MoveLightToFirstPos(){
		
		float fullTime = Time.time + LerpTime;
		
		while(Time.time < fullTime){
			float curTime = fullTime - Time.time;
			LightPos = transform.position;
			LightPos.z = Mathf.Lerp(SecondLightZPos, FirstLightZPos, 1 - (curTime/LerpTime));
			transform.position = LightPos;
			yield return null;
		}
		yield return new WaitForSeconds(0.15f);
		StartCoroutine(MoveLightToSecondPos());
	}


}
