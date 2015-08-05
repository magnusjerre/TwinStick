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

	public void OnTriggerEnter(Collider collider) {
		Debug.Log ("BAM! Collided!");

		Damageable damageable = collider.gameObject.GetComponent<Damageable> ();
		if (damageable != null) {
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			Physics.Raycast (ray, out hit, 1f);
			damageable.DoDamage(damage, transform.position, transform.forward * -1);
		}

		gameObject.SetActive (false);
	}

}
