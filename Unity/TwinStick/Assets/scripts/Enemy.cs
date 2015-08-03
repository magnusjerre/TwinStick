using UnityEngine;
using System.Collections;

public class Enemy : Owner {

	public Transform[] navPoints;
	public float waitTime = 2f;
	
	Animator anim;
	NavMeshAgent agent;

	float elapsedWaitTime = 0f;
	bool isWaiting = false;
	int currentNavTarget = 0;
	bool playerInSight = false;
	float minPlayerFollowDt = 0.2f;
	float playerFollowLeft = -1f;
	GameObject target;

	void Awake() {
		transform.position = navPoints [0].position;
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		target = GameObject.FindGameObjectWithTag ("Player");
		isWaiting = true;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

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
}
