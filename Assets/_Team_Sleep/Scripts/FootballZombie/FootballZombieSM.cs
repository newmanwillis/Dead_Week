using UnityEngine;
using System.Collections;

public class FootballZombieSM : MonoBehaviour {

	public enum BossStates {Chase, RaiseZombies, Stop, Dead};
	private BossStates bState = BossStates.Stop;

	// Boss Scripts
	private FootballZombieRaiseZombies RZ;
	private FootballZombieChase C;

	// Use this for initialization
	void Start () {
		RZ = GetComponent<FootballZombieRaiseZombies>();
		C = GetComponent<FootballZombieChase>();
	}

	void FixedUpDate(){
		if(transform.position.z != -0.1f){
			Vector3 newPos = transform.position; //new Vector3(transform.position.x, transform.position.y, 0);
			newPos.z = -0.1f;
			transform.position = newPos;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	public BossStates State(){
		return bState;
	}
	
	public void SetStateToRaiseZombies(){
		bState = BossStates.RaiseZombies;
		RZ.BeginRaisingZombies();
	}

	public void SetStateToChase(){
		bState = BossStates.Chase;
	}

	public void SetStateToDead() {
		bState = BossStates.Dead;
	}
}
