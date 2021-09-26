using UnityEngine;


public class MazeSpawner2D : MonoBehaviour
{
	public GameObject CellPrefab;
	public Vector3 CellSize = new Vector3(1,1,0);
	public int widthOfMaze, heightOfMaze;

	public Maze maze = new Maze();
	public MazeSolver2D mazeSolver2D;

	private void Awake()
	{
		MazeGenerator2D mazeGenerator = new MazeGenerator2D() { width = widthOfMaze, height = heightOfMaze};
		maze = mazeGenerator.GenerateMaze();

		InstantiateMaze();
		mazeSolver2D.DrawPath();
	}


	void InstantiateMaze()
	{
		for (int x = 0; x < maze.cells.GetLength(0); x++)
		{
			for (int y = 0; y < maze.cells.GetLength(1); y++)
			{
				Cell c = Instantiate(CellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity).GetComponent<Cell>();

				c.leftWall.SetActive(maze.cells[x, y].leftWall);
				c.bottomWall.SetActive(maze.cells[x, y].bottomWall);
				if (c.floor != null) c.floor.SetActive(maze.cells[x, y].floor);
			}
		}
	}


	/*private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			foreach (GameObject cell in Cells)
			{
				Destroy(cell);
			}

			MazeGenerator2D mazeGenerator = new MazeGenerator2D() { width = widthOfMaze, height = heightOfMaze };
			maze = mazeGenerator.GenerateMaze();
			InstantiateMaze();
		}
	}*/
}
