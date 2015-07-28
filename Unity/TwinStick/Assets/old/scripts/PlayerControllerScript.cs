using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {
	
	public float speed = 15;
	public float jumpForce = 5000;
	public GroundedScript groundedScript;
	public ShootingScript shootingScript;

	Animator anim;
	Rigidbody body;

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
				body.velocity = new Vector3(0, body.velocity.y, 0);
				anim.SetBool ("move", false);
				anim.SetFloat ("speed", 0);
			} else {
				Vector3 direction = new Vector3 (moveH, body.velocity.y, moveV);

				body.velocity = yLockedMultiplication(direction, speed);
				float magnitude = direction.magnitude;
				transform.LookAt (transform.position + new Vector3(moveH, 0, moveV));
				anim.SetBool ("move", true);
				anim.SetFloat ("speed", magnitude);
			}
		} else if (groundedScript.isGrounded() && aimH != 0.0f || aimV != 0.0f) {	//aiming
			Vector3 moveDirection = new Vector3 (moveH, body.velocity.y, moveV);
			body.velocity = yLockedMultiplication(moveDirection, speed * 0.25f);

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
			if (moveDirection.x != 0.0f || moveDirection.y != 0.0f) {
				anim.SetBool("move", true);
			} else {
				anim.SetBool("move", false);
			}

			if (moveDirection.x == 0f && moveDirection.z == 0f) {
				anim.SetFloat("xDirection", 0f);
				anim.SetFloat("yDirection", 0f);
			} else {
				anim.SetFloat("xDirection", x);
				anim.SetFloat("yDirection", z);
			}

			shootingScript.Fire();

		}

		if (Input.GetButton("Jump") && groundedScript.isGrounded ()) {
			body.AddForce(new Vector3(0, jumpForce, 0));
			anim.SetTrigger("jump");
		}


		anim.SetBool ("inAir", !groundedScript.isGrounded ());
	}

	Vector3 yLockedMultiplication(Vector3 vec3, float multiplier) {
		return new Vector3(vec3.x * multiplier, vec3.y, vec3.z * multiplier);
	}

}

