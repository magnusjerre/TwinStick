using UnityEngine;
using System.Collections;

public class BlowPipe : MonoBehaviour, Aimable {

	public float fireRate = 4f;	//Max bullets per second
	private float minTimeBetweenBullets;
	private float timeLeftToNextBullet = 0f;
	private Pool bManager;

	public Transform muzzle;
	private AimLine aimLine;

	void Awake() {
//		bManager = GetComponent<Pool> ();
		aimLine = GetComponent<AimLine> ();
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
			//bManager.FindAvailable(muzzle);
			timeLeftToNextBullet = minTimeBetweenBullets;
		}

	}
	
	#region Aimable implementation
	public void IsAiming (bool value)
	{
		aimLine.renderAim = value;
	}
	#endregion
}
