using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public int damage = 100;
	public float speed = 10;
	public float timeToLive = 3.0f;

	float timer = 0f;
	bool isFired = false;
	Vector3 direction;

	void Update () {
		if (isFired) {
			timer += Time.deltaTime;
			transform.position += direction * speed * Time.deltaTime;
		}
	}

	public void Fire(Vector3 dir) {
		if (!isFired) {
			direction = dir;
			isFired = true;
		}
	}

	public bool TimeIsOver() {
		return timer > timeToLive;
	}

	public void ResetBullet() {
		timer = 0;
		isFired = false;
	}
}
