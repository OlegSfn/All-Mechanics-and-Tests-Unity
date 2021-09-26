using UnityEngine;
using UnityEngine.SceneManagement;
public class GridTerritoryGame : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Vector2Int gridSize = new Vector2Int(15,10);
	[SerializeField] private Vector2Int rectSize = new Vector2Int(2, 1);

	[Header("Camera")]
	[SerializeField] private Camera cam;

	[Header("Prefabs")]
	[SerializeField] private GameObject tilePrefab;
	[SerializeField] private Rectangle startRect;

	[Header("Transforms")]
	[SerializeField] private Transform canvas;
	[SerializeField] private Transform gridParent;
	[SerializeField] private Transform player1Rects, player2Rects;
	
	// Variables
	private bool player1Turn = true;
	private Rectangle[,] grid;
	private Rectangle chosenRect;

	private void Awake()
	{
		grid = new Rectangle[gridSize.x, gridSize.y];
		CreateMap();
	}

	private void Update()
	{
		if (chosenRect != null)
			StartPlacingRectangle();

		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		if (Input.GetKeyDown(KeyCode.S))
			CreateRectangle(startRect, rectSize);
	}
	
	private void CreateMap()
	{
		for (int y = 0; y < gridSize.y; y++)
			for (int x = 0; x < gridSize.x; x++)
			{
				Transform tile = Instantiate(tilePrefab, new Vector3(x+0.5f, y+0.5f, 3), Quaternion.identity, gridParent).transform;

				// Remove extra walls
				if (x == gridSize.x - 1)
					tile.GetChild(1).gameObject.SetActive(false);
				if (y == gridSize.y - 1)
					tile.GetChild(0).gameObject.SetActive(false);
			}
	}

	// Create and Placing rectangle
	public void CreateRectangle(Rectangle rectangle, Vector2Int size)
	{
		if (chosenRect != null) Destroy(chosenRect.gameObject);
		chosenRect = Instantiate(rectangle, canvas);
		chosenRect.SetSize(size);
	}
	private void StartPlacingRectangle()
	{
		Vector2 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
		int x = Mathf.RoundToInt(worldPos.x);
		int y = Mathf.RoundToInt(worldPos.y);

		MoveChosenRect(x, y);
		if (Input.GetMouseButtonDown(1)) RotateRectangle();

		bool available = canPlaceRectangle(new Vector2Int(x, y));
		chosenRect.SetTransparentColor(available);
		if (available && Input.GetMouseButtonDown(0))
		{
			PlaceChosenRectangle(new Vector2Int(x, y));
		}
		

	}
	private void PlaceChosenRectangle(Vector2Int pos)
	{
		for (int x = 0; x < chosenRect.size.x; x++)
			for (int y = 0; y < chosenRect.size.y; y++)
				grid[pos.x + x, pos.y + y] = chosenRect;

		if (player1Turn)
			chosenRect.SetParent(player1Rects);
		else
			chosenRect.SetParent(player2Rects);
		player1Turn = !player1Turn;

		chosenRect.SetNormalColor();
		chosenRect = null;
	}

	// Moving rectangle
	private void MoveChosenRect(int x, int y)
	{
		Vector2 pos = new Vector2(x + chosenRect.GetOffsetValue().x, y + chosenRect.GetOffsetValue().y);
		chosenRect.transform.position = pos;
	}
	private void RotateRectangle()
	{
		int sizeXTemp = chosenRect.size.x;
		chosenRect.size.x = chosenRect.size.y;
		chosenRect.size.y = sizeXTemp;
		chosenRect.FitScaleToSize();

	}

	// Check for availibility
	private bool canPlaceRectangle(Vector2Int pos)
	{
		if (pos.x <= 0 || pos.x > gridSize.x-chosenRect.size.x) return false;
		else
		if (pos.x <= 0 || pos.x > gridSize.x-1) return false;

		if (pos.y <= 0 || pos.y > gridSize.y-chosenRect.size.y) return false;
		else
		if (pos.y <= 0 || pos.y > gridSize.y-1) return false;

		if (isPlaceTaken(pos)) return false;

		return true;
	}
	private bool isPlaceTaken(Vector2Int pos)
	{
		for (int x = 0; x < chosenRect.size.x; x++)
			for (int y = 0; y < chosenRect.size.y; y++)
			{
				int offsetX = pos.x + x;
				int offsetY = pos.y + y;
				if (offsetX < 0 || offsetX > gridSize.x - 1
					|| offsetY < 0 || offsetY > gridSize.y - 1) return true;
				if (grid[offsetX, offsetY] != null) return true;
			}

		return false;
	}
}
