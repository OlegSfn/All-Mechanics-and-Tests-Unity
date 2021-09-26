using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoxelMapGenerator : MonoBehaviour
{
    public List<VoxelTile> tilePrefabs;
    public Vector2Int mapSize = new Vector2Int(10, 10);
    private VoxelTile[,] spawnedTiles;

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

                    clone = Instantiate(tilePrefabs[i], tilePrefabs[i].transform.position + Vector3.right*2, Quaternion.identity);
                    clone.Rotate90();
                    clone.Rotate90();
                    tilePrefabs.Add(clone);

                    clone = Instantiate(tilePrefabs[i], tilePrefabs[i].transform.position + Vector3.right*3, Quaternion.identity);
                    clone.Rotate90();
                    clone.Rotate90();
                    clone.Rotate90();
                    tilePrefabs.Add(clone);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
		}
        

        StartCoroutine(Generate());
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
            StartCoroutine(Generate());
		}
	}

	public IEnumerator Generate()
	{
		for (int x = 1; x < mapSize.x-1; x++)
		{
			for (int y = 1; y < mapSize.y-1; y++)
			{

                yield return new WaitForSeconds(0.1f);
                PlaceTile(x, y);
            }
		}
	}

    public void PlaceTile(int x, int y)
    {
        List<VoxelTile> availableTiles = new List<VoxelTile>();
		foreach (VoxelTile tilePrefab in tilePrefabs)
		{
            if(canAppendTile(spawnedTiles[x+1,y], tilePrefab, Direction.Left) &&
                canAppendTile(spawnedTiles[x-1,y], tilePrefab, Direction.Right) &&
                canAppendTile(spawnedTiles[x,y+1], tilePrefab, Direction.Back) &&
                canAppendTile(spawnedTiles[x,y-1], tilePrefab, Direction.Forward))
			{
                availableTiles.Add(tilePrefab);
			}
		}

        if (availableTiles.Count == 0) return;

        VoxelTile selectedTile = GetRandomTile(availableTiles);
        Vector3 pos = new Vector3(x, 0, y) * selectedTile.voxelSize * selectedTile.tileSize;
        spawnedTiles[x,y] = Instantiate(selectedTile, pos, selectedTile.transform.rotation);
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
