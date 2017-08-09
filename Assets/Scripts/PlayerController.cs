using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;




public class PlayerController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler,IPointerEnterHandler
{
	public CameraFollow cameraFollow;
	public bool canEdit=false;
	public Material ToBe;
	public float cameraMoveSpeed = 1f;
	public bool touched = false;
	[SerializeField]
	private Vector2 OriginalPos, MovingPos, MovingOriPos,EndPos;
	private Renderer Target;
	private Material AsIs;
	public bool cubeRotate, cubeLeft, cubeRight, cubeUp, cubeDown, cubeDrop = false;



	float fall = 0;
	public float fallSpeed = float.MaxValue;
	public GameObject SimulatedCube;
	
	public bool allowRotation=true;
	public bool limitRotation=false;
	public bool cubeactive = true;

	private int lastTimeCameraNo = 0;
	private Vector3 DropPosition;


	// Use this for initialization
	private void Awake()
	{
		DropPosition = transform.position;
		touched = false;

		foreach (Transform cube in transform)
		{
			AsIs = cube.GetComponent<Renderer>().material;
		}

		canEdit = touched = false;
		OriginalPos = MovingPos = EndPos = Vector2.zero;
	}
	
	void Update()
	{
		CheckUserInput();
	}

	public void OnPointerEnter(PointerEventData data)
	{
		cameraFollow.cubeTouch = true;
	}

	public void OnPointerDown(PointerEventData data)
	{
		if (!touched)
		{
			OriginalPos = data.position;
			touched = true;
			canEdit = true;
		}
	}

	public void OnDrag(PointerEventData data)
	{
		if (canEdit)
		{
			foreach (Transform cube in transform)
			{
				cube.GetComponent<Renderer>().material = ToBe;
			}
		}

		MovingPos = MovingOriPos = data.position;
		

		if ((MovingPos.x - OriginalPos.x) < -10 && (MovingPos.y - OriginalPos.y) < -20)
		{
			cubeDown = true;
			OriginalPos = data.position;
			canEdit = false;
			touched = false;
		}
		else if ((MovingPos.x - OriginalPos.x) > 10 && (MovingPos.y - OriginalPos.y) > 20)
		{
			cubeUp = true;
			OriginalPos = data.position;
			canEdit = false;
			touched = false;
		}
		else if ((MovingPos.x - OriginalPos.x) < -20 && (MovingPos.y - OriginalPos.y) > 10)
		{
			cubeLeft = true;
			OriginalPos = data.position;
			canEdit = false;
			touched = false;
		}
		else if ((MovingPos.x - OriginalPos.x) > 20 && (MovingPos.y - OriginalPos.y) < -10)
		{
			cubeRight = true;
			OriginalPos = data.position;
			canEdit = false;
			touched = false;
		}

	}

	public void OnPointerUp(PointerEventData data)
	{
		foreach (Transform cube in transform)
		{
			cube.GetComponent<Renderer>().material = AsIs;
		}
		if (touched&&canEdit)
		{
			EndPos = data.position;
			touched = false;
			cubeRotate = true;
		}
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
			

			if (Input.GetKeyDown(KeyCode.D)||cubeRight)
			{
				cubeRight = false;
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
			else if (Input.GetKeyDown(KeyCode.A)||cubeLeft)
			{
				cubeLeft = false;
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

			else if (Input.GetKeyDown(KeyCode.W)||cubeUp)
			{
				cubeUp = false;
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

			else if (Input.GetKeyDown(KeyCode.S)||cubeDown)
			{
				cubeDown = false;
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

			else if (Input.GetKeyDown(KeyCode.Space)||cubeRotate)
			{
				cubeRotate = false;
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
			else if (Input.GetKeyDown(KeyCode.Return) || Time.time - fall >= fallSpeed||cubeDrop)
			{
				cubeDrop = false;
				DownTheCube();
				FindObjectOfType<GameController>().UpdateGrid(this);
				enabled = false;
				FindObjectOfType<GameController>().DropSpawn();

			}
		} else if (FindObjectOfType<CameraFollow>().CameraNo == 1)
		{
			
			if (Input.GetKeyDown(KeyCode.W)||cubeUp)
			{
				cubeUp = false;
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
			else if (Input.GetKeyDown(KeyCode.S)||cubeDown)
			{
				cubeDown = false;
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

			else if (Input.GetKeyDown(KeyCode.A)||cubeLeft)
			{
				cubeLeft = false;
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

			else if (Input.GetKeyDown(KeyCode.D)||cubeRight)
			{
				cubeRight = false;
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

			else if (Input.GetKeyDown(KeyCode.Space)||cubeRotate)
			{
				cubeRotate = false;
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
			else if (Input.GetKeyDown(KeyCode.Return) || Time.time - fall >= fallSpeed||cubeDrop)
			{
				cubeDrop = false;

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
			
			if (Input.GetKeyDown(KeyCode.A)||cubeLeft)
			{
				cubeLeft = false;
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
			else if (Input.GetKeyDown(KeyCode.D)||cubeRight)
			{
				cubeRight = false;
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

			else if (Input.GetKeyDown(KeyCode.S)||cubeDown)
			{
				cubeDown = false;
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

			else if (Input.GetKeyDown(KeyCode.W)||cubeUp)
			{
				cubeUp = false;
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

			else if (Input.GetKeyDown(KeyCode.Space)||cubeRotate)
			{
				cubeRotate = false;
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
			else if (Input.GetKeyDown(KeyCode.Return) || Time.time - fall >= fallSpeed||cubeDrop)
			{
				cubeDrop = false;
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
			
			if (Input.GetKeyDown(KeyCode.S)||cubeDown)
			{
				cubeDown = false;
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
			else if (Input.GetKeyDown(KeyCode.W)||cubeUp)
			{
				cubeUp = false;
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

			else if (Input.GetKeyDown(KeyCode.D)||cubeRight)
			{
				cubeRight = false;
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

			else if (Input.GetKeyDown(KeyCode.A)||cubeLeft)
			{
				cubeLeft = false;
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

			else if (Input.GetKeyDown(KeyCode.Space)||cubeRotate)
			{
				cubeRotate=false;
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
			else if (Input.GetKeyDown(KeyCode.Return) || Time.time - fall >= fallSpeed||cubeDrop)
			{
				cubeDrop = false;
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
		//transform.position = Vector3.Lerp(transform.position, DropPosition, Time.deltaTime * 1f);
		DropPosition = transform.position - new Vector3(0, FindObjectOfType<GameController>().GetDistanceToButtom(this), 0);
		transform.position = DropPosition;
	}

	public void DropButton()
	{
		DropPosition = transform.position - new Vector3(0, FindObjectOfType<GameController>().GetDistanceToButtom(this), 0);
		//while (transform.position.y - DropPosition.y > 0.1)
		//{

		//}
		transform.position = DropPosition;
		FindObjectOfType<GameController>().UpdateGrid(this);
		enabled = false;
		cubeDrop = false;
		FindObjectOfType<GameController>().DropSpawn();
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
