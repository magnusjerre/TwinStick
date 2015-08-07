using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float speed = 300f;
	public float damage = 50f;
	public float timeToLive = 3f;

	private float timeLeft;
	private Rigidbody body;

	void Awake() {
		timeLeft = timeToLive;
		body = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {

		if (gameObject.activeSelf) {
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0f) {
				body.velocity = Vector3.zero;
				ResetTimer();
				gameObject.SetActive(false);
			}
		}
	
	}

	public void ResetTimer() {
		timeLeft = timeToLive;
	}

	public void Fire() {
		body.AddForce (transform.forward * speed);
	}

	public void OnTriggerEnter(Collider collider) {

		Damageable damageable = collider.gameObject.GetComponent<Damageable> ();
		if (damageable != null) {
			damageable.DoDamage(damage, transform.position, transform.forward * -1);
		}

		gameObject.SetActive (false);
	}

}
