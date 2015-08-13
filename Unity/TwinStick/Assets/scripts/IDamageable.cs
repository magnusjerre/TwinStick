using UnityEngine;
using System.Collections;

public interface IDamageable {
	void DoDamage (float damage, Vector3 point, Vector3 normal, ProjectileType type);
}
