﻿using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	
	public Transform BlackScreen;
	
	private Transform PlayerT;
	private Transform BlackScreenCopy;
	private tk2dSprite BlackScreenSprite;
	
	// Use this for initialization
	void Start () {
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
		newPos.z = -15;
		BlackScreenCopy = ((Transform)(Instantiate(BlackScreen, newPos, Quaternion.identity)));
		BlackScreenSprite = BlackScreenCopy.GetComponent<tk2dSprite>();
		newPos.z = -50;
		transform.position = newPos;
		//PlayerT.position = newPos;	
		// print("moved PlayerT: " + PlayerT.position);
		
		StartCoroutine(HoldDeathScreen());
	}
	
	IEnumerator FadeInBlack(){
		
		yield return new WaitForSeconds(2);
		
	}
	
	IEnumerator HoldDeathScreen(){
		
		yield return new WaitForSeconds(6);
		// print ("called reload on player");
		GetComponent<Player>().ReloadPlayerAfterDeath();
	}
	
}
