using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] private GameObject KnifePrefab, CirclePrefab;
	[SerializeField] private Transform Circle;
	[HideInInspector] public bool needToSpawnKnife = true;
	[HideInInspector] public int knifeCount = 0;
	[SerializeField] private int knivesCountToEnterNewLevel = 5;


	private void Update()
	{
		if (needToSpawnKnife)
		{
			if (knifeCount >= knivesCountToEnterNewLevel)
			{
				Destroy(Circle.gameObject);
				Circle = Instantiate(CirclePrefab, Circle.position, Quaternion.identity).transform;
				knifeCount = 0;
			}

			Knife knife = Instantiate(KnifePrefab, transform.position, KnifePrefab.transform.rotation).GetComponent<Knife>();
			knife.KS = this;
			knife.Circle = Circle;
			needToSpawnKnife = false;
		}
	}
}
