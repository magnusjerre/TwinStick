using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MedalSummary : MonoBehaviour, IMeasurable {

	public Text medalText, medalDescription;
	BoxCollider2D bCollider;

	void Awake() {
		bCollider = GetComponent<BoxCollider2D> ();
	}
	// Use this for initialization
	void Start () {
	}
	
	public void DisplayMedal(string medalText, string medalDescription) {
		this.medalText.text = medalText;
		this.medalDescription.text = medalDescription;
	}

	public Vector2 Size() {
		return bCollider.size;
	}
}
