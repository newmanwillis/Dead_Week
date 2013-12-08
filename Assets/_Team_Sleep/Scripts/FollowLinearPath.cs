using UnityEngine;
using System.Collections;

public class FollowLinearPath : MonoBehaviour {
	private Vector3[] pathPoints;
	public int index = 0;
	private float delta = 0.01f;
	public float speed;
	public string defaultPathName;
	// Use this for initialization
	void Start () {
		if (defaultPathName != null && defaultPathName != "") {
			setPath(defaultPathName, false);
		}
	}

	public void setPath(string pathName, bool setPosAndIndex = true) {
		// Debug.Log("Setting path to : " + pathName);
		GameObject pathParentObj = GameObject.Find(pathName);
		int count = pathParentObj.transform.childCount;
		pathPoints = new Vector3[count];
		for (int i = 0; i < count; ++i) {
			pathPoints[i] = pathParentObj.transform.GetChild(i).position;
		}
		if (setPosAndIndex) {
			index = 0;
			transform.position = pathPoints[0];
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 between = pathPoints[index] - transform.position;
		if (between.magnitude < delta) {
			index++;
			if (index == pathPoints.Length) {
				Destroy(gameObject);
				return;
			}
		}

		Vector3 dir = between.normalized;
		Vector3 move = dir * speed * Time.deltaTime;
		if (move.magnitude > between.magnitude) {
			move = move.normalized * between.magnitude;
		}
		transform.position += move;
	}
}
