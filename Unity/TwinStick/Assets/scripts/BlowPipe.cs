using UnityEngine;
using System.Collections;

public class BlowPipe : MonoBehaviour {

	public float fireRate = 4f;	//Max bullets per second
	public Transform muzzle;

	private float minTimeBetweenBullets;
	private float timeLeftToNextBullet = 0f;

	private BulletManager bManager;

	void Awake() {
		bManager = GetComponent<BulletManager> ();
		minTimeBetweenBullets = 1f / fireRate;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		timeLeftToNextBullet -= Time.deltaTime;
	
	}

	public void Fire() {

		if (timeLeftToNextBullet < 0f) {
			bManager.FireBullet(muzzle);
			timeLeftToNextBullet = minTimeBetweenBullets;
		}

	}
}
