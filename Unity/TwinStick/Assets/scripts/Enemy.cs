using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IDamageable, IColliderListener {

	public SphereCollider earShotCollider;
	public float timeToRemoveOnDeath = 3f;
	float elapsedTimeToRemoveOnDeath;
	public ParticleSystem particleSystemPrefab;
	public float health = 100f;
	float healthLeft;

	//Club swing variables
	float minTimeBetweenSwings = 0.667f;
	float timeSinceLastSwing;
	public float swingRange = 1f;
	public HandWeapon handWeaponScript;

	public Turn turn;
	public GameObject lineOfSight;
	public Transform[] navPoints;
	int posCurrentNavPoint;
	Transform navTarget;

	public float waitTime = 2f;
	float elapsedWaitTime = 0f;
	bool isWaiting = false;

	GameObject target;
	bool targetWithinLineOfSight;
	NavMeshAgent agent;
	Animator anim;

	ParticleSystem particles;
	CapsuleCollider cCollider;

	ScoreManager scoreManager;

	void Awake() {

		target = GameObject.FindGameObjectWithTag ("Player");
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		particles = (ParticleSystem)Instantiate (particleSystemPrefab);
		cCollider = GetComponent<CapsuleCollider> ();
		scoreManager = GameObject.FindGameObjectWithTag ("ScoreManager").GetComponent<ScoreManager>();

		Reset ();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameObject.activeSelf)
			return;

		if (healthLeft < 1) {
			if (elapsedTimeToRemoveOnDeath > timeToRemoveOnDeath) {
				Reset();
				gameObject.SetActive(false);
			} else {
				elapsedTimeToRemoveOnDeath += Time.deltaTime;
			}
			return;
		}

		if (isWaiting) {
			elapsedWaitTime += Time.deltaTime;
		}

		if (ShouldMoveToNextNavPoint ()) {
			if (IsFacingMoveDirection(new Vector3(navTarget.position.x - transform.position.x, 0, navTarget.position.z - transform.position.z).normalized)) {
				ResetWaiting(false);
				agent.SetDestination(navTarget.position);
			} else {
				if (turn.IsFinished)
					turn.TurnTowards(navTarget);
			}
		}

		if (hasReachedTarget () && !isWaiting) {
			ResetWaiting(true);
			navTarget = NextNavPoint();
		}

		if (agent.velocity.sqrMagnitude > 0.1f) {
			anim.SetBool ("isMoving", true);
			anim.SetFloat ("yDir", agent.velocity.magnitude / agent.speed);
		} else {
			anim.SetBool("isMoving", false);
		}

		timeSinceLastSwing += Time.deltaTime;
		if (timeSinceLastSwing > minTimeBetweenSwings) {
			anim.SetLayerWeight (1, 0f);
			handWeaponScript.doDamageAllowed = false;
		}
		
	}

	Transform NextNavPoint() {
		posCurrentNavPoint = (posCurrentNavPoint + 1) % navPoints.Length;
		return navPoints [posCurrentNavPoint];
	}

	bool IsFacingMoveDirection(Vector3 moveDir) {
		if (transform.forward == moveDir)
			return true;
		return false;
	}

	bool ShouldMoveToNextNavPoint() {
		if (targetWithinLineOfSight)
			return false;
		if (elapsedWaitTime < waitTime)
			return false;
		if (agent.velocity.sqrMagnitude > 0.01f)
			return false;
		return true;
	}

	bool hasReachedTarget() {
	
		if (agent.pathPending)
			return false;

		if (agent.velocity.sqrMagnitude > 0.1f)
			return false;

		if (agent.hasPath)
			return false;
		
		return true;
	}

	#region Damageable implementation

	public void DoDamage (float damage, Vector3 point, Vector3 normal, ProjectileType type)
	{

		healthLeft -= damage;

		particles.transform.position = point;
		particles.transform.LookAt (point + normal);
		particles.Play ();

		if (healthLeft < 1) {
			anim.SetLayerWeight(1, 0f);
			anim.SetTrigger("death");
			agent.Stop();
			agent.ResetPath();
			scoreManager.RegisterKill(type);
			DisableBoxes();
		}

	}

	#endregion

	void DisableBoxes() {
		cCollider.enabled = false;
		agent.enabled = false;
		lineOfSight.SetActive (false);
		earShotCollider.enabled = false;

	}

	public void Reset() {
		targetWithinLineOfSight = false;
		agent.enabled = true;
		agent.Stop ();
		agent.ResetPath ();
		ResetWaiting (true);
		posCurrentNavPoint = 0;
		navTarget = navPoints [0];
		healthLeft = health;
		timeSinceLastSwing = minTimeBetweenSwings;
		anim.SetLayerWeight (1, 0f);
		elapsedTimeToRemoveOnDeath = 0f;
		cCollider.enabled = true;
		lineOfSight.SetActive (true);
		earShotCollider.enabled = true;
	}

	void ResetWaiting(bool waiting) {
		isWaiting = waiting;
		elapsedWaitTime = 0f;
	}

	bool IsWithinSwingRange(Vector3 a, Vector3 b, float sqrDist) {
		return (a - b).sqrMagnitude < sqrDist;
	}

	#region IColliderListener implementation

	public void OnColliderEnter (Collider owner, Collider collider) {
		if (collider.gameObject == target) {
			ResetWaiting(false);
			if (owner.gameObject == lineOfSight) {
				targetWithinLineOfSight = true;
				turn.Stop();
				transform.LookAt(collider.gameObject.transform.position);
				agent.destination = collider.gameObject.transform.position;
			} else if (!targetWithinLineOfSight) {
				turn.TurnTowards(target.transform);
			}
		}
	}

	public void OnColliderStay (Collider owner, Collider collider) {
		if (collider.gameObject == target) {
			ResetWaiting(false);
			if (owner.gameObject == lineOfSight) {
				turn.Stop();
				transform.LookAt(collider.gameObject.transform.position);
				agent.destination = collider.gameObject.transform.position;
				if (IsWithinSwingRange(target.transform.position, transform.position, swingRange)) {
					anim.SetLayerWeight(1, 1f);
					anim.SetTrigger("hit");
					timeSinceLastSwing = 0f;
					handWeaponScript.doDamageAllowed = true;
				}
			} else if (!targetWithinLineOfSight) {
				turn.TurnTowards(target.transform);
			}
		}
	}

	public void OnColliderExit (Collider owner, Collider collider)
	{
		if (owner.gameObject == lineOfSight && collider.gameObject == target) {
			targetWithinLineOfSight = false;
		}
	}

	#endregion
}
