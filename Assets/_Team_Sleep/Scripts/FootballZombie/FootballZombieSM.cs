using UnityEngine;
using System.Collections;

public class FootballZombieSM : MonoBehaviour {

	public enum BossStates {Chase, RaiseZombies, Stop};
	private BossStates bState = BossStates.Stop;

	// Boss Scripts
	private FootballZombieRaiseZombies RZ;
	private FootballZombieChase C;

	// Use this for initialization
	void Start () {
		RZ = GetComponent<FootballZombieRaiseZombies>();
		C = GetComponent<FootballZombieChase>();
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


}
