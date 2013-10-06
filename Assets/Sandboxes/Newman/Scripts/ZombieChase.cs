// This script requires objects to be labeled with correct tags and layers
// Tags: Player, Wall
// Layers: Default, Wall, Player

// problems with constant raycasting if ontriggerstay & problem with not finding player sometimes with ontriggerenter

using UnityEngine;
using System.Collections;

public class ZombieChase : MonoBehaviour {
	
	float _lookForPlayerTimer = 0.5f;
	float _chasePlayerTimer;
	
	Transform Player;
	Transform Zombie;
	private bool _foundPlayer = false;

	
	// Use this for initialization
	void Start () {
		Zombie = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other){
		
		if(other.tag == "Player" && !_foundPlayer && _lookForPlayerTimer < Time.time){		// Keeps checking for player every 0.5 seconds if in detection range
																							// but behind a wall. This is so it doesn't raycast every update.
			int layerMask = ~(1 << 0);
			//layerMask = ~layerMask;
			
			RaycastHit hit;
			if(Physics.Raycast(Zombie.position, other.transform.position - Zombie.position, out hit, 40, layerMask)){
				
				if(hit.transform.tag == "Wall"){
					_lookForPlayerTimer = Time.time + 0.5f;
					print("hit wall");					
				}
				else if(hit.transform.tag == "Player"){
					_foundPlayer = true;
					
					print("hit player");				
				}
				
				
				/*
				if(hit.transform.gameObject.layer == LayerMask.NameToLayer( "Wall" )){
					print("hit wall layer");
				}
				if(hit.transform.gameObject.layer == LayerMask.NameToLayer( "Player" )){
					print("hit player layer");
				}	*/		
			}
		}
		
	}
	
	void OnTriggerExit(Collider other){
		// Stop following player after some point
		
	}
}
