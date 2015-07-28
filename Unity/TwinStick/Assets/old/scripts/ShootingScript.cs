using UnityEngine;
using System.Collections;

public class ShootingScript : MonoBehaviour {

	public Transform leftHand, rightHand;
	public GameObject gunPrefab;
	public BulletManagerScript bms;

	Transform leftGunExitPoint, rightGunExitPoint;
	Transform leftGunStartPoint, rightGunStartPoint;

	GunScript leftGunScript, rightGunScript;

	void Awake() {
		GameObject leftGun = (GameObject) Instantiate (gunPrefab);
		GameObject rightGun = (GameObject) Instantiate (gunPrefab);

		leftGun.transform.parent = leftHand.transform;
		leftGun.transform.localPosition = Vector3.zero;
		leftGun.transform.localRotation = Quaternion.identity;

		rightGun.transform.parent = rightHand.transform;
		rightGun.transform.localPosition = Vector3.zero;
		rightGun.transform.localRotation = Quaternion.identity;

		leftGunScript = leftGun.GetComponent<GunScript> ();
		leftGunExitPoint = leftGunScript.exitPoint;
		leftGunStartPoint = leftGunScript.startPoint;

		rightGunScript = rightGun.GetComponent<GunScript> ();
		rightGunExitPoint = rightGunScript.exitPoint;
		rightGunStartPoint = rightGunScript.startPoint;

	}


	public void Fire() {
		if (rightGunScript.CanFire () && leftGunScript.CanFire ()) {
			bms.FireBullet (leftGunExitPoint, leftGunExitPoint.position - leftGunStartPoint.position);
			bms.FireBullet (rightGunExitPoint, rightGunExitPoint.position - rightGunStartPoint.position);
		}
	}
}
