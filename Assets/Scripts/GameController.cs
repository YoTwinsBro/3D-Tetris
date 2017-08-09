using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static int grid_X = 7;
	public static int grid_Y = 40;
	public static int grid_Z = 7;
	public static Transform[,,] grid = new Transform[grid_X,grid_Y,grid_Z];

	public CameraFollow cameraFollow;
	public GameObject[] cubes;
	public Transform[] dropPoints;
	public Transform[] cubeAngles;
	public GameObject cube_Clone;

	// Use this for initialization
	void Start () {
		DropSpawn();
	}
	
	public bool IsFullRowAt(int y)
	{
		for (int x =0;x<grid_X;++x)
		{
			for (int z = 0;z<grid_Z;z++)
			{
				if (grid[x,y,z]==null)
				{
					return false;
				}
			}
		}
		return true;
	}

	public void DeleteTetris(int y)
	{
		for (int x = 0;x<grid_X;++x)
		{
			for (int z =0;z<grid_Z;++z)
			{
				Destroy(grid[x, y, z].gameObject);
				grid[x, y, z] = null;
			}
		}
	}

	public void MoveRowDown(int y)
	{
		for (int x = 0;x<grid_X;++x)
		{
			for (int z =0;z<grid_Z;++z)
			{
				if (grid[x,y,z]!=null)
				{
					grid[x, y - 1, z] = grid[x, y, z];
					grid[x, y, z] = null;
					grid[x, y - 1, z].position += new Vector3(0, -1, 0);
				}
			}
		}
	}

	public void	MoveAllPlaneDown (int y)
	{
		for (int i =y;i<grid_Y;++i)
		{
			MoveRowDown(i);
		}
	}

	public void DeleteRow()
	{
		for (int y =0;y<grid_Y;++y)
		{
			if (IsFullRowAt(y))
			{
				DeleteTetris(y);
				MoveAllPlaneDown(y + 1);
				--y;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	 public int GetDistanceToButtom(PlayerController Tetris)
	{
		int Distance = grid_Y;

		foreach (Transform cube in Tetris.transform)
		{
			
			Vector3 pos = Round(cube.position);
			int cubeDistance = (int)pos.y;
			for (int posy = 0;posy<cube.position.y-1;++posy)
			{
				if (grid [(int)pos.x,posy,(int)pos.z]!=null&& grid[(int)pos.x, posy, (int)pos.z].parent.CompareTag("Cube")&& grid[(int)pos.x, posy, (int)pos.z].parent!=cube.parent)
				{
					cubeDistance = (int)pos.y - posy-1;
				}
			}
			if (cubeDistance < Distance)
				Distance = cubeDistance;
		}

		return Distance;
	}




	public void	UpdateGrid(PlayerController Tetris)
	{
		for (int y=0;y<grid_Y;++y)
		{
			for (int x = 0;x<grid_X;++x)
			{
				for (int z = 0;z<grid_Z;++z)
				{
					if (grid[x,y,z] != null)
					{
						if (grid[x,y,z].parent==Tetris.transform)
						{
							grid[x, y, z] = null;
						}
					}
				}
			}
		}
		foreach (Transform cube in Tetris.transform)
		{
			Vector3 pos = Round(cube.position);
			if (pos.y<grid_Y)
			{
				grid[(int)pos.x, (int)pos.y, (int)pos.z] = cube;
			}
		}
	}

	public Transform GetTransformAtGridPosition (Vector3 pos)
	{
		if (pos.y > grid_Y-1)
		{
			return null;
		} else
		{
			return grid[(int)pos.x, (int)pos.y, (int)pos.z];
		}
	}

	public	bool CheckIsInsideGrid (Vector3 pos)
	{
		return ((int)pos.x >= 0 && (int)pos.x < grid_X && (int)pos.z >= 0 && (int)pos.z < grid_Z && (int)pos.y >= 0);
	}

	public Vector3 Round(Vector3 pos)
	{
		return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
	}

	public void DropSpawn()
	{
		int spawnPointIndex = Random.Range(0, dropPoints.Length);
		int spawnAngleIndex = Random.Range(0, cubeAngles.Length);
		GameObject Activecube = cubes[Random.Range(0, cubes.Length)];

		cube_Clone= Instantiate(Activecube, dropPoints[spawnPointIndex].position, cubeAngles[spawnAngleIndex].rotation);
		cube_Clone.transform.GetComponent<PlayerController>().cameraFollow = cameraFollow;
	}

	public void DropButton()
	{
		cube_Clone.transform.GetComponent<PlayerController>().DropButton();
	}
}
