using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Medal : MonoBehaviour {

	public Vector2 start = new Vector2(50, 50);
	public Vector2 end = new Vector2(50, 400);
	public float animationTime = 2f;
	public Text numberText;

	Image image;
	float timer;
	bool display;
	
	void Start () {
		image = GetComponent<Image> ();
		timer = 0f;
	}

	void Update () {

		if (timer > animationTime) {
			timer = 0;
			display = false;
			gameObject.SetActive(false);
		}

		if (display) {
			timer += Time.deltaTime;
			float dt = timer / animationTime;
			float newX = Mathf.Lerp(start.x, end.x, dt);
			float newY = Mathf.Lerp(start.y, end.y, dt);
			image.transform.position = new Vector2(newX, newY);
		}
	}

	public void DisplayMedal(string number) {
		transform.position = new Vector3(start.x, start.y);
		display = true;
		numberText.text = number;
	}

}
