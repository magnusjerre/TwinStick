using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IDamageable, IColliderListener {

	/*
	public Transform[] navPoints;
	public float waitTime = 2f;
	public float health = 100f;
	public float timeToRemoveOnDeath = 3f;
	public float damageDealt = 50f;
	public float minTimeBetweenDamage = 1f;*/

	public ParticleSystem particleSystemPrefab;
	public float health = 100f;
	float healthLeft;

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

	/*
	Vector3 posOfTarget;


	float healthLeft;
	float elapsedWaitTime = 0f;
	bool isWaiting = false;
	int currentNavTarget = 0;
	float deathTimer;
	float damageTimer;

	bool playerInSight = false;
	float minPlayerFollowDt = 0.2f;
	float playerFollowLeft = -1f;

	GameObject target;
	ScoreManager scoreManager;
	*/

	void Awake() {

		target = GameObject.FindGameObjectWithTag ("Player");
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		particles = (ParticleSystem)Instantiate (particleSystemPrefab);
		cCollider = GetComponent<CapsuleCollider> ();

		Reset ();
		/*
		transform.position = navPoints [0].position;
		target = GameObject.FindGameObjectWithTag ("Player");
		scoreManager = GameObject.FindGameObjectWithTag ("ScoreManager").GetComponent<ScoreManager>();
		//turn = new Turn ();
		Reset ();
		*/
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameObject.activeSelf)
			return;

		if (isWaiting) {
			Debug.Log("elapsedWaiitingTime: " + elapsedWaitTime);
			elapsedWaitTime += Time.deltaTime;
		}

		if (ShouldMoveToNextNavPoint ()) {
			if (IsFacingMoveDirection(new Vector3(navTarget.position.x - transform.position.x, 0, navTarget.position.z - transform.position.z).normalized)) {
				ResetWaiting(false);
				agent.SetDestination(navTarget.position);
				Debug.Log("Setting destination");
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

		/*
		if (!gameObject.activeSelf)
			return;

		turn.Update ();
		damageTimer -= Time.deltaTime;

		if (healthLeft < 1) {
			deathTimer -= Time.deltaTime;
			if (deathTimer < 0f) {
				Reset();
				gameObject.SetActive(false);
			}
			return;
		}

		if (playerInSight) {
			turn.Stop();
			Vector3 distance = target.transform.position - transform.position;
			if (distance.sqrMagnitude < 0.5f && damageTimer < 0) {
				target.GetComponent<IDamageable>().DoDamage(damageDealt, Vector3.zero, Vector3.zero, ProjectileType.BULLET);
				damageTimer = minTimeBetweenDamage;
			}

			playerFollowLeft -= Time.deltaTime;
			if (playerFollowLeft < 0) {
				playerFollowLeft = minPlayerFollowDt;
				//agent.destination = target.transform.position;
			}
		}
		if (!playerInSight && isWaiting) {

			if (targetWithinEarShot) {

				turn.NewState(transform, target.transform);
				//transform.LookAt(posOfTarget);

			} else {

				elapsedWaitTime += Time.deltaTime;
				anim.SetBool ("isMoving", false);
				anim.SetFloat("yDir", 0.75f);

				if (elapsedWaitTime > waitTime) {
					elapsedWaitTime = 0f;
					currentNavTarget = (currentNavTarget + 1) % navPoints.Length;
					//agent.destination = navPoints [currentNavTarget].position;
					isWaiting = false;
				}

			}

		} else {
			anim.SetBool ("isMoving", true);
			anim.SetFloat("yDir", 0.75f);
		}

		if (hasReachedTarget()) {
			isWaiting = true;
		}
		*/
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

		Debug.Log ("target actually reached");
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
			anim.SetTrigger("death");
			//scoreManager.RegisterKill(type);
			DisableBoxes();
			//targetWithinEarShot = false;
			//turn.Stop();
		}

	}

	#endregion

	void DisableBoxes() {
		cCollider.enabled = false;
		//agent.enabled = false;

	}

	public void Reset() {
		targetWithinLineOfSight = false;
		agent.Stop ();
		agent.ResetPath ();
		ResetWaiting (true);
		posCurrentNavPoint = 0;
		navTarget = navPoints [0];
		healthLeft = health;
		/*
		isWaiting = true;
		healthLeft = health;
		deathTimer = timeToRemoveOnDeath;
		cCollider.enabled = true;
		//agent.enabled = true;
		damageTimer = minTimeBetweenDamage;
		turn.Stop ();*/
	}

	void ResetWaiting(bool waiting) {
		isWaiting = waiting;
		elapsedWaitTime = 0f;
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
