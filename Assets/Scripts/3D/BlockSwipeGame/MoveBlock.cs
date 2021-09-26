using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    [SerializeField] private bool freezeX, freezeZ;
    [SerializeField] private float dragSpeed = 10f, maxSpeed = 5f;

    private int freezeXInt, freezeZInt;
    private Rigidbody rb;

	private void Awake()
	{
        freezeXInt = (freezeX) ? 0 : 1;
        freezeZInt = (freezeZ) ? 0 : 1;
        rb = GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            if (rb.velocity.magnitude < maxSpeed)
			{
                rb.velocity += new Vector3(horizontal * Time.deltaTime * dragSpeed * freezeXInt, 0f, vertical * Time.deltaTime * dragSpeed * freezeZInt);
			}
        }
        else rb.velocity = Vector3.zero;
    }
}
