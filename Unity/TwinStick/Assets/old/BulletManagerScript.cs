using UnityEngine;
using System.Collections;

public class BulletManagerScript : MonoBehaviour {

	public GameObject prefab;

	GameObject[] allBullets = new GameObject[40];

	void Awake() {
		for (int i = 0; i < allBullets.Length; i++) {
			GameObject bullet = (GameObject) Instantiate(prefab);
			allBullets[i] = bullet;
			bullet.SetActive(false);
		}
	}

	void Update() {
		foreach (GameObject bullet in allBullets) {
			if (bullet.activeSelf) {
				BulletScript bs = bullet.GetComponent<BulletScript>();
				if (bs.TimeIsOver()) {
					bs.ResetBullet();
					bullet.SetActive(false);
				}
			}
		}
	}
	
	public void FireBullet(Transform t, Vector3 dir) {
		foreach (GameObject bullet in allBullets) {
			if (!bullet.activeSelf) {
				bullet.SetActive(true);
				bullet.transform.position = t.position;
				bullet.transform.rotation = t.rotation;
				BulletScript bs = bullet.GetComponent<BulletScript>();
				bs.Fire(dir);
				break;
			}
		}

	}

}
