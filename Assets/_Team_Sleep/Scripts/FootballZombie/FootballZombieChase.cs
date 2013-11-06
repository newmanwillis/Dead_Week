using UnityEngine;
using System.Collections;

public class FootballZombieChase : MonoBehaviour {
	public float _speed = 50; //pixels per second
	public float _chargeSpeed = 150; //pixels per second
	public float _chargeCooldown = 6; //seconds
	public float _chargeLength = 1; //seconds
	public float _recoverTime = 2; //seconds
	
	private Player _player;
	private FootballZombieHealth _health;
	
	// _nextCharge gets updated at the END of a charge.
	private float _nextCharge = 0;
	private Vector3 _chargeDirection;
	private bool _currentlyCharging = false;
	private float _recoveredTime = 0;
	
	private CharacterController CC;
	private tk2dSpriteAnimator curAnim;
	// Use this for initialization
	void Start () {
		_player = GameObject.Find("Player").GetComponent<Player>();
		_health = GetComponent<FootballZombieHealth>();
		CC = GetComponent<CharacterController>();
		curAnim = GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (_recoveredTime > Time.time) {
			return;
		}
		if (!_health.isVulnerable) {
			Vector3 direction = _player.transform.position - transform.position;
			float distance = direction.magnitude;
			direction.Normalize();
			
			Vector3 cardinal = analogToCardinal(direction);
			//Debug.Log ("cardinal: " + cardinal);
			//Debug.Log ("dot: " + Vector3.Dot(direction, cardinal));
			if ((!_currentlyCharging) && Vector3.Dot(direction, cardinal) > .99) {
				//Debug.Log("starting charge");
				_chargeDirection = cardinal;
				_currentlyCharging = true;
			}
			
			if (_currentlyCharging) {
				//Debug.Log("charging");
				CC.Move(_chargeDirection * _chargeSpeed * Time.deltaTime);
			} else {
				//Debug.Log("moving");
				CC.Move(cardinalTowardsChargeSpot() * _speed * Time.deltaTime);
			}
			
			/*if (_nextCharge < Time.time) {
				if (_nextCharge + _chargeLength < Time.time) {
					if (_chargeDirection == null) {
						_chargeDirection = direction;
					}
					CC.Move(_chargeDirection * _speed * 3 * Time.deltaTime);
				} else {
					// charge is over
					_nextCharge = Time.time + _chargeCooldown;
					_chargeDirection = null;
				}
			} else {
				CC.Move(direction * _speed * Time.deltaTime);
				curAnim.Play(walkingAnimationForDirection(direction));
			}*/
		}
	}
	
	Vector3 cardinalTowardsChargeSpot() {
		Vector3 direction = _player.transform.position - transform.position;
		if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x)) {
			if (direction.x > 0) {
				return new Vector3(1, 0, 0);
			} else {
				return new Vector3(-1, 0, 0);
			}
		} else {
			if (direction.y > 0) {
				return new Vector3(0, 1, 0);
			} else {
				return new Vector3(0, -1, 0);
			}
		}
	}
	
	Vector3 analogToCardinal(Vector3 analogDirection) {
		if (Mathf.Abs(analogDirection.x) > Mathf.Abs(analogDirection.y)) {
			if (analogDirection.x > 0) {
				return new Vector3(1, 0, 0);
			} else {
				return new Vector3(-1, 0, 0);
			}
		} else {
			if (analogDirection.y > 0) {
				return new Vector3(0, 1, 0);
			} else {
				return new Vector3(0, -1, 0);
			}
		}
	}
	
	string walkingAnimationForDirection(Vector3 direction) {
		//if (direction == Vector3(1, 0, 0)) {
		//	return "walkingRight";
		//}
		return "walkingRight";
		// walkingLeft, walkingBackward, walkingForwards
	}
	
	void OnTriggerEnter(Collider other) {
		if (_currentlyCharging) {
			//Debug.Log("hit " + other.tag + " name: " + other.name);
			if (other.tag == "Player" || other.tag == "Wall") {
				if (other.tag == "Player") {
					_player.GotHit(3);
					//Debug.Log("hit player");
				}
				_currentlyCharging = false;
				_recoveredTime = Time.time + _recoverTime;
			}
		}
	}
}
