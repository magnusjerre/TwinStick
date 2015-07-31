using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 20f;
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
				gameObject.SetActive(false);
			}
		}
	
	}

	public void ResetTimer() {
		timeLeft = timeToLive;
	}

	public void Fire() {
		body.velocity = body.transform.forward * speed;
	}

	public void OnCollisionEnter(Collision collision) {
		Debug.Log ("BAM! Collision!");

		Damageable damageable = collision.gameObject.GetComponent<Damageable> ();
		if (damageable != null) {
			damageable.DoDamage(damage);
		}

		gameObject.SetActive (false);
	}

}
