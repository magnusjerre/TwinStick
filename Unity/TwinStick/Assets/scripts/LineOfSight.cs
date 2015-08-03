using UnityEngine;
using System.Collections;

public class LineOfSight : MonoBehaviour {

	public GameObject target;
	public Owner observer;

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject == target)
			observer.NotifyTargetAcquired (target);
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject == target)
			observer.NotifyTargetLost (target);
	}

}
