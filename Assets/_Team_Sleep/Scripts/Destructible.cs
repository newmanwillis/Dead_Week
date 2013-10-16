﻿using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {
	private tk2dSpriteAnimator animator;
	
	private bool destroyed = false;
	
	public int nothingDropChance;
	public int healthDropChance;
	public Transform healthPickup;
	public int energyDropChance;
	public Transform energyPickup;
	
	private bool wasDisintigrated = false;
	void Start () {
		animator = GetComponent<tk2dSpriteAnimator>();
	}
	
	public void smash() {
		if (!destroyed) {
			if (animator.GetClipByName("smashed") != null) {
				animator.Play("smashed");
			} else {
				Destroy(GetComponent<tk2dSprite>());
			}
			destroyed = true;
			StartCoroutine(waitForAnimationAndDie());
		}
	}
	
	public void disintigrate() {
		if (animator.GetClipByName("Disintigrated") == null) {
			smash();
		} else if (!destroyed) {
			wasDisintigrated = true;
			animator.Play("Disintigrated");
			destroyed = true;
			StartCoroutine(waitForAnimationAndDie());
		}
	}
	
	void spawnPickups() {
		float roll = (float)Random.Range(0, 100) / 100;
		int totalChance = nothingDropChance + healthDropChance + energyDropChance;
		float nothingChance = (float)nothingDropChance / totalChance;
		float healthChance = (float)healthDropChance / totalChance;
		float energyChance = (float)energyDropChance / totalChance;
		
		if (roll < healthChance) {
			Instantiate(healthPickup, transform.position, Quaternion.identity);
		} else if (roll < healthChance + energyChance) {
			Instantiate(energyPickup, transform.position, Quaternion.identity);
		} // else nothing
	}
	
	IEnumerator waitForAnimationAndDie() {
		while (animator.Playing) {
			yield return null;
		}
		
		spawnPickups();
		Destroy(GetComponent<BoxCollider>());
		if (!wasDisintigrated && animator.GetClipByName("smashed_flashing") != null) {
			animator.Play("smashed_flashing");
		}
	}
}