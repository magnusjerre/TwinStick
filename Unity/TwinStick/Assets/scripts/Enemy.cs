using UnityEngine;
using System.Collections;

public class Enemy : Owner, IDamageable {

	public Transform[] navPoints;
	public float waitTime = 2f;
	public float health = 100f;
	public ParticleSystem particleSystemPrefab;
	public float timeToRemoveOnDeath = 3f;
	
	Animator anim;
	NavMeshAgent agent;

	float healthLeft;
	float elapsedWaitTime = 0f;
	bool isWaiting = false;
	int currentNavTarget = 0;
	float deathTimer;

	bool playerInSight = false;
	float minPlayerFollowDt = 0.2f;
	float playerFollowLeft = -1f;

	GameObject target;
	CapsuleCollider cCollider;
	ParticleSystem particles;

	void Awake() {
		transform.position = navPoints [0].position;
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		target = GameObject.FindGameObjectWithTag ("Player");
		cCollider = GetComponent<CapsuleCollider> ();
		Reset ();
		particles = (ParticleSystem)Instantiate (particleSystemPrefab);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameObject.activeSelf)
			return;

		if (healthLeft < 1) {
			deathTimer -= Time.deltaTime;
			if (deathTimer < 0f) {
				Reset();
				gameObject.SetActive(false);
			}
			return;
		}

		if (playerInSight) {
			playerFollowLeft -= Time.deltaTime;
			if (playerFollowLeft < 0) {
				playerFollowLeft = minPlayerFollowDt;
				agent.destination = target.transform.position;
			}
		}
		if (!playerInSight && isWaiting) {
			elapsedWaitTime += Time.deltaTime;
			anim.SetBool ("isMoving", false);
			anim.SetFloat("yDir", 0.75f);

			if (elapsedWaitTime > waitTime) {
				elapsedWaitTime = 0f;
				currentNavTarget = (currentNavTarget + 1) % navPoints.Length;
				Debug.Log ("gameobject.activeSelf: " + gameObject.activeSelf);
				agent.destination = navPoints [currentNavTarget].position;
				isWaiting = false;
			}

		} else {
			anim.SetBool ("isMoving", true);
			anim.SetFloat("yDir", 0.75f);
		}

		if (hasReachedTarget()) {
			isWaiting = true;
		}

	}

	bool hasReachedTarget() {

		if (agent.pathPending)
			return false;

		if (agent.velocity.sqrMagnitude > 0f)
			return false;

		if (agent.hasPath)
			return false;

		return true;
	}


	#region implemented abstract members of Owner
	public override void NotifyTargetAcquired (GameObject target)
	{
		Debug.Log ("Target acquired!");
		playerInSight = true;

	}
	public override void NotifyTargetLost (GameObject target)
	{
		Debug.Log ("Target lost...");
		playerInSight = false;
	}
	#endregion

	#region Damageable implementation

	public void DoDamage (float damage, Vector3 point, Vector3 normal)
	{
		healthLeft -= damage;

		particles.transform.position = point;
		particles.transform.LookAt (point + normal);
		particles.Play ();

		if (healthLeft < 1) {
			//Debug.Log ("enemy down");
			anim.SetTrigger("death");
			DisableBoxes();
		}

	}

	#endregion

	void DisableBoxes() {
		cCollider.enabled = false;
		agent.enabled = false;
	}

	public void Reset() {
		isWaiting = true;
		healthLeft = health;
		deathTimer = timeToRemoveOnDeath;
		cCollider.enabled = true;
		agent.enabled = true;
	}
}
