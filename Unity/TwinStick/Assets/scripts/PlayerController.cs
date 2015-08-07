using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 5f;
	public float speedDuringAim = 2f;
	public Transform rightHandHandle;

	private Animator anim;
	private Rigidbody body;

	private int attackLayerIndex;

	public Weapon weapon;

	void Start() {
		anim = GetComponent<Animator> ();
		body = GetComponent<Rigidbody> ();
		attackLayerIndex = anim.GetLayerIndex ("Attack Layer");
	}

	void FixedUpdate() {
		Vector3 moveStick = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		Vector3 aimStick = new Vector3 (Input.GetAxis ("RightH"), 0, Input.GetAxis ("RightV"));

		float speedMultiplier = speed;
		bool aiming = false;
		float aimWeight = 0f;

		if (IsNonZero(aimStick)) {
			aiming = true;
			speedMultiplier = speedDuringAim;
			aimWeight = 1f;
			transform.LookAt(transform.position + aimStick);
			rightHandHandle.LookAt(rightHandHandle.position + transform.forward);
		}
		anim.SetLayerWeight(attackLayerIndex, aimWeight);
		weapon.IsAiming(aiming);

		if (aiming && Input.GetAxis("FireRT") > 0.2f) {
			weapon.Fire();
		}

		bool moving = false;
		float xDir = 0f;
		float yDir = 0f;
		Vector3 velocity = Vector3.zero;
		if (IsNonZero(moveStick)) {
			moving = true;
			if (aiming) {
				Vector3 relMovement = calcRelativeMoveDirection(moveStick, aimStick);
				xDir = relMovement.x;
				yDir = relMovement.z;
			} else {
				transform.LookAt(transform.position + moveStick);
				yDir = moveStick.sqrMagnitude;
			}
			velocity = new Vector3(moveStick.x * speedMultiplier, 0f, moveStick.z * speedMultiplier);
		} 
		anim.SetBool ("isMoving", moving);
		anim.SetFloat ("xDir", xDir);
		anim.SetFloat ("yDir", yDir);
		body.velocity = velocity;
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

	bool IsNonZero(Vector3 vec) {
		return isOutsideDeadZone (vec.x) || isOutsideDeadZone (vec.z);
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
