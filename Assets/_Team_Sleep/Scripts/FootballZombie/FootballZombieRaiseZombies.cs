using UnityEngine;
using System.Collections;

public class FootballZombieRaiseZombies : MonoBehaviour {

	public Transform AscensionZombie;

	private Transform RaiseZombieZone;
	private int ZoneMaxX;
	private int ZoneMinX;
	private int ZoneMaxY;
	private int ZoneMinY;

	private int ZombieCount = 0;
	
	private FootballZombieSM FZSM;

	void Start () {
		FZSM = GetComponent<FootballZombieSM>();

		RaiseZombieZone = GameObject.Find("RaiseZombieZone").transform;
		ZoneMaxX = (int)RaiseZombieZone.collider.bounds.max.x;
		ZoneMinX = (int)RaiseZombieZone.collider.bounds.min.x;
		ZoneMaxY = (int)RaiseZombieZone.collider.bounds.max.y;
		ZoneMinY = (int)RaiseZombieZone.collider.bounds.min.y;

		print ("MAX: " + RaiseZombieZone.collider.bounds.max);
		print ("MIN: " + RaiseZombieZone.collider.bounds.min);

		// StartCoroutine(RaiseZombie());
	}

	public void BeginRaisingZombies(){
		// play animation
		StartCoroutine(RaiseZombie());

	}

	IEnumerator RaiseZombie(){
		//RaiseZombieZone.collider.bounds.
		yield return new WaitForSeconds(0.6f);

		int xPos = Random.Range(ZoneMinX, ZoneMaxX);
		int yPos = Random.Range(ZoneMinY, ZoneMaxY);
		Vector3 pos = new Vector3(xPos, yPos, 0);
		Instantiate(AscensionZombie, pos, Quaternion.identity);

		ZombieCount++;
		if(ZombieCount <= 8){
			StartCoroutine(RaiseZombie());
		}
		else{
			ZombieCount = 0;
			StartCoroutine(WaitBeforeSwitchingToChase());
		}
	}

	IEnumerator WaitBeforeSwitchingToChase(){
		yield return new WaitForSeconds(5);
		FZSM.SetStateToChase();
	}


}
