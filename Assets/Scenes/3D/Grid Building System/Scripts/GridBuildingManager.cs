using UnityEngine;

public class GridBuildingManager : MonoBehaviour
{
	public Vector2Int gridSize = Vector2Int.one * 10;
	private Building[,] grid;
	private Building flyingBuilding;

	public Camera cam;

	private void Awake()
	{
		grid = new Building[gridSize.x, gridSize.y];
	}

	private void Update()
	{
		if (flyingBuilding != null)
		{
			StartPlacingBuilding();
		}
	}

	public void CreateBuilding(Building buildingPrefab)
	{
		if (flyingBuilding != null) Destroy(flyingBuilding.gameObject);
		flyingBuilding = Instantiate(buildingPrefab);
	}
	
	private void StartPlacingBuilding()
	{
		var groundPlane = new Plane(Vector3.up, Vector3.zero);
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);

		if (groundPlane.Raycast(ray, out float pos))
		{
			Vector3 worldPos = ray.GetPoint(pos);
			int x = Mathf.RoundToInt(worldPos.x);
			int y = Mathf.RoundToInt(worldPos.z);

			if (Input.GetMouseButtonDown(1)) RotateBuilding();
			bool available = canPlaceBuilding(new Vector2Int(x, y));

			flyingBuilding.transform.position = new Vector3(x, 0, y);
			flyingBuilding.SetTransparentColor(available);

			if (available && Input.GetMouseButtonDown(0))
			{
				PlaceFlyingBuilding(new Vector2Int(x, y));
			}
		}
	}


	private void RotateBuilding()
	{
		flyingBuilding.transform.Rotate(0, 90, 0);
		flyingBuilding.RecalculateProjection();

		int sizeXTemp = flyingBuilding.size.x;
		flyingBuilding.size.x = flyingBuilding.size.y;
		flyingBuilding.size.y = sizeXTemp;
	}

	private bool isPlaceTaken(Vector2Int pos)
	{
		for (int x = 0; x < flyingBuilding.size.x; x++)
			for (int y = 0; y < flyingBuilding.size.y; y++)
			{
				int dirX = flyingBuilding.projection.x;
				int dirY = flyingBuilding.projection.y;
				int offsetX = pos.x + x * dirX;
				int offsetY = pos.y + y * dirY;
				if (offsetX < 0 || offsetX > gridSize.x-1 || offsetY < 0 || offsetY > gridSize.y-1) return true;
				if (grid[offsetX, offsetY] != null) return true;
			}

		return false;
	}

	private bool canPlaceBuilding(Vector2Int pos)
	{
		if (flyingBuilding.projection.x > 0)
			if (pos.x <= 0 || pos.x > gridSize.x - flyingBuilding.size.x) return false;
		else
			if (pos.x <= 0 || pos.x > gridSize.x-1) return false;

		if (flyingBuilding.projection.y > 0) 
			if (pos.y <= 0 || pos.y > gridSize.y - flyingBuilding.size.y) return false;
		else
			if (pos.y <= 0 || pos.y > gridSize.y-1) return false;

		if (isPlaceTaken(pos)) return false;
		
		return true;
	}

	private void PlaceFlyingBuilding(Vector2Int pos)
	{
		for (int x = 0; x < flyingBuilding.size.x; x++)
			for (int y = 0; y < flyingBuilding.size.y; y++)
			{
				grid[pos.x + x*flyingBuilding.projection.x, pos.y + y * flyingBuilding.projection.y] = flyingBuilding;
			}

		flyingBuilding.SetNormalColor();
		flyingBuilding = null;
	}
}
