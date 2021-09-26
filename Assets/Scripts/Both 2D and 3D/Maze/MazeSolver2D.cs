using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSolver2D : MonoBehaviour
{
	public MazeSpawner2D mazeSpawner2D;
	public LineRenderer lr;
	public int maxPosCount = 10000;

	private Vector3[] SolveMaze()
	{
		Maze maze = mazeSpawner2D.maze;

		Vector2Int startPos = maze.startPos;
		int x = maze.finishPos.x;
		int y = maze.finishPos.y;
		List<Vector3> positions = new List<Vector3>();

		while ((x != 0 || y != 0) && positions.Count < maxPosCount)
		{
			positions.Add(new Vector3(x * mazeSpawner2D.CellSize.x, y * mazeSpawner2D.CellSize.y, y * mazeSpawner2D.CellSize.z));

			CellInfo currentCell = maze.cells[x, y];

			if (x > 0 &&
				!currentCell.leftWall &&
				maze.cells[x - 1, y].distanceFromStart == currentCell.distanceFromStart - 1)
			{
				x--;
			}
			else if (y > 0 &&
				!currentCell.bottomWall &&
				maze.cells[x, y - 1].distanceFromStart == currentCell.distanceFromStart - 1)
			{
				y--;
			}
			else if (x < maze.cells.GetLength(0) - 1 &&
				!maze.cells[x + 1, y].leftWall &&
				maze.cells[x + 1, y].distanceFromStart == currentCell.distanceFromStart - 1)
			{
				x++;
			}
			else if (y < maze.cells.GetLength(1) - 1 &&
				!maze.cells[x, y + 1].bottomWall &&
				maze.cells[x, y + 1].distanceFromStart == currentCell.distanceFromStart - 1)
			{
				y++;
			}
		}

		positions.Add((Vector2)startPos);
		return positions.ToArray();
	}

	public void DrawPath()
	{
		Vector3[] positions = SolveMaze();
		lr.positionCount = positions.Length;
		lr.SetPositions(positions);
	}
}
