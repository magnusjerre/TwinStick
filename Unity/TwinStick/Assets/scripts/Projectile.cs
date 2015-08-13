using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float speed = 300f;
	public float damage = 50f;
	public float timeToLive = 3f;
	public ProjectileType pType;

	protected float timeLeft;
	protected Rigidbody body;

	public virtual void Awake() {
		timeLeft = timeToLive;
		body = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	public virtual void Update () {

		if (gameObject.activeSelf) {
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0f) {
				Reset();
			}
		}
	
	}

	public virtual void Reset() {
		ResetTimer ();
		gameObject.SetActive (false);
		body.velocity = Vector3.zero;
	}

	public virtual void ResetTimer() {
		timeLeft = timeToLive;
	}

	public virtual void Fire(Vector3 direction) {
		body.AddForce (direction * speed);
		//body.AddForce (transform.forward * speed);
	}

	public virtual void OnTriggerEnter(Collider collider) {

		IDamageable damageable = collider.gameObject.GetComponent<IDamageable> ();
		if (damageable != null) {
			damageable.DoDamage(damage, transform.position, transform.forward * -1, pType);
		}

		gameObject.SetActive (false);
	}

}
