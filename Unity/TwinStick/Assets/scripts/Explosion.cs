using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : MonoBehaviour {

	public ParticleSystem ps;
	public int maxObjectsInsideExplosion = 10;

	GameObject ground;
	GameObject[] insideBlastRadius;
	MeshRenderer meshRenderer;
	
	void Awake() {
		meshRenderer = GetComponent<MeshRenderer> ();
	}

	void Start() {
		insideBlastRadius = new GameObject[maxObjectsInsideExplosion];	
		Reset ();
	}

	void FixedUpdate() {
		transform.LookAt (transform.position + Vector3.forward);	//Object is apparently rotated, the intuitive thing would be to use Vector3.up...
	}

	public void OnTriggerEnter(Collider collider) {

		IDamageable damageable = collider.gameObject.GetComponent<IDamageable> ();
		if (damageable != null && PosOfGameObject (collider.gameObject) == -1)
			AddGameObject (collider.gameObject);
		
	}

	void OnTriggerExit(Collider collider) {
		RemoveGameObject (collider.gameObject);
	}
	
	int PosOfGameObject(GameObject obj) {
		
		for (int i = 0; i < insideBlastRadius.Length; i++) {
			if (insideBlastRadius[i] == obj) {
				return i;
			}
		}
		
		return -1;
	}

	void RemoveGameObject(GameObject obj) {
		int pos = PosOfGameObject (obj);
		if (pos != -1) {
			insideBlastRadius[pos] = null;
		}
	}

	void AddGameObject(GameObject obj) {
		for (int i = 0; i < insideBlastRadius.Length; i++) {
			if (insideBlastRadius[i] == null)
				insideBlastRadius[i] = obj;
			break;
		}
	}

	public void Explode(float damage) {
		if (!ps.isPlaying) {
			for (int i = 0; i < insideBlastRadius.Length; i++) {
				if (insideBlastRadius[i] != null)
					insideBlastRadius[i].GetComponent<IDamageable>().DoDamage(damage, Vector3.zero, Vector3.zero);
			}
			ps.Play ();
			DisplayRadius(false);
		}
	}

	public void DisplayRadius(bool value) {
		meshRenderer.enabled = value;
	}

	public void Reset() {
		for (int i = 0; i < insideBlastRadius.Length; i++) {
			insideBlastRadius[i] = null;
		}
		ps.Stop ();
		DisplayRadius (false);
	}

	public float ExplosionDuration() {
		return ps.duration;
	}
}
