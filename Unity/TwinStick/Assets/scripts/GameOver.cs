using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameOver : MonoBehaviour {

	public Text totalKills, totalBullets, totalGrenadeKills;
	public Image medalArea;

	public ScoreManager scoreManager;

	void Awake() {
		//scoreManager = GameObject.FindGameObjectWithTag ("ScoreManager").GetComponent<ScoreManager>();
	}
	// Use this for initialization
	void Start () {
		UpdateScore ();
	}
	
	public void UpdateScore() {
		totalKills.text = "" + scoreManager.TotalKills ();
		totalBullets.text = "" + scoreManager.kills [ProjectileType.BULLET];
		totalGrenadeKills.text = "" + scoreManager.kills [ProjectileType.GRENADE];
	}
}
