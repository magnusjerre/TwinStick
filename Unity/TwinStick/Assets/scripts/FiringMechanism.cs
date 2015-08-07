using UnityEngine;
using System.Collections;

public class FiringMechanism : MonoBehaviour {

	public float fireRate = 4f;	//Max projectiles fired per second
	public Transform muzzle;
	public Magazine magazine;

	private float minTimeBetweenFire;
	private float timeLeftToFire = 0f;
	
	void Awake() {
		//pManager = GetComponent<ProjectileManager> ();
		minTimeBetweenFire = 1f / fireRate;
		magazine.Setup ();
	}

	// Update is called once per frame
	void Update () {
		timeLeftToFire -= Time.deltaTime;
	}
	
	public void Fire() {
		
		if (timeLeftToFire < 0f) {
			magazine.FireProjectile(muzzle);
			timeLeftToFire = minTimeBetweenFire;
		}
		
	}



}
