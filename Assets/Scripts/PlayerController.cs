using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class PlayerController : MonoBehaviour
{
	float fall = 0;
	public float fallSpeed = float.MaxValue;
	public GameObject SimulatedCube;

	public bool allowRotation=true;
	public bool limitRotation=false;

	private int lastTimeCameraNo = 0;


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		CheckUserInput();
	}


	public void CheckUserInput()
	{
		FindObjectOfType<GameController>().UpdateGrid(this);

		Quaternion resume= transform.rotation;

		transform.Rotate(0, 90 * (FindObjectOfType<CameraFollow>().CameraNo-lastTimeCameraNo), 0,Space.World);

		if (!CheckIsValidPosition())
			transform.rotation = resume;
		FindObjectOfType<GameController>().UpdateGrid(this);

		lastTimeCameraNo = FindObjectOfType<CameraFollow>().CameraNo;



		if (FindObjectOfType<CameraFollow>().CameraNo == 0)
		{
			

			if (Input.GetKeyDown(KeyCode.D))
			{
				transform.position += new Vector3(1, 0, 0);

				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(1, 0, 0);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}
			else if (Input.GetKeyDown(KeyCode.A))
			{
				transform.position += new Vector3(-1, 0, 0);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(-1, 0, 0);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			else if (Input.GetKeyDown(KeyCode.W))
			{
				transform.position += new Vector3(0, 0, 1);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, 1);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			else if (Input.GetKeyDown(KeyCode.S))
			{
				transform.position += new Vector3(0, 0, -1);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, -1);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if (allowRotation)
				{
					if (limitRotation)
					{
						if (transform.rotation.eulerAngles.x >= 90)
						{
							transform.Rotate(-90, 0, 0);
						}
						else
						{
							transform.Rotate(90, 0, 0);
						}
					}
					else
					{
						transform.Rotate(90, 0, 0);
					}
					if (!CheckIsValidPosition())
					{
						if (limitRotation)
						{
							if (transform.rotation.eulerAngles.x >= 90)
							{
								transform.Rotate(-90, 0, 0);
							}
							else
							{
								transform.Rotate(90, 0, 0);
							}
						}
						else
						{
							transform.Rotate(-90, 0, 0);
						}
					}
					else
					{
						FindObjectOfType<GameController>().UpdateGrid(this);
					}
				}
			}
			else if (Input.GetKeyDown(KeyCode.Return) || Time.time - fall >= fallSpeed)
			{

				//transform.position += new Vector3(0, -1, 0);
				//if (!CheckIsValidPosition())
				//{
				//	transform.position -= new Vector3(0, -1, 0);
				//	//FindObjectOfType<GameController>().DeleteRow();
				//	enabled = false;
				//	FindObjectOfType<GameController>().DropSpawn();
				//}
				//else
				//{
				//	FindObjectOfType<GameController>().UpdateGrid(this);
				//}
				//fall = Time.time;
				DownTheCube();
				FindObjectOfType<GameController>().UpdateGrid(this);
				enabled = false;
				FindObjectOfType<GameController>().DropSpawn();

			}
		} else if (FindObjectOfType<CameraFollow>().CameraNo == 1)
		{
			
			if (Input.GetKeyDown(KeyCode.W))
			{
				transform.position += new Vector3(1, 0, 0);

				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(1, 0, 0);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				transform.position += new Vector3(-1, 0, 0);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(-1, 0, 0);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			else if (Input.GetKeyDown(KeyCode.A))
			{
				transform.position += new Vector3(0, 0, 1);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, 1);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			else if (Input.GetKeyDown(KeyCode.D))
			{
				transform.position += new Vector3(0, 0, -1);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, -1);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if (allowRotation)
				{
					if (limitRotation)
					{
						if (transform.rotation.eulerAngles.x >= 90)
						{
							transform.Rotate(-90, 0, 0);
						}
						else
						{
							transform.Rotate(90, 0, 0);
						}
					}
					else
					{
						transform.Rotate(90, 0, 0);
					}
					if (!CheckIsValidPosition())
					{
						if (limitRotation)
						{
							if (transform.rotation.eulerAngles.x >= 90)
							{
								transform.Rotate(-90, 0, 0);
							}
							else
							{
								transform.Rotate(90, 0, 0);
							}
						}
						else
						{
							transform.Rotate(-90, 0, 0);
						}
					}
					else
					{
						FindObjectOfType<GameController>().UpdateGrid(this);
					}
				}
			}
			else if (Input.GetKeyDown(KeyCode.Return) || Time.time - fall >= fallSpeed)
			{

				//transform.position += new Vector3(0, -1, 0);
				//if (!CheckIsValidPosition())
				//{
				//	transform.position -= new Vector3(0, -1, 0);
				//	//FindObjectOfType<GameController>().DeleteRow();
				//	enabled = false;
				//	FindObjectOfType<GameController>().DropSpawn();
				//}
				//else
				//{
				//	FindObjectOfType<GameController>().UpdateGrid(this);
				//}
				//fall = Time.time;
				DownTheCube();
				FindObjectOfType<GameController>().UpdateGrid(this);
				enabled = false;
				FindObjectOfType<GameController>().DropSpawn();

			}
		} else if (FindObjectOfType<CameraFollow>().CameraNo == 2) {
			
			if (Input.GetKeyDown(KeyCode.A))
			{
				transform.position += new Vector3(1, 0, 0);

				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(1, 0, 0);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				transform.position += new Vector3(-1, 0, 0);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(-1, 0, 0);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			else if (Input.GetKeyDown(KeyCode.S))
			{
				transform.position += new Vector3(0, 0, 1);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, 1);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			else if (Input.GetKeyDown(KeyCode.W))
			{
				transform.position += new Vector3(0, 0, -1);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, -1);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if (allowRotation)
				{
					if (limitRotation)
					{
						if (transform.rotation.eulerAngles.x >= 90)
						{
							transform.Rotate(-90, 0, 0);
						}
						else
						{
							transform.Rotate(90, 0, 0);
						}
					}
					else
					{
						transform.Rotate(90, 0, 0);
					}
					if (!CheckIsValidPosition())
					{
						if (limitRotation)
						{
							if (transform.rotation.eulerAngles.x >= 90)
							{
								transform.Rotate(-90, 0, 0);
							}
							else
							{
								transform.Rotate(90, 0, 0);
							}
						}
						else
						{
							transform.Rotate(-90, 0, 0);
						}
					}
					else
					{
						FindObjectOfType<GameController>().UpdateGrid(this);
					}
				}
			}
			else if (Input.GetKeyDown(KeyCode.Return) || Time.time - fall >= fallSpeed)
			{

				//transform.position += new Vector3(0, -1, 0);
				//if (!CheckIsValidPosition())
				//{
				//	transform.position -= new Vector3(0, -1, 0);
				//	//FindObjectOfType<GameController>().DeleteRow();
				//	enabled = false;
				//	FindObjectOfType<GameController>().DropSpawn();
				//}
				//else
				//{
				//	FindObjectOfType<GameController>().UpdateGrid(this);
				//}
				//fall = Time.time;
				DownTheCube();
				FindObjectOfType<GameController>().UpdateGrid(this);
				enabled = false;
				FindObjectOfType<GameController>().DropSpawn();

			}
		} else if (FindObjectOfType<CameraFollow>().CameraNo == 3)
		{
			
			if (Input.GetKeyDown(KeyCode.S))
			{
				transform.position += new Vector3(1, 0, 0);

				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(1, 0, 0);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}
			else if (Input.GetKeyDown(KeyCode.W))
			{
				transform.position += new Vector3(-1, 0, 0);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(-1, 0, 0);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			else if (Input.GetKeyDown(KeyCode.D))
			{
				transform.position += new Vector3(0, 0, 1);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, 1);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			else if (Input.GetKeyDown(KeyCode.A))
			{
				transform.position += new Vector3(0, 0, -1);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, -1);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if (allowRotation)
				{
					if (limitRotation)
					{
						if (transform.rotation.eulerAngles.x >= 90)
						{
							transform.Rotate(-90, 0, 0);
						}
						else
						{
							transform.Rotate(90, 0, 0);
						}
					}
					else
					{
						transform.Rotate(90, 0, 0);
					}
					if (!CheckIsValidPosition())
					{
						if (limitRotation)
						{
							if (transform.rotation.eulerAngles.x >= 90)
							{
								transform.Rotate(-90, 0, 0);
							}
							else
							{
								transform.Rotate(90, 0, 0);
							}
						}
						else
						{
							transform.Rotate(-90, 0, 0);
						}
					}
					else
					{
						FindObjectOfType<GameController>().UpdateGrid(this);
					}
				}
			}
			else if (Input.GetKeyDown(KeyCode.Return) || Time.time - fall >= fallSpeed)
			{

				//transform.position += new Vector3(0, -1, 0);
				//if (!CheckIsValidPosition())
				//{
				//	transform.position -= new Vector3(0, -1, 0);
				//	//FindObjectOfType<GameController>().DeleteRow();
				//	enabled = false;
				//	FindObjectOfType<GameController>().DropSpawn();
				//}
				//else
				//{
				//	FindObjectOfType<GameController>().UpdateGrid(this);
				//}
				//fall = Time.time;
				DownTheCube();
				FindObjectOfType<GameController>().UpdateGrid(this);
				enabled = false;
				FindObjectOfType<GameController>().DropSpawn();

			}
		}
		UpdateSimulatedCube();
	}

	void DownTheCube()
	{
		transform.position-= new Vector3(0, FindObjectOfType<GameController>().GetDistanceToButtom(this), 0);
	}

	void UpdateSimulatedCube()
	{
		foreach (Transform cube in transform)
		{
			Vector3 pos = FindObjectOfType<GameController>().Round(cube.position)-new Vector3(0,FindObjectOfType<GameController>().GetDistanceToButtom(this),0);
			GameObject Simu= Instantiate(SimulatedCube, pos, cube.rotation);
			Destroy(Simu, 0.02f);
		}

	}

	bool CheckIsValidPosition ()
	{
		foreach (Transform cube in transform )
		{
			Vector3 pos = FindObjectOfType<GameController>().Round(cube.position);

			if (FindObjectOfType<GameController>().CheckIsInsideGrid(pos)==false)
			{
				return false;
			}

			if (FindObjectOfType<GameController>().GetTransformAtGridPosition(pos) !=null &&FindObjectOfType<GameController>().GetTransformAtGridPosition(pos).parent !=transform)
			{
				return false;
			}
		}

		return true;
	}


}
