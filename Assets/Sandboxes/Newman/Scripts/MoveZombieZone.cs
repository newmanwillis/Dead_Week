using UnityEngine;
using System.Collections;

public class MoveZombieZone : MonoBehaviour {
	
	public float Direction = -1;
	public float Speed = 60;
	
	private Vector3 Move;
	
	// Use this for initialization
	void Start () {
		Move = new Vector3(0, Speed * Direction, 0);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	
	
	
	void OnTriggerStay(Collider other){
		
		if(other.tag == "Zombie"){
			ZombieSM zsm = other.GetComponent<ZombieSM>();
			if(zsm.curState == ZombieSM.ZombieState.Wander || zsm.curState == ZombieSM.ZombieState.Stop){
				zsm.curState = ZombieSM.ZombieState.Stop;	
				//print ("zone force mvoe");
				if(Direction < 0){
					ZombieInfo.Animate.WalkDown(other.GetComponent<tk2dSpriteAnimator>());
				}
				else{
					
					ZombieInfo.Animate.WalkUp(other.GetComponent<tk2dSpriteAnimator>());
				}		
				
				other.GetComponent<CharacterController>().Move(Move * Time.deltaTime);
			}
			//other.GetComponent<ZombieSM>().curState == ZombieSM.ZombieState.Wander
			//check 
			//move cc
			
		}
		
	}
}