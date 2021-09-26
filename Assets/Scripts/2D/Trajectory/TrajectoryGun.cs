using UnityEngine;

public class TrajectoryGun : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 2;
    [SerializeField] private GameObject bulletPref;
    [SerializeField] private TrajectoryRenderer tr;

	void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseCamDifference = (mousePos - transform.position);
        Vector2 speed = mouseCamDifference * speedMultiplier;

        float angle = Vector2.Angle(Vector2.right, mouseCamDifference);
        transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < mousePos.y ? angle : -angle);

        //tr.RenderSimple2DTrajectory(transform.position, speed);
        tr.RenderComplicated2DTrajectory(bulletPref, transform.position, speed);
        if (Input.GetMouseButtonDown(0))
        {
            Rigidbody2D bullet = Instantiate(bulletPref, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            bullet.AddForce(speed, ForceMode2D.Impulse);
		}
    }
}
