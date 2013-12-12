using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public GUIStyle myStyle;
	GameObject playerObject;
	Player player;

	private class MegaBarBar {
		public Texture color;
		public Rect pos;
		public float maxLength;
		public float lastPercent;
	}

	//public Texture healthBar;
	//public Texture staminaBar;
	//public Texture energyBarLabel;
	//public Texture controlKeys;
	public Texture megaBar;
	private Rect megaBarPosition;
	public Texture megaBarRed;
	public Texture megaBarBlue;
	public Texture megaBarGreen;
	private MegaBarBar healthBar;
	private MegaBarBar energyBar;
	private MegaBarBar staminaBar;

	public Texture swordKey;
	private Rect swordKeyPosition;
	public Texture lazerKey;
	private Rect lazerKeyPosition;
	public Texture stunKey;
	private Rect stunKeyPosition;
	public Texture flashlightKey;
	private Rect flashlightKeyPosition;
	//private Rect controlKeysPosition;
	
	public Texture winMessage;
	public Texture loseMessage;
	public Texture blackBox;
	public Texture whiteBox;
	public Texture blueBox;
	public Texture redBox;
	public Texture greenBox;
	public Texture yellowBox;
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
	
	private GameObject boss;

	private float lastHealthPercent = 0;
	private float lastEnergyPercent = 0;

	private Rect rectForSprite(float topLeftX, float topLeftY, Texture sprite, bool yIsFromBottom = false) {
		if (yIsFromBottom) {
			return new Rect(topLeftX, Screen.height - topLeftY - sprite.height, sprite.width, sprite.height);
		} else {
			return new Rect(topLeftX, topLeftY, sprite.width, sprite.height);
		}
	}

	// Use this for initialization
	void Start () {
		//camera.orthographic = true;
		//camera.orthographicSize = Screen.height/4;
		if (!isCutscene) {
			playerObject = GameObject.Find("Player");
			player = playerObject.GetComponent<Player>();
			//controlKeysPosition = new Rect(50, Screen.height - 15 - controlKeys.height, controlKeys.width, controlKeys.height);
			flashlightKeyPosition = rectForSprite(30, 15, flashlightKey, true);
			swordKeyPosition = rectForSprite(flashlightKeyPosition.xMax - 10, 15, swordKey, true);
			lazerKeyPosition = rectForSprite(swordKeyPosition.xMax + 5, 15, lazerKey, true);
			stunKeyPosition = rectForSprite(lazerKeyPosition.xMax + 5, 15, stunKey, true);

			megaBarPosition = new Rect(10, 10, megaBar.width, megaBar.height);
			//Debug.Log ("megabar width: " + megaBar.width + " height: " + megaBar.height);
			healthBar = new MegaBarBar();
			healthBar.color = megaBarRed;
			healthBar.pos = new Rect(megaBarPosition.xMin + 108, megaBarPosition.yMin + 24, 110, 36);
			healthBar.maxLength = healthBar.pos.width;
			energyBar = new MegaBarBar();
			energyBar.color = megaBarBlue;
			energyBar.pos = new Rect(healthBar.pos.xMin, healthBar.pos.yMax-2, 110, 22);
			energyBar.maxLength = healthBar.pos.width;
			staminaBar = new MegaBarBar();
			staminaBar.color = megaBarGreen;
			staminaBar.pos = new Rect(healthBar.pos.xMin, energyBar.pos.yMax-2, 110, 15);
			staminaBar.maxLength = healthBar.pos.width;
		}
		
		isPaused = false;

		GameObject[] computers = GameObject.FindGameObjectsWithTag("ComputerTerminal");
		computerTerminals = new HackableComputer[computers.Length];
		for (int i = 0; i < computers.Length; i++) {
			computerTerminals[i] = computers[i].GetComponent<HackableComputer>();
		}
		
		currentlyRedIfLow = false;
		redWhiteSwitchTime = Time.time + 1;
		
		boss = GameObject.Find("FootballZombie");
}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > redWhiteSwitchTime) {
			redWhiteSwitchTime = Time.time + 1;
			currentlyRedIfLow = !currentlyRedIfLow;
		}
	}

	void drawMegaBarBar(MegaBarBar bar, float percent) {
		drawPercentBar((int)bar.pos.xMin, (int)bar.pos.yMin, (int)bar.maxLength, (int)bar.pos.height, percent, bar.color, null, bar.lastPercent, true, 2);
		bar.lastPercent = Mathf.Max(percent, bar.lastPercent - 0.001F);
	}
	
	// called for GRAPHICS frames and not physics timesteps. GUI methods are only allowed in here.
	void OnGUI() {
		if (Event.current.isKey && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return) {
			if (isPaused) {
				isPaused = false;
				Time.timeScale = 1;
			}
		}	
		if (isPaused) {
			if (currentMessage != null) {
				drawTextMessage(currentMessage);
			} else if (currentInfoCard != null) {
				drawInfoCard(currentInfoCard);
			}
		} else {
			/* 
			 * Some unknown force is setting this to zero if a scene starts without an
			 * initial info card. 
			 * I don't know what it is, but this fixes the symptoms.
			 */
			Time.timeScale = 1;
		}
		
		if (!isCutscene) {
			GUI.DrawTexture(megaBarPosition, megaBar);

			GUI.depth = 0;
			float health = playerHealth();
			float healthPercent = health / player.maxHealth;
			drawMegaBarBar(healthBar, healthPercent);
			//GUI.DrawTexture(new Rect(0, 0, healthBar.width, healthBar.height), healthBar);
			//drawPercentBar(5, 40, 234, 27, healthPercent, redBox, null, lastHealthPercent);
			//lastHealthPercent = Mathf.Max(healthPercent, lastHealthPercent - 0.001F);

			float stamPercent = player.curStamina / (float) player.maxStamina;
			drawMegaBarBar(staminaBar, stamPercent);
			//GUI.DrawTexture(new Rect(0, 66, staminaBar.width, staminaBar.height), staminaBar);
			//drawPercentBar(7, 2*33 + 26, 232, 27, stamPercent, greenBox, null, 0, false);
			
			drawEnergyBar();
			
			drawBossHealth();
		
			// draw hack bar:
			HackableComputer hacking = findCurrentHack();
			if (hacking != null) {
				float percent = hacking.hackSoFar / hacking.timeToHackSeconds;
				// 200 wide, centered
				// 40 high, starting 3/4 down the screen
				string msg = hacking.isGenerator ? "Restarting..." : "Hacking...";
				drawPercentBar(Screen.width/2 - 100, (int)(Screen.height * (3.0/4.0)), 200, 40, percent, blueBox, msg, 0, false);
			}
			
			//GUI.DrawTexture(controlKeysPosition, blackBox);
			//GUI.DrawTexture(controlKeysPosition, controlKeys);
			drawControlKeys();
		
			if (GameObject.FindGameObjectsWithTag("LevelExit").Length == 0 && (!boss)) {
				Rect position = new Rect(Screen.width/2-winMessage.width/2, Screen.height/2-winMessage.height/2, winMessage.width, winMessage.height);
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
				GUI.DrawTexture(position, winMessage);
			} else if (playerHealth() <= 0) {
				//Rect position = new Rect(Screen.width/2-loseMessage.width/2, Screen.height/2-loseMessage.height/2, loseMessage.width, loseMessage.height);
				//GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
				//GUI.DrawTexture(position, loseMessage);
			}
		}
	}

	void drawControlKeys() {
		if (player.hasSword) {
			GUI.DrawTexture(swordKeyPosition, swordKey, ScaleMode.ScaleAndCrop);
		}
		if (player.hasPhoneBullet) {
			GUI.DrawTexture(lazerKeyPosition, lazerKey, ScaleMode.ScaleAndCrop);
		}
		if (player.hasPhoneStun) {
			GUI.DrawTexture(stunKeyPosition, stunKey, ScaleMode.ScaleAndCrop);
		}
		GUI.DrawTexture(flashlightKeyPosition, flashlightKey, ScaleMode.ScaleAndCrop);
	}
	
	void drawEnergyBar() {
		float energy = player.curPhoneCharge;
		float energyMax = player.maxPhoneCharge;
		float percent = energy / energyMax;
		drawMegaBarBar(energyBar, percent);
		//GUI.DrawTexture(new Rect(Screen.width - 289, 10, energyBarLabel.width, energyBarLabel.height), energyBarLabel);
		//drawPercentBar(Screen.width - 284, 10+energyBarLabel.height, 234, 25, percent, null, null, lastEnergyPercent);
		//lastEnergyPercent = Mathf.Max(percent, lastEnergyPercent - 0.001F);
	}
	
	void drawBossHealth() {
		if (boss) {
			FootballZombieHealth bossHealth = boss.GetComponent<FootballZombieHealth>();
			float percent = bossHealth.health / (float)bossHealth.maxHealth;
			int ctrlKeysOffset = (int)stunKeyPosition.xMax;
			drawPercentBar(100 + ctrlKeysOffset, Screen.height - 75, Screen.width - 200 - ctrlKeysOffset, 50, percent, redBox, "Boss Health", 0, false);
		}
	}
	
	void drawPercentBar(int topLeftX, int topLeftY, int length, int height, float percent, Texture color = null, string text = null, float oldPercent = 0f, bool flashIfLow = true, int margin = 5) {
		color = color == null ? blueBox : color;
		if (percent < .25 && currentlyRedIfLow && flashIfLow) {
			GUI.DrawTexture(new Rect(topLeftX, topLeftY, length, height), redBox);
		} else {
			GUI.DrawTexture(new Rect(topLeftX, topLeftY, length, height), whiteBox);
		}
		float oldPercentWidth = Mathf.Max((length - (margin*2))*oldPercent, 0);
		float newPercentWidth = Mathf.Max((length - (margin*2))*percent, 0);
		GUI.DrawTexture(new Rect(topLeftX + margin, topLeftY + margin, oldPercentWidth, height - margin*2), yellowBox);
		GUI.DrawTexture(new Rect(topLeftX + margin, topLeftY + margin, newPercentWidth, height - margin*2), color);
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
	
	//public void gotCheckpoint() {
	//	pauseAndDrawTextMessage("Checkpoint!!");
	//}
	
	public void pauseAndDrawTextMessage(string message) {
		Debug.Log ("TM Called with: " + message);
		currentMessage = message;
		isPaused = true;
		Time.timeScale = 0;
		currentSpeakerFace = null;
	}
	
	public void pauseAndDrawInfoCard(Texture infoCard) {
		Debug.Log ("IF Called with: " + infoCard);
		currentMessage = null;
		isPaused = true;
		Time.timeScale = 0;
		currentInfoCard = infoCard;
	}
	
	public void pauseAndDrawTextMessage(string message, Texture face, bool faceToTheLeft) {
		Debug.Log ("TM Called with: " + message + ", " + face + ", " + faceToTheLeft);
		pauseAndDrawTextMessage(message);
		currentSpeakerFace = face;
		faceIsToTheLeft = faceToTheLeft;
	}
	
	void drawInfoCard(Texture infoCard) {
		//Debug.Log("width: " + infoCard.width + " height: " + infoCard.height);
		Rect pos = new Rect((Screen.width - infoCard.width)/2, (Screen.height - infoCard.height)/2, infoCard.width, infoCard.height);
		//Rect pos = new Rect((Screen.width - 630)/2, (Screen.height - 380)/2, 630, 380);
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
