using UnityEngine;
using System.Collections;

[System.Serializable]
public class Pool {

	public int size;
	public GameObject prefab;

	GameObject[] projectiles;

	public void Setup() {
		projectiles = new GameObject[size];
		for (int i = 0; i < size; i++) {
			projectiles[i] = (GameObject) MonoBehaviour.Instantiate(prefab);
			projectiles[i].SetActive(false);
		}
	}

	public GameObject FindAvailable() {

		foreach (GameObject projectile in projectiles) {
			if (!projectile.activeSelf) {
				projectile.SetActive(true);
				return projectile;
			}
		}

		return null;

	}
}
