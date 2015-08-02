using UnityEngine;
using System.Collections;

public class Leaf : MonoBehaviour {

	private Vector3 origEulerAngles;
	private Vector3 directionMultiplier;
	
	private float[] rotations = new float[] {13f	,25f	,10f	,9f		,16f	,30f	,11f	,10f	,6f		,19f};
	private float[] times = new float[] 	{1.5f	,1.5f	,1f		,1.5f	,1.5f	,1.5f	,1.5f	,1.5f	,0.8f	,1.5f};
	private float[] stops = new float[] {0f, 0f, 0f};
	private float steps = 4f;

	private int pos = 0;
	private float elapsedTime = 0f;

	// Use this for initialization
	void Start () {
		origEulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		pos = Random.Range (0, rotations.Length - 1);
		changeRotations ();
	}
	
	// Update is called once per frame
	void Update () {

		if (elapsedTime > stops [2]) {
			elapsedTime = 0f;
			changeRotations();
		}

		elapsedTime += Time.deltaTime;

		if (elapsedTime < stops [0]) {
			directionMultiplier.x = 1f;
		} else if (elapsedTime < stops [1]) {
			directionMultiplier.x = -1f;
		} else {
			directionMultiplier.x = 1f;
		}

		float velocity = rotations [pos] / times[pos] ;
		float step = velocity * Time.deltaTime;
		float rot = step * directionMultiplier.x;
		transform.Rotate (rot, rot, rot);
	}

	void changeRotations() {
		pos = (pos + 1) % rotations.Length;

		float timePerStep = times [pos] / steps;
		stops [0] = timePerStep;
		stops [1] = timePerStep * 3;
		stops [2] = timePerStep * 4;

		transform.rotation = Quaternion.Euler (origEulerAngles);
	}


}
