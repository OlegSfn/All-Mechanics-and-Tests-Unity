using UnityEngine;

public class Building : MonoBehaviour
{
	public Vector2Int size = Vector2Int.one;
	public Direction dir;
	public Renderer meshRenderer;

	[HideInInspector] public Vector2Int projection;
	private Color defaultColor;

	// Forward - y+ Back - y- Left x- Right x+
	private void Awake()
	{
		defaultColor = meshRenderer.material.color;
		ChangeProjection();
	}

	public void SetTransparentColor(bool available)
	{
		if (available)
			meshRenderer.material.color = Color.green;
		else
			meshRenderer.material.color = Color.red;
	}

	public void SetNormalColor()
	{
		meshRenderer.material.color = defaultColor;
	}

	public void RecalculateProjection()
	{
		dir = (Direction)((int)dir+1);
		if ((int)dir == 4) dir = 0;
		ChangeProjection();
	}

	private void ChangeProjection()
	{
		if (dir == Direction.Forward) projection = new Vector2Int(1, 1);
		else if (dir == Direction.Back) projection = new Vector2Int(1, -1);
		else if (dir == Direction.Left) projection = new Vector2Int(-1, 1);
		else if (dir == Direction.Right) projection = new Vector2Int(1, 1);
	}

	private void OnDrawGizmosSelected()
	{
		for (int x = 0; x < size.x; x++)
			for (int y = 0; y < size.y; y++)
			{
				Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);
				Gizmos.DrawCube(transform.position + new Vector3(x * projection.x, 0, y * projection.y), new Vector3(1f, 0.5f, 1f));
			}
	}
}
