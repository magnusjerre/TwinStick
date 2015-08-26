using UnityEngine;
using System.Collections;

public class Turn : MonoBehaviour {
	
	public float turnSpeed = 0.5f;	//In seconds

	float elapsedTime = 0f;
	float turnTime;
	bool isFinished = true;
	public bool IsFinished 
	{
		get { return isFinished; }
		private set { isFinished = value; }
	}

	Quaternion startRotation;
	Quaternion targetRotation;

	void Start() {
		Debug.Log ("start called");
		Debug.Log ("transform: " + transform);
	}

	void Update () 
	{
		if (IsFinished) 
			return;

		elapsedTime += Time.deltaTime;
		float fraction = elapsedTime / turnTime;
		if (fraction < 1.001f) {
			transform.rotation = Quaternion.Lerp (startRotation, targetRotation, fraction);
		} else {
			Stop ();
		}
	}

	public void TurnTowards(Transform target) 
	{
		if (IsFinished)
		{
			IsFinished = false;
			elapsedTime = 0f;
			startRotation = transform.rotation;
			targetRotation = Quaternion.LookRotation(Diff (target.position, transform.position), new Vector3(0,1,0));

			float degrees;
			Vector3 outV;
			Quaternion.FromToRotation(transform.forward, Diff (target.position, transform.position)).ToAngleAxis(out degrees, out outV);
			turnTime = turnSpeed * (degrees / 360f);
		}
	}

	Vector3 Diff(Vector3 a, Vector3 b) 
	{
		return new Vector3(a.x - b.x, 0, a.z - b.z);
	}

	public void Stop()
	{
		elapsedTime = 0f;
		IsFinished = true;
	}
}
