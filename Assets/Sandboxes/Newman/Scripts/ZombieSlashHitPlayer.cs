﻿using UnityEngine;
using System.Collections;

public class ZombieSlashHitPlayer : MonoBehaviour {
	
	private tk2dSpriteAnimator anim;
	private bool _hitPlayer = false;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<tk2dSpriteAnimator>();
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag == "Player" && !_hitPlayer  && anim.ClipTimeSeconds > 0.2){  //......
			_hitPlayer = true;
			int damage = 1;
			other.GetComponent<Player>().GotHit(damage);
		}
	}
}
