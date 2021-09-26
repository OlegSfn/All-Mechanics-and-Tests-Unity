using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FInish : MonoBehaviour
{
	private void OnTriggerEnter(Collider col)
	{
		Debug.Log("You've won");
	}
}
