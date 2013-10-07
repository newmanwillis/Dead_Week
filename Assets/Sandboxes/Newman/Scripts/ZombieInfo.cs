using UnityEngine;
using System.Collections;

public class ZombieInfo : MonoBehaviour {
	
	//public static tk2dSpriteAnimator curAnim;	
	
	// Use this for initialization
	void Start () {
		//curAnim = GetComponent<tk2dSpriteAnimator>();
	}

	public static class Animate{
	
		public static void WalkUp(tk2dSpriteAnimator curAnim){
			curAnim.Play("walkingBackward");
		}
		public static void WalkDown(tk2dSpriteAnimator curAnim){
			curAnim.Play("walkingForward");
		}
		public static void WalkLeft(tk2dSpriteAnimator curAnim){
			curAnim.Play("walkingLeft");
		}
		public static void WalkRight(tk2dSpriteAnimator curAnim){
			curAnim.Play("walkingRight");
		}		
		
	}
	
}
