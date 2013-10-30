﻿using UnityEngine;
using System.Collections;

public class HackableComputer : MonoBehaviour {
	public float timeToHackSeconds;
	
	public bool beingHacked = false;
	public float hackSoFar = 0;
	
	public string unlockUpgrade;
	public string textMessage;
	public Texture infoCard;
	
	// isGenerator should be set to true iff this is being used to restart a parent generator, 
	// which this gameobject is a child of.
	public bool isGenerator;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && hackSoFar < timeToHackSeconds) {
			beingHacked = true;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			beingHacked = false;
		}
	}
	
	void OnTriggerStay(Collider other) {
		if (other.tag == "Player" && hackSoFar < timeToHackSeconds) {
			hackSoFar += Time.deltaTime;
			if (hackSoFar >= timeToHackSeconds) {
				hackSoFar = timeToHackSeconds;
				beingHacked = false;
				//tk2dSprite mySprite = gameObject.GetComponent<tk2dSprite>();
				//mySprite.SetSprite("computerTerminalGreen");
				
				if (unlockUpgrade != null && unlockUpgrade != "") {
					GameObject.Find("Player").GetComponentInChildren<Player>().unlockUpgrade(unlockUpgrade);
				}
				if (textMessage != null && textMessage != "") {
					Camera.main.GetComponent<CameraControl>().pauseAndDrawTextMessage(textMessage);
				}
				if (infoCard != null) {
					Camera.main.GetComponent<CameraControl>().pauseAndDrawInfoCard(infoCard);
				}
				
				// We will use this script to power on generators as well
				if (isGenerator) {
					transform.parent.GetComponent<Generator>().IsRunning = true;
				}
			}
		}
	}
}
