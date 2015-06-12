using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {

	public float fireRate = 4;
	public Transform exitPoint, startPoint;

	float timer = 0f;
	float minTimeBetweenBullets;

	void Awake() {
		minTimeBetweenBullets = 1.0f / fireRate;
	}

	void Update () {
		timer += Time.deltaTime;
	}

	public bool CanFire() {
		if (timer > minTimeBetweenBullets) {
			timer = 0f;
			return true;
		}

		return false;
	}

}
