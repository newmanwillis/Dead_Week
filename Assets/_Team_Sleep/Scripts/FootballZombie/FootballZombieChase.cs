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

	private bool _gotHit = false;
	private int _numCharges = 0;

	// _nextCharge gets updated at the END of a charge.
	private float _nextCharge = 0;
	private Vector3 _chargeDirection;
	private bool _currentlyPreparingForCharge = false; // true only if _currentlyCharging is also true
	private bool _currentlyCharging = false;
	private bool _hitPlayerLastCharge = false;
	private float _recoveredTime = 0;
	
	private CharacterController CC;
	private tk2dSpriteAnimator curAnim;
	private FootballZombieSM FZSM;

	// Use this for initialization
	void Start () {
		_player = GameObject.Find("Player").GetComponent<Player>();
		_health = GetComponent<FootballZombieHealth>();
		CC = GetComponent<CharacterController>();
		curAnim = GetComponent<tk2dSpriteAnimator>();
		FZSM = GetComponent<FootballZombieSM>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(FZSM.State() == FootballZombieSM.BossStates.Chase){
			if (_recoveredTime > Time.time) {

				if (!_hitPlayerLastCharge) {
					CC.Move(-_chargeDirection * (_chargeSpeed / 10) * Time.deltaTime);
				}
			} else if (!_health.isVulnerable) {
				Vector3 direction = _player.transform.position - transform.position;
				float distance = direction.magnitude;
				direction.Normalize();
				
				Vector3 cardinal = analogToCardinal(direction);
				if ((!_currentlyCharging) && Vector3.Dot(direction, cardinal) > .99) {
					//*****
					if(_gotHit || _numCharges >= 2){
						_numCharges = 0;
						FZSM.SetStateToRaiseZombies();
						_gotHit = false;
					}
					else{
						_numCharges ++;
					//*****
						_chargeDirection = cardinal;
						_currentlyCharging = true;
						_currentlyPreparingForCharge = true;
						curAnim.Play(cardinalToStr(cardinal) + "DashCharge");
					}
				}
				
				if(FZSM.State() == FootballZombieSM.BossStates.Chase){

					if (_currentlyPreparingForCharge) {
						if (!curAnim.Playing) {
							_currentlyPreparingForCharge = false;
							curAnim.Play(cardinalToStr(_chargeDirection) + "RunLoop");
						}
					} else if (_currentlyCharging) {
						//Debug.Log("charging");
						CC.Move(_chargeDirection * _chargeSpeed * Time.deltaTime);
					} else {
						Vector3 moveDir = cardinalTowardsChargeSpot();
						string animStr = "walking_" + cardinalToStr(moveDir);
						if ((curAnim.CurrentClip == null) || (curAnim.CurrentClip.name != animStr)) {
							curAnim.Play(animStr);
						}
						CC.Move(moveDir * _speed * Time.deltaTime);
					}

				}

		} else {
			// he hit an electric wall, so he's done charging
			_currentlyCharging = false;
		}
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
	
	string cardinalToStr(Vector3 cardinalDirection) {
		if (cardinalDirection.x == 1) {
			return "right";
		} else if (cardinalDirection.x == -1) {
			return "left";
		} else if (cardinalDirection.y == 1) {
			return "up";
		} else if (cardinalDirection.y == -1) {
			return "down";
		} else {
			return null;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (_currentlyCharging && !_currentlyPreparingForCharge) {
			bool hitLaserWall = false;

			if (other.tag == "Button" || other.tag == "Wall") {
				Debug.Log("hit " + other.tag + " name: " + other.name);
			}
			if (other.tag == "LazerWall") {
				_gotHit = true;
				_numCharges = 0;
				_health.becomeVulnerable();
				// print("STRING: zap_" + cardinalToStr(_chargeDirection));
				curAnim.Play("zap_" + cardinalToStr(_chargeDirection));
				hitLaserWall = true;

			}
			if (other.tag == "Player" || other.tag == "Wall" || other.tag == "Button" || other.tag == "LazerWall") {
				if (other.tag == "Player") {
					_player.knockBack(_chargeDirection);
					_player.GotHit(3);
					_hitPlayerLastCharge = true;
					//Debug.Log("hit player");
				} else {
					_hitPlayerLastCharge = false;
				}
				_currentlyCharging = false;
				_recoveredTime = Time.time + _recoverTime;
				if(!hitLaserWall)
					curAnim.Play("walking_" + cardinalToStr(_chargeDirection));
			}
			else if(other.tag == "Zombie"){
				other.GetComponent<ZombieHealth>().TakeDamage(1007, ZombieHealth.HitTypes.sword, false);

			}
		}
	}

	//void ElectrocutedAnimation(){

	//}
}
