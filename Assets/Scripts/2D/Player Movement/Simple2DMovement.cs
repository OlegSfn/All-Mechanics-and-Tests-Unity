using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple2DMovement : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] [Range(0f, 10f)] private float speed;
	private float defaultSpeed;
	[SerializeField] [Range(0f, 10f)] private float runSpeed;
	[SerializeField] private bool canRun;
	private float horizontalDir, verticalDir;

	private void Awake()
	{
		defaultSpeed = speed;
		if (runSpeed < speed) runSpeed = speed;
	}

	private void Update()
	{
		rb.velocity = Vector2.zero;

		horizontalDir = Input.GetAxisRaw("Horizontal");
		verticalDir = Input.GetAxisRaw("Vertical");


		if (horizontalDir != 0) rb.velocity += Vector2.right * speed * horizontalDir;
		if (verticalDir != 0) rb.velocity += Vector2.up * speed * verticalDir;

		if (horizontalDir != 0 && verticalDir != 0)
		{
			rb.velocity = new Vector2(rb.velocity.x * Mathf.Cos(45 * Mathf.Deg2Rad), 
				rb.velocity.y * Mathf.Cos(45 * Mathf.Deg2Rad));
		}

		if (canRun)
		{
			if (Input.GetButton("Run")) speed = runSpeed;
			else speed = defaultSpeed;
		}
	}
}
