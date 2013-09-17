using UnityEngine;
using System.Collections;

public class GroundControl : MonoBehaviour {
	// 0 is wall, 1 is grass, 2 is grass2
	public Texture[] sprites;
	public bool[] pathable;
	public int[,] groundTypes = {
		{0, 0, 0, 0, 0},
		{0, 1, 2, 1, 0},
		{0, 2, 1, 2, 0},
		{0, 1, 2, 1, 0},
		{0, 2, 1, 2, 0},
		{0, 1, 2, 1, 0},
		{0, 0, 0, 0, 0}
	};
	
	
	int width;
	int height;
	int totalWidth;
	int totalHeight;
	int upperRightX;
	int upperRightY;
	
	CameraControl drawer;
	// Use this for initialization
	void Start () {
		drawer = Camera.main.GetComponent<CameraControl>();
		
		width = sprites[0].width;
		height = sprites[0].height;
		totalWidth = width * groundTypes.GetLength(0);
		totalHeight = height * groundTypes.GetLength(1);
		upperRightX = -totalWidth/2;
		upperRightY = totalHeight/2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.depth = 10;
		for (int r = 0; r < groundTypes.GetLength(0); r++) {
			for (int c = 0; c < groundTypes.GetLength(1); c++) {
				Vector3 worldCenter = worldCenterOf(r, c);
				drawer.drawSpriteInWorld(sprites[groundTypes[r,c]], worldCenter.x, worldCenter.y);
			}
		}
	}
	
	Vector3 worldCenterOf(int row, int col) {
		return new Vector3(upperRightX+(width*col), upperRightY+(-height*row), 0);
	}
	
	Rect worldRectOf(int row, int col) {
		Vector3 center = worldCenterOf(row, col);
		return new Rect(center.x-width/2, center.y-height/2, width, height);
	}
	
	public bool pointPathable(Vector3 pos) {
		for (int r = 0; r < groundTypes.GetLength(0); r++) {
			for (int c = 0; c < groundTypes.GetLength(1); c++) {
				if (!pathable[groundTypes[r,c]]) {
					if (worldRectOf(r, c).Contains(pos)) {
						return false;
					}
				}
			}
		}
		return true;
	}
}
