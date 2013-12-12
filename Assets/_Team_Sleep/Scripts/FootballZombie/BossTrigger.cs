using UnityEngine;
using System.Collections;

public class BossTrigger : MonoBehaviour {

	private float LerpTime = 2;

	private Vector3 NewCamPos;
	private float NewCamSize = 220;
	//private Vector3 NewBlockPos;

	private FootballZombieSM FZSM;


	private Transform MainCamera;
	private GameObject BlockEntrance;

	// Use this for initialization
	void Start () {
		NewCamPos = new Vector3(0, 42.83717f, -500);
		//NewBlockPos = new Vector3(5.091614f, -101.8378f, 0);
		FZSM = GameObject.Find("FootballZombie").GetComponent<FootballZombieSM>();
		BlockEntrance = GameObject.Find("Referee Zombie Bottom");
		BlockEntrance.SetActive(false);

	}

	void OnTriggerEnter(Collider other){

		if(other.tag == "Player"){
			GetComponent<BoxCollider>().enabled = false;		// Turn off this collider, we don't need it anymore
			MainCamera = other.transform.FindChild("Main Camera");
			MainCamera.parent = null;

			BlockEntrance.SetActive(true);

			StartCoroutine(LerpCamera());
			StartCoroutine(StartZombieCrowd());
		}

	}

	IEnumerator StartZombieCrowd(){

		transform.FindChild("LeftFanSection").FindChild("ZombieFansTop1").GetComponent<tk2dSpriteAnimator>().Play();
		transform.FindChild("LeftFanSection").FindChild("ZombieFansTop2").GetComponent<tk2dSpriteAnimator>().Play();

		yield return new WaitForSeconds(1.1f);
		transform.FindChild("RightFanSection").FindChild("ZombieFansTop1").GetComponent<tk2dSpriteAnimator>().Play();
		transform.FindChild("RightFanSection").FindChild("ZombieFansTop2").GetComponent<tk2dSpriteAnimator>().Play();

	}

	IEnumerator LerpCamera(){

		Vector3 origCamPos = MainCamera.position;
		float origCamSize = MainCamera.camera.orthographicSize;
		//Vector3 origBlockPos = BlockEntrance.transform.position;

		float fullTime = Time.time + LerpTime;

		while(Time.time < fullTime){
			float curTime = fullTime - Time.time;
			MainCamera.position = Vector3.Lerp(origCamPos, NewCamPos, 1 - (curTime/LerpTime));
			MainCamera.camera.orthographicSize = Mathf.Lerp(origCamSize, NewCamSize, 1 - (curTime/LerpTime));
			//BlockEntrance.transform.position = Vector3.Lerp(origBlockPos, NewBlockPos, 1 - (curTime/LerpTime));
			yield return null;
		}

		yield return new WaitForSeconds (1);
		FZSM.SetStateToChase();			// Activate Boss
	}

}
