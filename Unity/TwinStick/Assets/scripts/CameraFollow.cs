using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	GameObject player;
	Vector3 prevPos;

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	// Use this for initialization
	void Start () {
		prevPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
	}

	void FixedUpdate() {
		Vector3 currentPos = player.transform.position;
		Vector3 diff = currentPos - prevPos;
		prevPos = new Vector3 (currentPos.x, currentPos.y, currentPos.z);
		transform.position += diff;
	}
}
