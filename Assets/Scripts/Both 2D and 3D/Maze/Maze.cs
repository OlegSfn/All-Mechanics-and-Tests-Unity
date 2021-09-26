using UnityEngine;
public class CellInfo
{
	public int posX, posY;

	public bool leftWall = true, bottomWall = true, floor = true;

	public bool visited = false;
	public int distanceFromStart;
}


public class Maze
{
	public CellInfo[,] cells;

	public Vector2Int startPos;
	public Vector2Int finishPos;
}
