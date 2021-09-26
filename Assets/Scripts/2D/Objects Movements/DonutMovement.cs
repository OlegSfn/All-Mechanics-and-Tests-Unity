using UnityEngine;

public class DonutMovement : MonoBehaviour
{
    [Range(0.01f, 100f)] public float radius = 1f;
    [Range(0f, 360f)] public float angle = 0;
    [Range(0.01f, 100f)] public float speed = 2f;
    public Transform anchorPoint;
    public bool movingRight = true;
    public bool isMoving = true;

    float x, y;

	private void Update()
    {
        if (isMoving)
        {
            if (anchorPoint == transform)
			{
                x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
                y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            }
			else
			{
                x = anchorPoint.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
                y = anchorPoint.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            }

            if (movingRight) angle -= Time.deltaTime * speed * 50;
            else angle += Time.deltaTime * speed * 50;
            if (angle >= 360) angle = 0;

            transform.position = new Vector2(x, y);
        }
    }
}
