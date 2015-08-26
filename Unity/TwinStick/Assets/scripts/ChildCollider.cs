using UnityEngine;
using System.Collections;

public class ChildCollider : MonoBehaviour 
{
	[Header("Must implement IColliderListener")]
	public GameObject colliderListener;

	IColliderListener cListener;
	Collider thisCollider;
	
	void Start () 
	{
		thisCollider = GetComponent<Collider> ();
		cListener = colliderListener.GetComponent<IColliderListener> ();
		if (cListener == null) 
		{
			throw new UnityException("The object provided doesn't implement the IColliderListener interface. ");
		}
	}

	void OnTriggerEnter(Collider collider) 
	{
		cListener.OnColliderEnter (thisCollider, collider);
	}

	void OnTriggerStay(Collider collider) 
	{
		cListener.OnColliderStay (thisCollider, collider);
	}

	void OnTriggerExit(Collider collider) 
	{
		cListener.OnColliderExit (thisCollider, collider);
	}
}
