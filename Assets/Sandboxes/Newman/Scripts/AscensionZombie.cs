using UnityEngine;
using System.Collections;

public class AscensionZombie : MonoBehaviour {
	
	public Transform Zombie;
	public Transform Hole;
	
	private tk2dSpriteAnimator anim;
	
	void Start () {
		anim = GetComponent<tk2dSpriteAnimator>();
	}
	
	void Update () {
		if(!anim.Playing){
			print ("STOPPED");
			Transform zombie = ((Transform) Instantiate(Zombie, transform.position, Quaternion.identity) );
			// Make zombie auto chase
			Destroy(gameObject);
		}
	}
}
