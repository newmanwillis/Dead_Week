using UnityEngine;
using System.Collections;

public class RandomizeColor : MonoBehaviour {
	
	public float time = 5f;
	
	private float trueTime;
	private float timeMod;
	private tk2dSprite Sprite;
	private Color[] Colors;

	private tk2dSpriteAnimator curAnim;
	//private tk2dSprite sprite;
	private ZombieSM _state;
	private CharacterController CC;

	private enum direction {up, down, left, right};



	void Start () {

		curAnim = GetComponent<tk2dSpriteAnimator>();
		_state = GetComponent<ZombieSM>();
		CC = GetComponent<CharacterController>();

		Sprite = GetComponent<tk2dSprite>();
		
		timeMod = time;
		trueTime = 1f/timeMod;
		//Color orange = new Color(255, 80, 0, 255);
		Colors = new Color[] {Color.cyan, Color.green, Color.magenta, Color.yellow, Color.blue, Color.white, Color.grey, Color.red, Color.black};
		StartCoroutine(ChangeColor(trueTime));
	}
	
	void FixedUpdate () {
		if(_state.curState == ZombieSM.ZombieState.TakingDamage){
			direction dir = FindDirection();
			switch(dir){
				case direction.up:
					float rand = Random.value;
					if(rand <= 0.3)
						CC.Move(Vector3.down);
					else if(rand > 0.3 && rand < 0.7)
						CC.Move(Vector3.down + Vector3.right);
					else
						CC.Move(Vector3.down + Vector3.left);
					break;
				case direction.down:
					//CC.Move(Vector3.up);
					float rand2 = Random.value;
					if(rand2 <= 0.3)
						CC.Move(Vector3.up);
					else if(rand2 > 0.3 && rand2 < 0.7)
						CC.Move(Vector3.up + Vector3.right);
					else
						CC.Move(Vector3.up + Vector3.left);
					break;
				case direction.left:
					CC.Move(Vector3.right);
					break;
				case direction.right:
					//transform.position = transform.position - Vector3.right;
				print ("in here");
					CC.Move(Vector3.left);
					break;
			}
		}
	}
	
	IEnumerator ChangeColor(float colorTime){
		Color startColor = Sprite.color;
		Color endColor = ChooseColor();
		float fullTime = Time.time + colorTime;
		while(Time.time < fullTime){
			float timeLeft = (colorTime - (fullTime - Time.time)) * timeMod;
			Sprite.color = Color.Lerp(startColor, endColor, timeLeft);
			yield return null;
		}
		StartCoroutine(ChangeColor(trueTime));
	}
	
	public Color ChooseColor(){
		Color randColor = Sprite.color;
		while(randColor.Equals(Sprite.color)){
			int rand = Random.Range(0, Colors.Length);
			randColor = Colors[rand];
		}
		return randColor;
	}

	private direction FindDirection(){
		//int facing;
		string clipName = curAnim.CurrentClip.name;
		if(clipName.Contains("Down") || clipName.Contains("Forward"))
			return direction.down;
		else if(clipName.Contains("Up") || clipName.Contains("Backward"))
			return direction.up;		
		else if(clipName.Contains("Left"))
			return direction.left;
		else
			return direction.right;			
	}

}