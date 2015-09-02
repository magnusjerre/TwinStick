using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public float spawnInterval = 3f;	//In seconds 
	public Transform spawnLocation;

	public Pool pool;

	float timer;

	// Use this for initialization
	void Start () {
		timer = spawnInterval;
		pool.Setup ();
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer < 0) {
			timer = spawnInterval;
			Spawn();
		}
	}

	public void Spawn()  {
		GameObject enemy = pool.FindAvailable();
		if (enemy != null) {
			enemy.transform.position = spawnLocation.position;
			enemy.transform.rotation = spawnLocation.rotation;
			enemy.GetComponent<Enemy>().Reset();
		}
	}

}
