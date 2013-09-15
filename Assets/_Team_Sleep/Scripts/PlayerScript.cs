﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public Texture sprite;
	public float scale;
	public int maxHealth;
	public int health;
	
	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(sprite.width*scale, 1, sprite.height*scale);
		Debug.Log("width: " + sprite.width + " height: " + sprite.height);
		
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
