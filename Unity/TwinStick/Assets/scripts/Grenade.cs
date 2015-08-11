using UnityEngine;
using System.Collections;

public class Grenade : Projectile {

	public Explosion explosion;

	GameObject surface;
	MeshRenderer grenadeMesh;
	SphereCollider sphereCollider;

	public override void Awake() {
		base.Awake ();
		surface = GameObject.FindGameObjectWithTag ("Ground");
		grenadeMesh = GetComponent<MeshRenderer> ();
		sphereCollider = GetComponent<SphereCollider> ();
		Reset ();
	}

	public override void Fire() {
		Vector3 forceDir = transform.forward;
		forceDir += transform.up;
		body.AddForce(forceDir * speed);
	}

	// Update is called once per frame
	public override void Update () {
		
		if (gameObject.activeSelf) {
			timeLeft -= Time.deltaTime;
			if (timeLeft < explosion.ExplosionDuration()) {
				explosion.Explode(damage);
				grenadeMesh.enabled = false;
				sphereCollider.enabled = false;
			}
			if (timeLeft < 0f) {
				explosion.Reset();
				Reset();
			}
		}
		
	}

	public void OnCollisionEnter(Collision collision) {

		if (collision.gameObject == surface) {
			explosion.DisplayRadius(true);
		}

	}

	public override void Reset() {
		base.Reset ();
		grenadeMesh.enabled = true;
		sphereCollider.enabled = true;
	}

}
