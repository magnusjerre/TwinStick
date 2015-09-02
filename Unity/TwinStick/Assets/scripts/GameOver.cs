using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameOver : MonoBehaviour {

	public Text totalKills, totalBullets, totalGrenadeKills;
	public Image medalArea;

	public ScoreManager scoreManager;
	public GameObject medalStaticPrefab;
	public GridLayoutGroup glg;
	public GridExtras gExtras;

	void Awake() {
		//scoreManager = GameObject.FindGameObjectWithTag ("ScoreManager").GetComponent<ScoreManager>();
	}
	// Use this for initialization
	void Start () {
		UpdateScore ();
	}
	
	public void UpdateScore() {
		Debug.Log ("Update score");
		totalKills.text = "" + scoreManager.TotalKills ();
		totalBullets.text = "" + scoreManager.kills [ProjectileType.BULLET];
		totalGrenadeKills.text = "" + scoreManager.kills [ProjectileType.GRENADE];

		int counter = 0;
		foreach (KeyValuePair<int, int> pair in scoreManager.multiKills) {
			if (counter < gExtras.MaxElements) {
				GameObject medal = Instantiate(medalStaticPrefab);
				medal.GetComponent<MedalSummary>().DisplayMedal("" + pair.Key, "x" + pair.Value);
				medal.transform.SetParent(glg.gameObject.transform);
				counter++;
			}
		}
	}
}
