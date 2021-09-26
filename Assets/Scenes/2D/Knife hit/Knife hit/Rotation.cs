using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotation : MonoBehaviour
{
	private float rotationSpeed, nextRotationSpeed;
	[SerializeField] private float minSpeed = 0.2f, maxSpeed = 2f;
	[SerializeField] private float minDelay = 1f, maxDelay = 5f;

	private void Awake()
	{
		rotationSpeed = Random.Range(minSpeed, maxSpeed);
		StartCoroutine(Rotation());
	}

	private void Update()
	{
		transform.Rotate(new Vector3(0,0, rotationSpeed));
	}

	private IEnumerator Rotation()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
			nextRotationSpeed = Random.Range(minSpeed, maxSpeed);
			StartCoroutine(SmoothSpeedChange());
		}
	}

	private IEnumerator SmoothSpeedChange()
	{
		while (Mathf.Abs(nextRotationSpeed - rotationSpeed) >= 0.05f)
		{
			rotationSpeed = Mathf.MoveTowards(rotationSpeed, nextRotationSpeed, Time.deltaTime);
			yield return null;
		}
		yield break;
	}
}
