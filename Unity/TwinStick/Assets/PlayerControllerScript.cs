using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {

	Animator anim;
	Rigidbody body;
	public float speed = 5;
	bool isSpaceDown = false;

	void Awake() {
		anim = GetComponent<Animator> ();
		body = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		if (Input.GetAxis ("Vertical") > 0.1 && Input.GetKey(KeyCode.Space) ){
			body.velocity = Vector3.forward * speed * 2;
			anim.SetFloat ("speed", 6.2f);
		} else if (Input.GetAxis ("Vertical") > 0.1) {
			body.velocity = Vector3.forward * speed;
			anim.SetFloat ("speed", 0.21f);
		} else if (Input.GetAxis ("Vertical") < -0.1) {
			body.velocity = Vector3.back * speed * 0.8f;
			anim.SetFloat("speed", 0.2f);
		} else {
			body.velocity = Vector3.zero;
			anim.SetFloat ("speed", 0.0f);
		}

		if (Input.GetKey (KeyCode.Space)) {
			Debug.Log ("Space key is down");
		} else {
			Debug.Log ("Space key is not down...");
		}
	}

}
