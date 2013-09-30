using UnityEngine;
using System.Collections;

public class ZombieMovement : MonoBehaviour {
	
	public float speed = 0.4f;
	public enum moveDirection {up, down, left, right};
	
	moveDirection curMoveDirection; 
	
	public bool isMoving = false;
	
	// Use this for initialization
	void Start () {
		curMoveDirection = moveDirection.down;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Move(){
		
		switch(curMoveDirection){
			// case 
		}
		
	}
}
