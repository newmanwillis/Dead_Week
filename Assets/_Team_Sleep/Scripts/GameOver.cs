using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	
	public Transform BlackScreen;
	
	private Transform PlayerT;
	private Transform BlackScreenCopy;
	private tk2dSprite BlackScreenSprite;
	private AudioListener AudioListen;
	
	// Use this for initialization
	void Start () {

		//transform.FindChild("Main Camera").GetComponents<AudioListener>().

		AudioListen = transform.FindChild("Main Camera").GetComponent<AudioListener>();


		//PlayerT =  GameObject.Find("Player").transform;
		// BlackScreen.renderer.material.color = Color.clear;
		//BlackScreen.Rotate(new Vector3(270,0,0));
		
		//
		// StartGameOverSequence();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void StartGameOverSequence(/*Transform player*/){
		// move player forward in Z (handle this in player script, will try to move him back to -0.01)
		// instantiate black screen at 0 alpha
		// fade to full black at some point in animation
		// fade in "Game Over"  -- make sure these aspects arent affected by light;
		// fade in menu options - "continue", "quit"
		// call playerscript to then reset it checkpoint or start over.
		
		//Player = player;
		
		// Move player forward in Z zone
		Vector3 newPos = transform.position; // PlayerT.position;
		newPos.z = -100;
		BlackScreenCopy = ((Transform)(Instantiate(BlackScreen, newPos, Quaternion.identity)));
		BlackScreenSprite = BlackScreenCopy.GetComponent<tk2dSprite>();
		newPos.z = -200;
		transform.position = newPos;
		//PlayerT.position = newPos;	
		// print("moved PlayerT: " + PlayerT.position);
		
		StartCoroutine(HoldDeathScreen());
	}
	
	IEnumerator FadeInBlack(){
		
		yield return new WaitForSeconds(2);
		
	}
	
	IEnumerator HoldDeathScreen(){
		//AudioListen.enabled = false;

		yield return new WaitForSeconds(2.2f);

		BlackScreenCopy.GetChild(0).gameObject.SetActive(true);

		/*
		float lerpTime = 2f;
		float fullTime = Time.time + lerpTime;
		Color colorVector = TextSprite.color;
		
		while(Time.time < fullTime){
			float curTime = fullTime - Time.time;
			colorVector.a = Mathf.Lerp(1, 0, 1 - (curTime/lerpTime));
			TextSprite.color = colorVector;
			yield return null;
		}*/



		yield return new WaitForSeconds(4f);
		// print ("called reload on player");
		GetComponent<Player>().ReloadPlayerAfterDeath();
		//AudioListen.enabled = true;
	}
	
}
