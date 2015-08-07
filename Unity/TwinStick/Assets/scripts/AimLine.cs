using UnityEngine;
using System.Collections;

public class AimLine : MonoBehaviour {

	public Transform startTransf;
	public float maxAimDistance = 10f;
	public bool renderAim = false;
	public LineRenderer mLineRenderer;


	// Update is called once per frame
	void Update () {
		
		if (renderAim) {
			mLineRenderer.enabled = true;
			RenderLine ();
		} else {
			mLineRenderer.enabled = false;
		}
		
	}
	
	public void RenderLine() {
		Ray ray = new Ray (startTransf.position, startTransf.forward);
		RaycastHit rHit;

		int layerMask = 1 << 8 | 1 << 9;
		layerMask = ~layerMask;

		if (Physics.Raycast (ray, out rHit, 10f, layerMask)) {
			mLineRenderer.SetPosition (1, transform.InverseTransformPoint (startTransf.position + startTransf.forward * rHit.distance));
		} else {
			mLineRenderer.SetPosition(1, transform.InverseTransformPoint(startTransf.position + startTransf.forward * maxAimDistance));
		}
	}
}


