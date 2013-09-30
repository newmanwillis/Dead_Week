// problems with constant raycasting if ontriggerstay & problem with not finding player sometimes with ontriggerenter

using UnityEngine;
using System.Collections;

public class ZombieChase : MonoBehaviour {
	
	
	Transform Player;
	
	private bool _foundPlayer = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){/*
		print ("in ontriggeenter");	
		// dont see them if behind wall
		if(other.tag == "Player"){
			print ("found player");	
			if(!_foundPlayer){
				print ("foundplayer check");					
				RaycastHit hit;
				Vector3 playerPos =  other.transform.position; //  transform.TransformDirection(Vector3.forward);
        		if (Physics.Raycast(transform.position, playerPos, out hit, 100))	{
					print ("in raycast");		
					if(hit.collider.tag == "Wall"){
						print ("wall in the way");	
					}
					else if(hit.collider.tag == "Player"){
						print ("found Player");
						Player = other.transform;					
					}
				}
			}
		}*/
	}
	
	void OnTriggerExit(Collider other){
		// Stop following player after some point
		
	}
}
