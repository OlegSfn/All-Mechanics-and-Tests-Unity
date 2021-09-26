using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementByMouse2D : MonoBehaviour
{
	[SerializeField] [Range(0f, 10f)] private float zoomVal;
	[SerializeField] [Range(0.01f, 100f)] private float maxZoomIn, maxZoomOut;
	[SerializeField] [Range(0f, 100f)] private float dragSpeed;
	private Camera cam;

	private void Awake()
	{
		cam = Camera.main;
	}

	private void Update()
	{
		// Zoom control
		if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && cam.orthographicSize > maxZoomIn)
		{
			dragSpeed /= cam.orthographicSize/(cam.orthographicSize-zoomVal);
			cam.orthographicSize = Mathf.Clamp(cam.orthographicSize-zoomVal, maxZoomIn, maxZoomOut);
		}
		else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && cam.orthographicSize < maxZoomOut)
		{
			dragSpeed /= cam.orthographicSize/(cam.orthographicSize+zoomVal);
			cam.orthographicSize = Mathf.Clamp(cam.orthographicSize+zoomVal, maxZoomIn, maxZoomOut);
		}

		// Drag movement
		if (Input.GetMouseButton(0))
		{
			transform.position -= new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0f);
		}
	}
}
