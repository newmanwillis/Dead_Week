using UnityEngine;
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
	
	private tk2dSpriteAnimator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<tk2dSpriteAnimator>();  // possibly null
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && hackSoFar < timeToHackSeconds) {
			beingHacked = true;
			if (anim != null) {
				if (hackSoFar == 0) {
					anim.Play("hacking");
				} else {
					anim.Resume();
				}
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			beingHacked = false;
			if (hackSoFar < timeToHackSeconds) {
				anim.Pause();
			}
		}
	}
	
	void OnTriggerStay(Collider other) {
		if (other.tag == "Player" && hackSoFar < timeToHackSeconds) {
			hackSoFar += Time.deltaTime;
			if (hackSoFar >= timeToHackSeconds) {
				hackSoFar = timeToHackSeconds;
				beingHacked = false;
				
				if (anim != null) {
					anim.Play("hackFinished");
				}
				
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
