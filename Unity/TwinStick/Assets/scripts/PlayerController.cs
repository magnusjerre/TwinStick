using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 5f;
	public float speedDuringAim = 2f;

	private Animator anim;
	private Rigidbody body;

	private int attackLayerIndex;

	void Awake() {
		anim = GetComponent<Animator> ();
		body = GetComponent<Rigidbody> ();

		attackLayerIndex = anim.GetLayerIndex ("Attack Layer");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		Vector3 move = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		Vector3 aim = new Vector3 (Input.GetAxis ("RightH"), 0, Input.GetAxis ("RightV"));

		float speedMultiplier = speed;
		bool isAiming = false;
		//Aim handling
		if (isOutsideDeadZone (aim.x) || isOutsideDeadZone (aim.z)) {
			//Aiming something somewhere
			isAiming = true;
			speedMultiplier = speedDuringAim;
			anim.SetLayerWeight(attackLayerIndex, 1f);
			transform.LookAt(transform.position + aim);
		} else {
			//Not aiming anywhere...
			isAiming = false;
			anim.SetLayerWeight(attackLayerIndex, 0f);
		}
		//The following should be moved inside the above else-statement
		//Movement handling
		if (isOutsideDeadZone(move.x) || isOutsideDeadZone(move.z)) {
			anim.SetBool("isMoving", true);
			Vector3 relMovement = calcRelativeMoveDirection(move, aim);
			anim.SetFloat("xDir", relMovement.x);
			anim.SetFloat("yDir", relMovement.z);
			body.velocity = new Vector3(move.x * speedMultiplier, 0f, move.z * speedMultiplier);

			if (!isAiming) {
				transform.LookAt(transform.position + move);
			}
		} else {
			anim.SetBool("isMoving", false);
			anim.SetFloat("xDir", 0f);
			anim.SetFloat("yDir", 0f);
			body.velocity = Vector3.zero;
		}
	}

	Vector3 calcRelativeMoveDirection(Vector3 absMoveDir, Vector3 aimDir) {
		if (isInsideDeadZone (aimDir.x) && isInsideDeadZone (aimDir.z)) {
			return new Vector3(0, 0, Mathf.Abs(absMoveDir.z));
		}

		if (isInsideDeadZone (absMoveDir.x) && isInsideDeadZone (absMoveDir.z)) {
			return Vector3.zero;
		}

		Vector3 cross = Vector3.Cross (aimDir, absMoveDir);
		float angleBetween = Vector3.Angle (aimDir, absMoveDir);
		float forward = Mathf.Cos (Mathf.Deg2Rad * angleBetween);
		float sideways = Mathf.Sin (Mathf.Deg2Rad * angleBetween);
		if (cross.y > 0) {
			sideways = Mathf.Abs (sideways);
		} else {
			sideways = -Mathf.Abs (sideways);
		}

		return new Vector3 (sideways, 0, forward);
	}

	bool isOutsideDeadZone(float value) {
		return !isBetween (-0.1f, 0.1f, value);
	}

	bool isInsideDeadZone(float value) {
		return isBetween (-0.1f, 0.1f, value);
	}
	bool isBetween(float min, float max, float value) {
		return min <= value && value <= max;	
	}
}
