using UnityEngine;
using System.Collections;

public class GroundedScript : MonoBehaviour {

	bool grounded = false;
	GameObject ground;

	void Awake() {
		ground = GameObject.FindGameObjectWithTag ("Ground");
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject == ground) {
			grounded = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject == ground) {
			grounded = false;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject == ground) {
			grounded = true;
		}
	}

	public bool isGrounded() {
		return grounded;
	}
}
