using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
	private LineRenderer lr;
	[SerializeField] private float periodsToCheckPhysics = 0.05f;

	private void Awake()
	{
		lr = GetComponent<LineRenderer>();
	}

	public void RenderSimple2DTrajectory(Vector3 origin, Vector3 speed)
	{
		Vector3[] points = new Vector3[100];
		lr.positionCount = points.Length;
		for (int i = 0; i < points.Length; i++)
		{
			float time = i * periodsToCheckPhysics;
			points[i] = origin + speed*time + Physics.gravity*time*time/2f;
			if (points[i].y < -2)
			{
				lr.positionCount = i;
				break;
			}
		}
		lr.SetPositions(points);
	}

	// Need a lot of resources
	public void RenderComplicated2DTrajectory(GameObject bulletPref, Vector3 origin, Vector3 speed)
	{
		// Preparing:
		Rigidbody2D bullet = Instantiate(bulletPref, origin, Quaternion.identity).GetComponent<Rigidbody2D>();
		bullet.AddForce(speed, ForceMode2D.Impulse);

		Vector3[] points = new Vector3[100];
		lr.positionCount = points.Length;

		// Simulation
		Physics2D.simulationMode = SimulationMode2D.Script;
		for (int i = 0; i < points.Length; i++)
		{
			Physics2D.Simulate(periodsToCheckPhysics);
			points[i] = bullet.transform.position;
		}

		lr.SetPositions(points);
		Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
		Destroy(bullet.gameObject);
	}
}
