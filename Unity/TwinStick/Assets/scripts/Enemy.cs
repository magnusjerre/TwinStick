using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public Transform[] navPoints;
	public float waitTime = 2f;
	
	Animator anim;
	NavMeshAgent agent;

	float elapsedWaitTime = 0f;
	bool isWaiting = false;
	int currentNavTarget = 0;

	void Awake() {
		transform.position = navPoints [0].position;
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		isWaiting = true;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (isWaiting) {
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
			Debug.Log("hello?");
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
}
