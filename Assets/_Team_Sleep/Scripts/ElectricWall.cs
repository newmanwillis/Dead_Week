﻿using UnityEngine;
using System.Collections;

public class ElectricWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			other.GetComponent<Player>().GotHit(2);
		}
		if (other.tag == "Zombie") {
			other.GetComponent<ZombieHealth>().TakeDamage(4, ZombieHealth.HitTypes.burstLaser, false);
		}
		if (other.tag == "FootballZombie") {
			Debug.Log("hit boss");
			other.GetComponent<FootballZombieHealth>().becomeVulnerable();
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "FootballZombie") {
			Debug.Log("hit boss enter");
		}
	}
}
