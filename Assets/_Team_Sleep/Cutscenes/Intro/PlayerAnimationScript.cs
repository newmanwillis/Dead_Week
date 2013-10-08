using UnityEngine;
using System.Collections;

public class PlayerAnimationScript : MonoBehaviour {
	private tk2dSpriteAnimator curAnim;
	
	// Use this for initialization
	void Start () {
		curAnim = GetComponentInChildren<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void cutscenWalkLeft() {
		curAnim.Play("walkingLeft");
	}
	
	void cutsceneWalkRight() {
		curAnim.Play("walkingRight");
	}
	
	void swordAttackAnimation() {
		curAnim.Play("swordRight");
		StartCoroutine(waitForAnimationtoEnd());
	}
	
	IEnumerator waitForAnimationtoEnd(){
		while(curAnim.Playing){
			yield return null;
		}
		// curAnim.Pause();
		curAnim.Play("walkingRight");
	}
}
