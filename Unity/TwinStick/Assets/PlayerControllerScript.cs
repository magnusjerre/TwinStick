using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {

	Animator anim;
	Rigidbody body;
	public float speed = 10;

	void Awake() {
		anim = GetComponent<Animator> ();
		body = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		float moveH = Input.GetAxis ("Horizontal");
		float moveV = Input.GetAxis ("Vertical");

		float aimH = Input.GetAxis ("RightH");
		float aimV = Input.GetAxis ("RightV");

		if (aimH == 0.0f && aimV == 0.0f) {	//Not aiming
			anim.SetBool ("aim", false);
			if (moveH == 0.0f && moveV == 0.0f) {
				body.velocity = Vector3.zero;
				anim.SetBool ("move", false);
				anim.SetFloat ("speed", 0);
			} else {
				Vector3 direction = new Vector3 (moveH, 0, moveV);
				
				body.velocity = direction * speed;
				float magnitude = direction.magnitude;
				transform.LookAt (transform.position + direction);
				anim.SetBool ("move", true);
				anim.SetFloat ("speed", magnitude);
			}
		} else {	//aiming
			Vector3 moveDirection = new Vector3 (moveH, 0, moveV);
			body.velocity = moveDirection * speed * 0.25f;

			Vector3 lookDirection = new Vector3 (aimH, 0, aimV);
			transform.LookAt (transform.position + lookDirection);

			//Use righthand rule to calculate cross product, have the index finger point along the
			//first input and the middle finger along the second. The thumb will be the resultant normal vector
			Vector3 cross = Vector3.Cross(lookDirection, moveDirection);
			float angle = Vector3.Angle(lookDirection, moveDirection);
			float z = Mathf.Cos(Mathf.Deg2Rad * angle);
			float x = Mathf.Sin(Mathf.Deg2Rad * angle);
			if (cross.y > 0) {
				x = Mathf.Abs(x);
			} else {
				x = -Mathf.Abs(x);
			}

			anim.SetBool("aim", true);
			anim.SetBool("move", true);
			anim.SetFloat("xDirection", x);
			anim.SetFloat("yDirection", z);

		}

	}

}
