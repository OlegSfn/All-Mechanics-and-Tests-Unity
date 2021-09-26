using UnityEngine;

public class PointsMovement : MonoBehaviour
{
    // Create an empty objects and put into checkpoints to move
    public Transform pointsParent;
    public float speed;
    public LeanTweenType easeType;
    public enum MovementType {linear, loop, pingPong};
    public MovementType moveType = MovementType.linear;

    private int index = 0, increment = 1;

	private void OnDrawGizmos()
	{
        if (pointsParent.childCount < 2) return;
        if (moveType == MovementType.linear) Gizmos.color = Color.white;
        else if (moveType == MovementType.loop) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        for (int i = 0; i < pointsParent.childCount-1; i++)
		{
            Gizmos.DrawLine(pointsParent.GetChild(i).position, pointsParent.GetChild(i+1).position);
		}

        if (moveType == MovementType.loop) Gizmos.DrawLine(pointsParent.GetChild(pointsParent.childCount-1).position, pointsParent.GetChild(0).position);
	}

	void Awake()
    {
        if (pointsParent.childCount < 2)
        {
            Debug.LogError("There must be 2 or more points to animate");
            return;
        }

        Move();
    }

	private void Move()
	{
        Vector3 pointPos = pointsParent.GetChild(index).position;
        transform.LeanMove(pointPos, (transform.position-pointPos).magnitude / speed)
            .setEase(easeType)
            .setOnComplete(() =>
            {
                index += increment;
                if (index == pointsParent.childCount)
                {
                    if (moveType == MovementType.loop) index = 0;
                    else if (moveType == MovementType.pingPong)
                    {
                        increment = -1;
                        index -= 2;
                    }
                    else return;
                }
                else if (index == 0) increment = 1;

                Move();
            }
            );
    }
}
