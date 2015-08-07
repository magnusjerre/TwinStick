using UnityEngine;
using System.Collections;

public class BlowPipe : FiringMechanism, Aimable {

	public AimLine aimLine;

	#region Aimable implementation
	public void IsAiming (bool value)
	{
		aimLine.renderAim = value;
	}
	#endregion
}
