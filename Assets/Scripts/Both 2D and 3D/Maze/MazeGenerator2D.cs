using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator2D
{
	public int width = 10, height = 10;
	private CellInfo[,] cells;

	public Maze GenerateMaze()
	{
		cells = new CellInfo[width, height];

		for (int x = 0; x < cells.GetLength(0); x++)
		{
			for (int y = 0; y < cells.GetLength(1); y++)
			{
				cells[x, y] = new CellInfo { posX = x, posY = y};
			}
		}

		// Remove extra walls
		for (int x = 0; x < width; x++)
		{
			cells[x, height - 1].leftWall = false;
			cells[x, height - 1].floor = false;
		}
		for (int y = 0; y < height; y++)
		{
			cells[width - 1, y].bottomWall = false;
			cells[width - 1, y].floor = false;
		}

		Maze maze = new Maze();

		// Make passes
		RemoveWalls();
		maze.cells = cells;
		maze.startPos = Vector2Int.zero;
		maze.finishPos = PlaceExit();
		
		return maze;
	}


	private void RemoveWalls()
	{
		CellInfo current = cells[0, 0];
		current.visited = true;
		current.distanceFromStart = 0;

		Stack<CellInfo> stack = new Stack<CellInfo>();

		do
		{
			List<CellInfo> unvisitedNeighbours = new List<CellInfo>();
			int x = current.posX;
			int y = current.posY;

			if (x > 0 && !cells[x-1, y].visited) unvisitedNeighbours.Add(cells[x-1, y]);
			if (y > 0 && !cells[x, y-1].visited) unvisitedNeighbours.Add(cells[x, y-1]);
			if (x < width-2 && !cells[x+1, y].visited) unvisitedNeighbours.Add(cells[x+1, y]);
			if (y < height-2 && !cells[x, y+1].visited) unvisitedNeighbours.Add(cells[x, y+1]);

			if (unvisitedNeighbours.Count > 0)
			{
				CellInfo chosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
				RemoveWall(current, chosen);

				chosen.distanceFromStart = current.distanceFromStart+1;
				chosen.visited = true;
				stack.Push(chosen);
				current = chosen;
			}
			else
			{
				current = stack.Pop();
			}
		}
		while (stack.Count > 0);
	}

	void RemoveWall(CellInfo current, CellInfo chosen)
	{
		if (current.posX == chosen.posX)
		{
			if (current.posY > chosen.posY) current.bottomWall = false;
			else chosen.bottomWall = false;
		}
		else
		{
			if (current.posX > chosen.posX) current.leftWall = false;
			else chosen.leftWall = false;
		}
	}

	Vector2Int PlaceExit()
	{
		CellInfo furthest = cells[0, 0];

		for (int x = 0; x < width; x++)
		{
			if (cells[x, height-2].distanceFromStart > furthest.distanceFromStart) furthest = cells[x, height-2];
			if (cells[x, 0].distanceFromStart > furthest.distanceFromStart) furthest = cells[x, 0];
		}

		for (int y = 0; y < height; y++)
		{
			if (cells[width-2, y].distanceFromStart > furthest.distanceFromStart) furthest = cells[width-2, y];
			if (cells[0, y].distanceFromStart > furthest.distanceFromStart) furthest = cells[0, y];
		}

		if (furthest.posX == 0) furthest.leftWall = false;
		else if (furthest.posY == 0) furthest.bottomWall = false;
		else if (furthest.posX == width-2) cells[furthest.posX+1, furthest.posY].leftWall = false;
		else if (furthest.posY == height-2) cells[furthest.posX, furthest.posY+1].bottomWall = false;

		return new Vector2Int(furthest.posX, furthest.posY);
	}
}
