using UnityEngine;
using System.Collections;

public class BlowPipe : MonoBehaviour {

	public float maxAimDistance = 10f;
	public float fireRate = 4f;	//Max bullets per second
	public Transform muzzle;
	public bool renderAim = false;

	private float minTimeBetweenBullets;
	private float timeLeftToNextBullet = 0f;

	private BulletManager bManager;
	private LineRenderer mLineRenderer;


	void Awake() {
		bManager = GetComponent<BulletManager> ();
		minTimeBetweenBullets = 1f / fireRate;
		mLineRenderer = GetComponentInChildren<LineRenderer> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		timeLeftToNextBullet -= Time.deltaTime;

		if (renderAim) {
			mLineRenderer.enabled = true;
			RenderLine ();
		} else {
			mLineRenderer.enabled = false;
		}

	}

	public void RenderLine() {
		Ray ray = new Ray (muzzle.position, transform.forward);
		RaycastHit rHit;
		int layerMask = 1 << 8;
		layerMask = ~layerMask;
		if (Physics.Raycast (ray, out rHit, 10f, layerMask)) {
			mLineRenderer.SetPosition (1, transform.InverseTransformPoint (muzzle.position + (transform.forward * rHit.distance)));
		} else {
			mLineRenderer.SetPosition(1, transform.InverseTransformPoint(muzzle.position + transform.forward * maxAimDistance));
		}
	}

	public void Fire() {

		if (timeLeftToNextBullet < 0f) {
			bManager.FireBullet(muzzle);
			timeLeftToNextBullet = minTimeBetweenBullets;
		}

	}
}
