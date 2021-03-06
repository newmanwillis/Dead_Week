﻿using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	private GameObject childObj;
	public bool enableChildWhenPowered;
	public float poweredTime;
	private float unpowerTime;
	private bool currentlyPowered;
	public bool noTimeLimit;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < transform.childCount; i++) {
			// A button controls either a door or an electric gate
			// either way, it must be named "ControlledByButton"
			if (transform.GetChild(i).name == "ControlledByButton") {
				childObj = transform.GetChild(i).gameObject;
				break;
			}
		}
		unpowerTime = 0;
		currentlyPowered = false;
		childObj.SetActive(!enableChildWhenPowered);
	}
	
	// Update is called once per frame
	void Update () {
		if ((!noTimeLimit) && currentlyPowered && unpowerTime < Time.time) {
			childObj.SetActive(!enableChildWhenPowered);
			GetComponent<tk2dSprite>().SetSprite("laserbuttonOFFcropped");
			RepeatedlySpawn spawner = GetComponent<RepeatedlySpawn>();
			if (spawner != null) {
				spawner.spawning = !enableChildWhenPowered;
			}
		}
	}
	
	public void power() {
		childObj.SetActive(enableChildWhenPowered);
		currentlyPowered = true;
		GetComponent<tk2dSprite>().SetSprite("laserbuttonONcropped");
		RepeatedlySpawn spawner = GetComponent<RepeatedlySpawn>();
		if (spawner != null) {
			spawner.spawning = enableChildWhenPowered;
		}
		unpowerTime = Time.time + poweredTime;
	}
	
	public void powerForever() {
		childObj.SetActive(enableChildWhenPowered);
		RepeatedlySpawn spawner = GetComponent<RepeatedlySpawn>();
		if (spawner != null) {
			spawner.spawning = enableChildWhenPowered;
		}
		currentlyPowered = true;
		noTimeLimit = true;
	}
}
