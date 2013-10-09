using UnityEngine;
using System.Collections;

public class RepeatAttackAnimation : MonoBehaviour {
	private tk2dSpriteAnimator animator;
	private string left = "Left Attack";
	private string right = "Right Attack";
	private bool onLeft = true;
	
	// Use this for initialization
	void Start () {
		animator = GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!animator.Playing) {
			if (onLeft) {
				animator.Play(right);
			} else {
				animator.Play(left);
			}
			onLeft = !onLeft;
		}
	}
}
