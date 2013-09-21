﻿using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {
	public int damage;
	public bool disappearOnCollide;
	public bool dealDamageOnce;
	bool dealtDamage = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other) {
		if (other.tag == "Zombie" && !(dealDamageOnce && dealtDamage)) {
			other.GetComponent<ZombieHealth>().health -= damage;
			dealtDamage = true;
		}
		if(other.tag != "Player" && other.tag != "Attack" && other.tag != "ZombieDetectionRange" && other.tag != "ZombieAttack"){
			if (disappearOnCollide) {
				Destroy(gameObject);
			}	
		}
	}
}