using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
	public Vector3 RotationSpeed = new Vector3(0,0,0.2f);
	private void Update()
	{
		transform.Rotate(RotationSpeed);
	}
}
