using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public GUIStyle myStyle;
	GameObject playerObject;
	Player player;
	
	public Texture healthBar;
	public Texture staminaBar;
	public Texture energyBarLabel;
	
	public Texture winMessage;
	public Texture loseMessage;
	public Texture blackBox;
	public Texture whiteBox;
	public Texture blueBox;
	public Texture redBox;
	public Texture textMessageBox;
	
	private bool isPaused;
	private string currentMessage;
	private Texture currentSpeakerFace;
	private Texture currentInfoCard;
	private bool faceIsToTheLeft;
	
	private HackableComputer[] computerTerminals;
	
	private bool currentlyRedIfLow;
	private float redWhiteSwitchTime;
	
	public bool isCutscene;
	
	// Use this for initialization
	void Start () {
		//camera.orthographic = true;
		//camera.orthographicSize = Screen.height/4;
		if (!isCutscene) {
			playerObject = GameObject.Find("Player");
			player = playerObject.GetComponent<Player>();
		}
		
		isPaused = false;
		
		GameObject[] computers = GameObject.FindGameObjectsWithTag("ComputerTerminal");
		computerTerminals = new HackableComputer[computers.Length];
		for (int i = 0; i < computers.Length; i++) {
			computerTerminals[i] = computers[i].GetComponent<HackableComputer>();
		}
		
		currentlyRedIfLow = false;
		redWhiteSwitchTime = Time.time + 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > redWhiteSwitchTime) {
			redWhiteSwitchTime = Time.time + 1;
			currentlyRedIfLow = !currentlyRedIfLow;
		}
	}
	
	// called for GRAPHICS frames and not physics timesteps. GUI methods are only allowed in here.
	void OnGUI() {
		if (Event.current.isKey && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return) {
			if (isPaused) {
				isPaused = false;
				Time.timeScale = 1;
			}// else {
			//	isPaused = true;
			//	currentMessage = "You paused the game!";
			//	Time.timeScale = 0;
			//}
		}
		
		if (isPaused) {
			if (currentMessage != null) {
				drawTextMessage(currentMessage);
			} else if (currentInfoCard != null) {
				drawInfoCard(currentInfoCard);
			}
		}
		
		if (!isCutscene) {
			GUI.depth = 0;
			float health = playerHealth();
			float healthPercent = health / player.maxHealth;
			GUI.DrawTexture(new Rect(0, 0, healthBar.width, healthBar.height), healthBar);
			drawPercentBar(5, 33, 234, 25, healthPercent, redBox);
		
			drawEnergyBar();
		
			// draw hack bar:
			HackableComputer hacking = findCurrentHack();
			if (hacking != null) {
				float percent = hacking.hackSoFar / hacking.timeToHackSeconds;
				// 200 wide, centered
				// 40 high, starting 3/4 down the screen
				drawPercentBar(Screen.width/2 - 100, (int)(Screen.height * (3.0/4.0)), 200, 40, percent, blueBox, "Hacking...");
			}
		
			if (GameObject.FindGameObjectsWithTag("LevelExit").Length == 0) {
				Rect position = new Rect(Screen.width/2-winMessage.width/2, Screen.height/2-winMessage.height/2, winMessage.width, winMessage.height);
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
				GUI.DrawTexture(position, winMessage);
			} else if (playerHealth() <= 0) {
				Rect position = new Rect(Screen.width/2-loseMessage.width/2, Screen.height/2-loseMessage.height/2, loseMessage.width, loseMessage.height);
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
				GUI.DrawTexture(position, loseMessage);
			}
		}
	}
	
	void drawEnergyBar() {
		float energy = player.curPhoneCharge;
		float energyMax = player.maxPhoneCharge;
		float percent = energy / energyMax;
		GUI.DrawTexture(new Rect(Screen.width - 289, 10, energyBarLabel.width, energyBarLabel.height), energyBarLabel);
		drawPercentBar(Screen.width - 284, 10+energyBarLabel.height, 234, 25, percent);
	}
	
	void drawPercentBar(int topLeftX, int topLeftY, int length, int height, float percent, Texture color = null, string text = null, int margin = 5) {
		color = color == null ? blueBox : color;
		if (percent < .25 && currentlyRedIfLow) {
			GUI.DrawTexture(new Rect(topLeftX, topLeftY, length, height), redBox);
		} else {
			GUI.DrawTexture(new Rect(topLeftX, topLeftY, length, height), whiteBox);
		}
		GUI.DrawTexture(new Rect(topLeftX + margin, topLeftY + margin, (length - (margin*2))*percent, height - margin*2), color);
		if (text != null) {
			myStyle.alignment = TextAnchor.MiddleCenter;
			GUI.Label(new Rect(topLeftX + margin, topLeftY + margin, length - margin*2, height - margin*2), text, myStyle);
			myStyle.alignment = TextAnchor.UpperLeft;
		}
	}
	
	HackableComputer findCurrentHack() {
		foreach (HackableComputer comp in computerTerminals) {
			if (comp.beingHacked) {
				return comp;
			}
		}
		return null;
	}
	
	public void drawSpriteInWorld(Texture sprite, float x, float y) {
		Vector2 pixels = ConvertWorldToPixels(new Vector2(x, y));
		drawSpriteAt((int)(pixels.x - sprite.width/2), (int)(pixels.y - sprite.height/2), sprite);
	}
	
	Vector2 ConvertWorldToPixels(Vector2 world) {
		float x = world.x;
		float y = world.y;
		x -= transform.position.x;
		y -= transform.position.y;
		y *= -1;
		x += Screen.width/2;
		y += Screen.height/2;
		return new Vector2(x, y);
	}
	
	public void pauseAndDrawTextMessage(string message) {
		currentMessage = message;
		isPaused = true;
		Time.timeScale = 0;
		currentSpeakerFace = null;
	}
	
	public void pauseAndDrawInfoCard(Texture infoCard) {
		currentMessage = null;
		isPaused = true;
		Time.timeScale = 0;
		currentInfoCard = infoCard;
	}
	
	public void pauseAndDrawTextMessage(string message, Texture face, bool faceToTheLeft) {
		pauseAndDrawTextMessage(message);
		currentSpeakerFace = face;
		faceIsToTheLeft = faceToTheLeft;
	}
	
	void drawInfoCard(Texture infoCard) {
		Debug.Log("width: " + infoCard.width + " height: " + infoCard.height);
		//Rect pos = new Rect((Screen.width - infoCard.width)/2, (Screen.height - infoCard.height)/2, infoCard.width, infoCard.height);
		Rect pos = new Rect((Screen.width - 630)/2, (Screen.height - 380)/2, 630, 380);
		GUI.DrawTexture(pos, infoCard);
	}
	
	void drawTextMessage(string message) {
		int upperLeftY = Screen.height - textMessageBox.height;
		int upperLeftX = (Screen.width - textMessageBox.width) / 2;
		GUI.DrawTexture(new Rect(upperLeftX, upperLeftY, textMessageBox.width, textMessageBox.height), textMessageBox);
		GUI.Label(new Rect(upperLeftX + 20, upperLeftY + 15, textMessageBox.width-25, (textMessageBox.height-15)), message, myStyle);
		
		if (currentSpeakerFace != null) {
			int faceUpperLeftX = upperLeftX+textMessageBox.width;
			int faceUpperLeftY = upperLeftY;
			Rect facePos = new Rect(faceUpperLeftX, faceUpperLeftY, currentSpeakerFace.width, currentSpeakerFace.height);
			GUI.DrawTexture(facePos, currentSpeakerFace);
		}
	}
	
	/*
	 * x and y are in pixels indexed from the top left of the screen. The top left
	 * corner of the sprite will be at (x,y).
	 * This is only usable from OnGUI.
	 */
	void drawSpriteAt(int x, int y, Texture sprite) {
		GUI.DrawTexture(new Rect(x, y, sprite.width, sprite.height), sprite);
	}
	
	float playerHealth() {
		if (playerObject == null) { // this is apparently overloaded to check whether it has been destroyed
			return -1;
		}
		return player.curHealth;
	}
}
