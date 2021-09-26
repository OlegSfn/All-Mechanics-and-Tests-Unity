using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knife : MonoBehaviour
{
	// Place trigger collider to make knives collide and simple collider to make knife stab
    [HideInInspector] public Spawner KS;
	[HideInInspector] public Transform Circle;
	private bool collided = false;
	private bool isStabbed = false, buttonPressed = false;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0)) buttonPressed = true;
		if (!isStabbed && buttonPressed) transform.Translate(0, 0, 0.05f);
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("Knife")) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("Circle") && !collided)
		{
			transform.SetParent(Circle);
			collided = true;
			isStabbed = true;
			KS.needToSpawnKnife = true;
			KS.knifeCount++;
		}
	}
}
