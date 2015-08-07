using UnityEngine;
using System.Collections;

public class Weapon : FiringMechanism, IAimable {

	public AimLine aimLine;
	
	#region Aimable implementation
	public void IsAiming (bool value)
	{
		if (aimLine != null)
			aimLine.renderAim = value;
	}
	#endregion
}
