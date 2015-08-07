using UnityEngine;
using System.Collections;

public class FiringMechanism : MonoBehaviour {

	public float fireRate = 4f;	//Max projectiles fired per second
	public Transform muzzle;
	public Magazine magazine;

	private float minTimeBetweenFire;
	private float timeLeftToFire = 0f;
	
	public virtual void Awake() {
		minTimeBetweenFire = 1f / fireRate;
		magazine.Setup ();
	}

	// Update is called once per frame
	void Update () {
		timeLeftToFire -= Time.deltaTime;
	}
	
	public virtual void Fire() {
		
		if (timeLeftToFire < 0f) {
			magazine.FireProjectile(muzzle);
			timeLeftToFire = minTimeBetweenFire;
		}
		
	}



}
