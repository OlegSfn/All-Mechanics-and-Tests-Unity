using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoxelMapWFC : MonoBehaviour
{
    public List<VoxelTile> tilePrefabs;
    public Vector2Int mapSize = new Vector2Int(10, 10);
    private VoxelTile[,] spawnedTiles;

    private Queue<Vector2Int> needToRecalcPossibleTiles = new Queue<Vector2Int>();
    private List<VoxelTile>[,] possibleTiles;

    void Start()
    {
        spawnedTiles = new VoxelTile[mapSize.x, mapSize.y];
        foreach (VoxelTile tile in tilePrefabs)
        {
            tile.CalculateSidesColors();
        }

        int countBeforeAddingRotated = tilePrefabs.Count;
        for (int i = 0; i < countBeforeAddingRotated; i++)
        {
            VoxelTile clone;
            switch (tilePrefabs[i].rotation)
            {
                case VoxelTile.RotationType.OneRotation:
                    break;
                case VoxelTile.RotationType.TwoRotations:
                    tilePrefabs[i].weight /= 2;
                    if (tilePrefabs[i].weight == 0) tilePrefabs[i].weight = 1;

                    clone = Instantiate(tilePrefabs[i], tilePrefabs[i].transform.position + Vector3.right, Quaternion.identity);
                    clone.Rotate90();
                    tilePrefabs.Add(clone);
                    break;
                case VoxelTile.RotationType.FourRotations:
                    tilePrefabs[i].weight /= 4;
                    if (tilePrefabs[i].weight == 0) tilePrefabs[i].weight = 1;

                    clone = Instantiate(tilePrefabs[i], tilePrefabs[i].transform.position + Vector3.right, Quaternion.identity);
                    clone.Rotate90();
                    tilePrefabs.Add(clone);

                    clone = Instantiate(tilePrefabs[i], tilePrefabs[i].transform.position + Vector3.right * 2, Quaternion.identity);
                    clone.Rotate90();
                    clone.Rotate90();
                    tilePrefabs.Add(clone);

                    clone = Instantiate(tilePrefabs[i], tilePrefabs[i].transform.position + Vector3.right * 3, Quaternion.identity);
                    clone.Rotate90();
                    clone.Rotate90();
                    clone.Rotate90();
                    tilePrefabs.Add(clone);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }


        Generate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopAllCoroutines();

            foreach (VoxelTile tile in spawnedTiles)
            {
                if (tile != null) Destroy(tile.gameObject);
            }

            spawnedTiles = new VoxelTile[mapSize.x, mapSize.y];
            Generate();
        }
    }

	private void Generate()
	{
        int maxAttemps = 10;
        int attemp = 0;

        while(attemp++ < maxAttemps)
		{
            possibleTiles = new List<VoxelTile>[mapSize.x, mapSize.y];

            for (int x = 0; x < mapSize.x; x++)
                for (int y = 0; y < mapSize.y; y++)
                    possibleTiles[x, y] = new List<VoxelTile>(tilePrefabs);

            VoxelTile centerTile = GetRandomTile(tilePrefabs);
            possibleTiles[mapSize.x / 2, mapSize.y / 2] = new List<VoxelTile> { centerTile };

            needToRecalcPossibleTiles.Clear();
            EnqueueNeighboursToRecalc(new Vector2Int(mapSize.x / 2, mapSize.y / 2));

            bool isGeneratedCorrectly = GenerateAllPossibleTiles();
            if (isGeneratedCorrectly) break;
        }

        PlaceAllTiles();
    }

    private void EnqueueNeighboursToRecalc(Vector2Int pos)
	{
        needToRecalcPossibleTiles.Enqueue(new Vector2Int(pos.x+1, pos.y));
        needToRecalcPossibleTiles.Enqueue(new Vector2Int(pos.x-1, pos.y));
        needToRecalcPossibleTiles.Enqueue(new Vector2Int(pos.x, pos.y+1));
        needToRecalcPossibleTiles.Enqueue(new Vector2Int(pos.x, pos.y-1));
    }

    private bool GenerateAllPossibleTiles()
	{
        int maxIterations = mapSize.x*mapSize.y;
        int iterations = 0;
		while (iterations++ < maxIterations)
		{
            int maxInnerIterations = 500;
            int innerIterations = 0;
            while (needToRecalcPossibleTiles.Count > 0 && innerIterations++ < maxInnerIterations)
            {
                Vector2Int pos = needToRecalcPossibleTiles.Dequeue();
                if (pos.x == 0 || pos.y == 0 || pos.x == mapSize.x - 1 || pos.y == mapSize.y - 1) continue;
                List<VoxelTile> possibleTilesByPos = possibleTiles[pos.x, pos.y];

                int removedCount = possibleTilesByPos.RemoveAll(t => !isTilePossible(t, pos));

                if (removedCount > 0) EnqueueNeighboursToRecalc(pos);

                if (possibleTilesByPos.Count == 0)
				{
                    possibleTilesByPos.AddRange(tilePrefabs);
                    possibleTiles[pos.x+1, pos.y] = new List<VoxelTile>(tilePrefabs);
                    possibleTiles[pos.x-1, pos.y] = new List<VoxelTile>(tilePrefabs);
                    possibleTiles[pos.x, pos.y+1] = new List<VoxelTile>(tilePrefabs);
                    possibleTiles[pos.x, pos.y-1] = new List<VoxelTile>(tilePrefabs);

                    EnqueueNeighboursToRecalc(pos);
				}
            }
            if (innerIterations == maxInnerIterations) return false;

            List<VoxelTile> maxCountTiles = possibleTiles[1, 1];
            Vector2Int maxCountTilePosition = new Vector2Int(1, 1);
            for (int x = 1; x < mapSize.x - 1; x++)
                for (int y = 1; y < mapSize.y - 1; y++)
                {
                    if (possibleTiles[x, y].Count > maxCountTiles.Count)
                    {
                        maxCountTiles = possibleTiles[x, y];
                        maxCountTilePosition = new Vector2Int(x, y);
                    }
                }
            if (maxCountTiles.Count == 1) return true;

            VoxelTile tileToCollapse = GetRandomTile(maxCountTiles);
            possibleTiles[maxCountTilePosition.x, maxCountTilePosition.y] = new List<VoxelTile> { tileToCollapse };
            EnqueueNeighboursToRecalc(maxCountTilePosition);
        }
        Debug.Log("Error");
        return false;
    }

    private bool isTilePossible(VoxelTile tile, Vector2Int pos)
	{
        bool isAllForwardImpossible = possibleTiles[pos.x, pos.y+1].All(forwardTile => !canAppendTile(tile, forwardTile, Direction.Forward));
        if (isAllForwardImpossible) return false;

        bool isAllBackImpossible = possibleTiles[pos.x, pos.y-1].All(backTile => !canAppendTile(tile, backTile, Direction.Back));
        if (isAllBackImpossible) return false;

        bool isAllLeftImpossible = possibleTiles[pos.x-1, pos.y].All(rightTile => !canAppendTile(tile, rightTile, Direction.Left));
        if (isAllLeftImpossible) return false;

        bool isAllRightImpossible = possibleTiles[pos.x+1, pos.y].All(leftTile => !canAppendTile(tile, leftTile, Direction.Right));
        if (isAllRightImpossible) return false;

        return true;
    }

    private void PlaceAllTiles()
	{
        for (int x = 1; x < mapSize.x - 1; x++)
            for (int y = 1; y < mapSize.y - 1; y++)
                PlaceTile(x, y);
    }

    public void PlaceTile(int x, int y)
    {
        List<VoxelTile> availableTiles = possibleTiles[x, y];
        /*foreach (VoxelTile tilePrefab in tilePrefabs)
        {
            if (canAppendTile(spawnedTiles[x + 1, y], tilePrefab, Direction.Left) &&
                canAppendTile(spawnedTiles[x - 1, y], tilePrefab, Direction.Right) &&
                canAppendTile(spawnedTiles[x, y + 1], tilePrefab, Direction.Back) &&
                canAppendTile(spawnedTiles[x, y - 1], tilePrefab, Direction.Forward))
            {
                availableTiles.Add(tilePrefab);
            }
        }*/

        if (availableTiles.Count == 0) return;

        VoxelTile selectedTile = GetRandomTile(availableTiles);
        Vector3 pos = new Vector3(x, 0, y) * selectedTile.voxelSize * selectedTile.tileSize;
        spawnedTiles[x, y] = Instantiate(selectedTile, pos, selectedTile.transform.rotation);
    }

    private VoxelTile GetRandomTile(List<VoxelTile> availableTiles)
    {
        List<float> chances = new List<float>();
        for (int i = 0; i < availableTiles.Count; i++)
        {
            chances.Add(availableTiles[i].weight);
        }

        float value = Random.Range(0, chances.Sum());
        float sum = 0;

        for (int i = 0; i < chances.Count; i++)
        {
            sum += chances[i];
            if (value < sum)
            {
                return availableTiles[i];
            }
        }

        return availableTiles[availableTiles.Count - 1];
    }

    private bool canAppendTile(VoxelTile existingTile, VoxelTile tileToAppend, Direction direction)
    {
        if (existingTile == null) return true;

        if (direction == Direction.Back)
            return Enumerable.SequenceEqual(existingTile.colorsForward, tileToAppend.colorsBack);
        else if (direction == Direction.Forward)
            return Enumerable.SequenceEqual(existingTile.colorsBack, tileToAppend.colorsForward);
        else if (direction == Direction.Right)
            return Enumerable.SequenceEqual(existingTile.colorsLeft, tileToAppend.colorsRight);
        else if (direction == Direction.Left)
            return Enumerable.SequenceEqual(existingTile.colorsRight, tileToAppend.colorsLeft);
        else
            throw new System.ArgumentException("Wrong direction value, should be Vector.3.forward/back/left/right");

    }
}
