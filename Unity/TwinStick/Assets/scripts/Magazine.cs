using UnityEngine;
using System.Collections;

[System.Serializable]
public class Magazine {
	
	public int size;
	public int maxCarriable;
	public Pool pool;

	int currentAmount = 0;

	public void Setup() {
		currentAmount = size;
		pool.Setup (); 
	}

	public void FireProjectile(Transform exit, Vector3 direction) {
		if (currentAmount != 0) {
			GameObject projectile = pool.FindAvailable();
			projectile.transform.position = exit.position;
			projectile.transform.rotation = exit.rotation;

			Projectile projectileScript = projectile.GetComponent<Projectile>();
			projectileScript.ResetTimer();
			projectileScript.Fire(direction);

			currentAmount--;
		}
	}

	public void Reload(int amount) {
		currentAmount += amount;
		if (amount > size)
			amount = size;
	}

	public int CurrentAmount() {
		return currentAmount;
	}
}
