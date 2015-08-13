using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDamageable {

	public float speed = 5f;
	public float speedDuringAim = 2f;
	public Transform rightHandHandle;
	public float health = 300f;
	private bool isDead;

	private Animator anim;
	private Rigidbody body;

	private int attackLayerIndex;
	private bool isThrowing = false;
	private float healthLeft;

	public Weapon weapon;
	public FiringMechanism grenadeThrowMech;
	public GameObject dummyGrenade;
	private float grenadeTimer = 0f;
	private float aimTimer = 0f;
	private bool aiming = false;
	private CapsuleCollider capsuleCollider;

	void Start() {
		anim = GetComponent<Animator> ();
		body = GetComponent<Rigidbody> ();
		capsuleCollider = GetComponent<CapsuleCollider> ();
		attackLayerIndex = anim.GetLayerIndex ("Attack Layer");
		Reset ();
	}

	void FixedUpdate() {
		if (isDead)
			return;

		Vector3 moveStick = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		Vector3 aimStick = new Vector3 (Input.GetAxis ("RightH"), 0, Input.GetAxis ("RightV"));

		float speedMultiplier = speed;
		float aimWeight = 0f;
		aimTimer += Time.deltaTime;
		if (IsNonZero (aimStick)) {
			aiming = true;
			speedMultiplier = speedDuringAim;
			aimWeight = 1f;
			transform.LookAt (transform.position + aimStick);
			rightHandHandle.LookAt (rightHandHandle.position + transform.forward);
		} else {
			aiming = false;
			aimTimer = 0f;
		}
		anim.SetLayerWeight(attackLayerIndex, aimWeight);
		weapon.IsAiming(aiming);

		if (aiming && !isThrowing && aimTimer > 0.1f && Input.GetAxis("FireRT") > 0.2f) {
			weapon.Fire(weapon.muzzle.forward);
		}

		if (Input.GetAxis ("FireLT") > 0.2f) {

			if (aiming) {
				if (!isThrowing) {
					Debug.Log("throwthrow");
					anim.SetTrigger("throw");
					dummyGrenade.SetActive(true);
				}
			} else {
				grenadeThrowMech.Fire(Vector3.zero);	//Drop it
			}
			isThrowing = true;
		}

		if (isThrowing) {
			grenadeTimer += Time.deltaTime;
			anim.SetLayerWeight(attackLayerIndex, 1f);
			if (grenadeTimer > 0.385f) {
				dummyGrenade.SetActive(false);
				grenadeThrowMech.Fire(grenadeThrowMech.muzzle.forward + grenadeThrowMech.muzzle.up);
			} if (grenadeTimer > 0.9f) {
				isThrowing = false;
				grenadeTimer = 0f;
				anim.SetLayerWeight(attackLayerIndex, 0f);
			}
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

	#region IDamageable implementation

	public void DoDamage (float damage, Vector3 point, Vector3 normal, ProjectileType type)
	{
		if (!isDead) {
			healthLeft -= damage;
			if (healthLeft < 0) {
				anim.SetTrigger ("death");
				DisableBoxes ();
				isDead = true;
			}
		}
	}

	#endregion

	public void Reset() {
		healthLeft = health;
		dummyGrenade.SetActive (false);
		capsuleCollider.enabled = true;
		isDead = false;
	}

	void DisableBoxes() {
		capsuleCollider.enabled = false;
	}
}
