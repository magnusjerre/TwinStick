using System;
using UnityEngine;

public interface IColliderListener
{

	void OnColliderEnter(Collider owner, Collider collider);
	void OnColliderStay(Collider owner, Collider collider);
	void OnColliderExit(Collider owner, Collider collider);

}


