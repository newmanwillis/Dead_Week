using UnityEngine;
using System.Collections;

public class DieAfterAnimation : MonoBehaviour {
	private tk2dSpriteAnimator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!anim.Playing) {
			Destroy(gameObject);
		}
	}
}
