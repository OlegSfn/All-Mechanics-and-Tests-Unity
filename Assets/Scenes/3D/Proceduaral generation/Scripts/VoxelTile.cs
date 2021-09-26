using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VoxelTile : MonoBehaviour
{

	public float voxelSize = 0.1f;
	public int tileSize = 8;

	[Range(0,100)]
	public int weight = 50;

	public RotationType rotation;
	public enum RotationType
	{
		OneRotation,
		TwoRotations,
		FourRotations
	}

	[HideInInspector] public byte[] colorsForward;
	[HideInInspector] public byte[] colorsBack;
	[HideInInspector] public byte[] colorsLeft;
	[HideInInspector] public byte[] colorsRight;

	public void CalculateSidesColors()
	{
		colorsForward = new byte[tileSize * tileSize];
		colorsBack = new byte[tileSize * tileSize];
		colorsLeft = new byte[tileSize * tileSize];
		colorsRight = new byte[tileSize * tileSize];

		for (int y = 0; y < tileSize; y++)
		{
			for (int x = 0; x < tileSize; x++)
			{
				colorsForward[y*tileSize + x] = GetVoxelColor(x, y, Direction.Forward);
				colorsBack[y*tileSize + x] = GetVoxelColor(x, y, Direction.Back);
				colorsLeft[y*tileSize + x] = GetVoxelColor(x, y, Direction.Left);
				colorsRight[y*tileSize + x] = GetVoxelColor(x, y, Direction.Right);
			}
		}
	}

	public void Rotate90()
	{
		transform.Rotate(0, 90, 0);

		byte[] colorsForwardNew = new byte[tileSize*tileSize];
		byte[] colorsBackNew = new byte[tileSize*tileSize];
		byte[] colorsLeftNew = new byte[tileSize*tileSize];
		byte[] colorsRightNew = new byte[tileSize*tileSize];

		for (int y = 0; y < tileSize; y++)
		{
			for (int x = 0; x < tileSize; x++)
			{
				colorsRightNew[y * tileSize + x] = colorsForward[y * tileSize + x];
				colorsForwardNew[y * tileSize + x] = colorsLeft[y * tileSize + tileSize - 1 - x];
				colorsLeftNew[y * tileSize + x] = colorsBack[y * tileSize + tileSize-1 - x];
				colorsBackNew[y * tileSize + x] = colorsRight[y * tileSize + x];
			}
		}

		colorsForward = colorsForwardNew;
		colorsBack = colorsBackNew;
		colorsLeft = colorsLeftNew;
		colorsRight = colorsRightNew;
	}

	private byte GetVoxelColor(int x, int y, Direction dir)
	{
		MeshCollider meshCollider = GetComponentInChildren<MeshCollider>();

		float vox = voxelSize;
		float half = voxelSize/2;

		Vector3 rayStart;
		Vector3 rayDir;
		if (dir == Direction.Forward)
		{
			rayStart = meshCollider.bounds.min +
				new Vector3((half + x*vox), 0, -half);
			rayDir = Vector3.forward;
		}
		else if (dir == Direction.Back)
		{
			rayStart = meshCollider.bounds.max +
				new Vector3(-half - (tileSize-1 - x) * vox, 0, half);
			rayDir = Vector3.back;
		}
		else if (dir == Direction.Left)
		{
			rayStart = meshCollider.bounds.max +
				new Vector3(half, 0, -half - (tileSize-1 - x) * vox);
			rayDir = Vector3.left;
		}
		else if (dir == Direction.Right)
		{
			rayStart = meshCollider.bounds.min +
				new Vector3(-half, 0, half + x*vox);
			rayDir = Vector3.right;
		}
		else
		{
			throw new System.ArgumentException("Wrong direction value, should be Direction.Forward/Back/Left/Right");
		}
		rayStart.y = meshCollider.bounds.min.y+half + y*vox;

		if (Physics.Raycast(new Ray(rayStart, rayDir*0.1f), out RaycastHit hit, voxelSize))
		{
			byte colorIndex = (byte)(hit.textureCoord.x * 256);
			if (colorIndex == 0) Debug.LogWarning("There is color 0 in mesh palette, this can cause conflicts");

			return (byte)(hit.textureCoord.x * 256);
		}

		return 0;
	}
}
