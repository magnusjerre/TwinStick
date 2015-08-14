using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject character;
	public int totalLives;
	public Canvas canvas;
	public GameOver gameOver;
	public ScoreManager scoreManager;

	void Start() {
		gameOver.gameObject.SetActive (false);
		scoreManager.gameObject.SetActive (true);
	}

	public void RegisterDeath(GameObject deadCharacter) {
		if (character == deadCharacter) {
			totalLives--;
			if (totalLives < 1) {
				gameOver.gameObject.SetActive(true);
				scoreManager.gameObject.SetActive(false);
			}
		}
	}

}

