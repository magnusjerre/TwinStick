using UnityEngine;
using System.Collections;

public class HandWeapon : MonoBehaviour {

	public float damage = 75f;
	public GameObject owner;

	public bool doDamageAllowed = false;

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject == owner)	//Implies that the collider collided with the entity wielding the hand weapon
			return;

		if (!doDamageAllowed)
			return;

		IDamageable damageable = collider.gameObject.GetComponent<IDamageable> ();
		if (damageable != null) {
			damageable.DoDamage(damage, Vector3.zero, Vector3.zero, ProjectileType.NONE);
		}
	}
}
