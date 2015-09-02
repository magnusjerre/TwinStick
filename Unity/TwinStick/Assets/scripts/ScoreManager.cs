using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {

	public float maxTimeBetweenKills = 2f;
	public Pool pool;
	public Canvas canvas;

	public Dictionary<ProjectileType, int> kills;

	public Dictionary<int, int> multiKills;
	float timeSinceLastKill;
	int multiKillCounter = 0;
	float minTimeBetweenMedals = 0.25f;
	float tbmTimer;
	bool displayMedal = false;
	GameObject[] medalQueue;
	int[] killCounterQueue;

	void Awake() {
		kills = new Dictionary<ProjectileType, int>(); 
		multiKills = new Dictionary<int, int>();
		pool.Setup ();
		medalQueue = new GameObject[pool.size];
		killCounterQueue = new int[pool.size];
	}
	// Use this for initialization
	void Start () {
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceLastKill += Time.deltaTime;
		tbmTimer -= Time.deltaTime;
		if (displayMedal) {
			if (tbmTimer < 0f) {
				//GameObject obj = RemoveFromQueue();
				GameObject obj = medalQueue[0];
				int kills = killCounterQueue[0];
				RemoveFromQueue();
				if (obj != null) {
					obj.transform.SetParent(canvas.transform);
					obj.GetComponent<Medal>().DisplayMedal("" + kills);
				} else {
					displayMedal = false;
				}
				tbmTimer = minTimeBetweenMedals;
				Debug.Log("reset tbmTimer: " + tbmTimer);
			}
		}

	}

	public void RegisterKill(ProjectileType type) {
		kills [type] += 1;
		Debug.Log ("Kill registered");
		if (timeSinceLastKill < maxTimeBetweenKills) {
			multiKillCounter++;
			if (!multiKills.ContainsKey(multiKillCounter))
			    multiKills.Add(multiKillCounter, 0);
			multiKills[multiKillCounter] += 1;
			GameObject obj = pool.FindAvailable();
			if (obj != null) {
				AddToQueue(obj, multiKillCounter);
				displayMedal = true;
			}
		} else {
			multiKillCounter = 1;
		}

		timeSinceLastKill = 0;

	}

	public void Reset() {
		Debug.Log ("Reset ScoreMaanger");
		multiKillCounter = 0;
		timeSinceLastKill = maxTimeBetweenKills;
		tbmTimer = minTimeBetweenMedals;
		kills.Clear ();
		kills.Add (ProjectileType.BULLET, 0);
		kills.Add (ProjectileType.GRENADE, 0);
	}

	void AddToQueue(GameObject obj, int kills) {
		for (int i = 0; i < medalQueue.Length; i++) {
			if (medalQueue[i] == null) {
				medalQueue[i] = obj;
				killCounterQueue[i] = kills;
			}
		}
	}

	void RemoveFromQueue() {

		GameObject[] tempQueue = new GameObject[pool.size];
		int[] tempKillCounterQueue = new int[pool.size];
		for (int i = 1; i < medalQueue.Length; i++) {
			tempQueue[i-1] = tempQueue[i];
			tempKillCounterQueue[i-1] = killCounterQueue[i];
		}
		medalQueue = tempQueue;
		killCounterQueue = tempKillCounterQueue;

	}

	public int TotalKills() {
		int sum = 0;
		foreach (KeyValuePair<ProjectileType, int> pair in kills) {
			sum += pair.Value;
		}
		return sum;
	}
}
