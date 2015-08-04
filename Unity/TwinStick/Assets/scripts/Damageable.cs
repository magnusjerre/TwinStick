using UnityEngine;
using System.Collections;

public interface Damageable {
	void DoDamage (float damage, Vector3 point, Vector3 normal);
}
