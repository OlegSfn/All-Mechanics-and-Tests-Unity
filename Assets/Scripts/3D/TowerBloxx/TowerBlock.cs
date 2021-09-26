using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBlock : MonoBehaviour
{
    [HideInInspector] public Transform cranePos;
	public bool isFirstCube = false, collided = false;
	[HideInInspector] public TowerBlockSpawner tbSpawner;
	[SerializeField] private Rigidbody rb;
	private bool isOnCrane = true;

	void Update()
    {
        if (cranePos != null && isOnCrane)   transform.position = cranePos.position;

		if (Input.GetMouseButtonDown(0))
		{
			isOnCrane = false;
			rb.useGravity = true;
		}
    }

	private void OnCollisionEnter(Collision col)
	{
		if ((col.gameObject.tag == "Cube" && !collided) || (isFirstCube && !collided))
		{
			if (isFirstCube) rb.isKinematic = true;
			tbSpawner.InstantiateTowerBlock(false);
			tbSpawner.towerBlocks.Add(this);
			collided = true;
			StartCoroutine(WaitToLift());
		}

		if (col.gameObject.tag == "Ground" && !isFirstCube)
		{
			if (collided)
			{
				tbSpawner.towerBlocks.Remove(this);
				tbSpawner.StartCoroutine(tbSpawner.Lift(-tbSpawner.distanceToMove));
			}
			else tbSpawner.InstantiateTowerBlock(false);
			tbSpawner.health--;
			Destroy(gameObject);
		}
	}

	IEnumerator WaitToLift()
	{
		yield return new WaitForSecondsRealtime(0.5f);
		if (!isFirstCube) tbSpawner.StartCoroutine(tbSpawner.Lift(tbSpawner.distanceToMove));
	}
}
