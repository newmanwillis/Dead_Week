﻿using UnityEngine;
using System.Collections;

public class ZombieHealth : MonoBehaviour {
	
	public int health = 100;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Attack"){
			health -= 50;
			if(health <= 0){
				Destroy(transform.parent.parent.gameObject);	
			}
			else{
				//Knockback();
			}
		}
	}
	
	void Knockback(){
		float xOffset = 1f;
		float yOffset = 1f;
		Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
		Vector3 newZombiePos = transform.position;
		//if(playerPos.x > transform.position){a
				
		//}
		
	}

}
