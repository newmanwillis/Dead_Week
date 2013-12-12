using UnityEngine;
using System.Collections;

public class ElectronicDoor2 : MonoBehaviour {

	public AudioClip AccessDenied;
	private GameObject RedLight;
	private GameObject ClosedDoor;
	private GameObject OpenDoor;

	private bool Opened = false;
	private bool Openable {get; set;}
	private bool LightCurrentlyFlashing = false;
	private float blinkTime = 0.6f;



	// Use this for initialization
	void Start () {
		Openable = false;
		OpenDoor = transform.FindChild("OpenDoor").gameObject;
		ClosedDoor = transform.FindChild("ClosedDoor").gameObject;
		RedLight = transform.FindChild("ClosedDoor").FindChild("redLightSprite").gameObject;
	}
	
	void OnTriggerStay(Collider other){

		if(other.tag == "Player" && Input.GetKey(KeyCode.A) && !Opened){
			if(!Openable && !LightCurrentlyFlashing){
				print ("light flashing");
				LightCurrentlyFlashing = true;
				transform.audio.clip = AccessDenied;
				transform.audio.Play();
				StartCoroutine(BlinkLight());
			}
			else if(Openable){
				// open door
				Opened = true;
				ClosedDoor.SetActive(false);
				OpenDoor.SetActive(true);
				// destroy the trigger
			}
		}
	}

	IEnumerator BlinkLight(){
		float startTime = Time.time;
		float fullTime = startTime + blinkTime;

		while(Time.time < fullTime){
			float curTime = Time.time - startTime;
			if(curTime < 0.2f || (curTime > 0.4f && curTime < 0.6f)) // || (curTime > 0.8f && curTime < 1f))
				RedLight.SetActive(false);
			else
			   RedLight.SetActive(true);
			yield return null;
		}
		RedLight.SetActive(true);
		LightCurrentlyFlashing = false;
	}

	public void MakeOpenable(){
		Openable = true;
		RedLight.SetActive(false);
	}
}
