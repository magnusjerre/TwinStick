using UnityEngine;
using System.Collections;

public abstract class Owner : MonoBehaviour {
	public abstract void NotifyTargetAcquired(GameObject target);
	public abstract void NotifyTargetLost(GameObject target);
}
