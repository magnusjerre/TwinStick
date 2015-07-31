using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {

	public GameObject prefab;
	public int size = 20;

	private GameObject[] bullets;

	void Awake() {
		bullets = new GameObject[size];
		for (int i = 0; i < size; i++) {
			bullets[i] = (GameObject) Instantiate(prefab);
			bullets[i].SetActive(false);
		}
	}

	public void FireBullet(Transform transf) {

		foreach (GameObject bullet in bullets) {
			if (!bullet.activeSelf) {
				bullet.SetActive(true);
				bullet.transform.position = transf.position;
				bullet.transform.rotation = transf.rotation;
				Bullet bulletScript = bullet.GetComponent<Bullet>();
				bulletScript.ResetTimer();
				bulletScript.Fire();
				break;
			}
		}

	}
}
