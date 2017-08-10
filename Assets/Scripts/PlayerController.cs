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
	public Vector3 DropPosition;


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
		DownTheCube();
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

		if ((MovingPos.x - OriginalPos.x) < -20 && (MovingPos.y - OriginalPos.y) > -10 && (MovingPos.y - OriginalPos.y) < 0)
		{
			cubeDown = true;
			cubeLeft = true;
			canEdit = false;
			touched = false;
			OriginalPos = data.position;
		}

		if ((MovingPos.x - OriginalPos.x) > -10 && (MovingPos.x - OriginalPos.x) < 0 && (MovingPos.y - OriginalPos.y) > 20)
		{
			cubeUp = true;
			cubeLeft = true;
			canEdit = false;
			touched = false;
			OriginalPos = data.position;
		}

		if ((MovingPos.x - OriginalPos.x) > 20 && (MovingPos.y - OriginalPos.y) < 10 && (MovingPos.y - OriginalPos.y) > 0)
		{
			cubeUp = true;
			cubeRight = true;
			canEdit = false;
			touched = false;
			OriginalPos = data.position;
		}

		if ((MovingPos.x - OriginalPos.x) < 10 && (MovingPos.x - OriginalPos.x) > 0 && (MovingPos.y - OriginalPos.y) < -20)
		{
			cubeDown = true;
			cubeRight = true;
			canEdit = false;
			touched = false;
			OriginalPos = data.position;
		}

		if ((MovingPos.x - OriginalPos.x) < -10 && (MovingPos.y - OriginalPos.y) < -20)
		{
			cubeDown = true;
			canEdit = false;
			touched = false;
			Debug.Log(string.Format("Down/CamerNo: {0}",cameraFollow.CameraNo));
			OriginalPos = data.position;
		}
		if ((MovingPos.x - OriginalPos.x) > 10 && (MovingPos.y - OriginalPos.y) > 20)
		{
			cubeUp = true;
			canEdit = false;
			touched = false;
			Debug.Log(string.Format("Up/CamerNo: {0}", cameraFollow.CameraNo));
			OriginalPos = data.position;
		}
		if ((MovingPos.x - OriginalPos.x) < -20 && (MovingPos.y - OriginalPos.y) > 10)
		{
			cubeLeft = true;
			canEdit = false;
			touched = false;
			Debug.Log(string.Format("Left/CamerNo: {0}", cameraFollow.CameraNo));
			OriginalPos = data.position;
		}
		if ((MovingPos.x - OriginalPos.x) > 20 && (MovingPos.y - OriginalPos.y) < -10)
		{
			cubeRight = true;
			canEdit = false;
			touched = false;
			Debug.Log(string.Format("Right/CamerNo: {0}", cameraFollow.CameraNo));
			OriginalPos = data.position;
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
				DropPosition = Round(transform.position);

				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(1, 0, 0);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}
			if (Input.GetKeyDown(KeyCode.A)||cubeLeft)
			{
				cubeLeft = false;
				transform.position += new Vector3(-1, 0, 0);
				DropPosition = Round(transform.position);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(-1, 0, 0);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			if (Input.GetKeyDown(KeyCode.W)||cubeUp)
			{
				cubeUp = false;
				transform.position += new Vector3(0, 0, 1);
				DropPosition = Round(transform.position);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, 1);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			if (Input.GetKeyDown(KeyCode.S)||cubeDown)
			{
				cubeDown = false;
				transform.position += new Vector3(0, 0, -1);
				DropPosition = Round(transform.position);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, -1);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			if (Input.GetKeyDown(KeyCode.Space)||cubeRotate)
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
			if (Input.GetKeyDown(KeyCode.Return) || Time.time - fall >= fallSpeed||cubeDrop)
			{
				DropButton();

			}
		} else if (FindObjectOfType<CameraFollow>().CameraNo == 1)
		{
			
			if (Input.GetKeyDown(KeyCode.W)||cubeUp)
			{
				cubeUp = false;
				transform.position += new Vector3(1, 0, 0);
				DropPosition = Round(transform.position);

				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(1, 0, 0);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}
			if (Input.GetKeyDown(KeyCode.S)||cubeDown)
			{
				cubeDown = false;
				transform.position += new Vector3(-1, 0, 0);
				DropPosition = Round(transform.position);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(-1, 0, 0);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			if (Input.GetKeyDown(KeyCode.A)||cubeLeft)
			{
				cubeLeft = false;
				transform.position += new Vector3(0, 0, 1);
				DropPosition = Round(transform.position);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, 1);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			if (Input.GetKeyDown(KeyCode.D)||cubeRight)
			{
				cubeRight = false;
				transform.position += new Vector3(0, 0, -1);
				DropPosition = Round(transform.position);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, -1);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			if (Input.GetKeyDown(KeyCode.Space)||cubeRotate)
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
			if (Input.GetKeyDown(KeyCode.Return) || Time.time - fall >= fallSpeed||cubeDrop)
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
				DropButton();

			}
		} else if (FindObjectOfType<CameraFollow>().CameraNo == 2) {
			
			if (Input.GetKeyDown(KeyCode.A)||cubeLeft)
			{
				cubeLeft = false;
				transform.position += new Vector3(1, 0, 0);
				DropPosition = Round(transform.position);

				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(1, 0, 0);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}
			if (Input.GetKeyDown(KeyCode.D)||cubeRight)
			{
				cubeRight = false;
				transform.position += new Vector3(-1, 0, 0);
				DropPosition = Round(transform.position);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(-1, 0, 0);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			if (Input.GetKeyDown(KeyCode.S)||cubeDown)
			{
				cubeDown = false;
				transform.position += new Vector3(0, 0, 1);
				DropPosition = Round(transform.position);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, 1);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			if (Input.GetKeyDown(KeyCode.W)||cubeUp)
			{
				cubeUp = false;
				transform.position += new Vector3(0, 0, -1);
				DropPosition = Round(transform.position);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, -1);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			if (Input.GetKeyDown(KeyCode.Space)||cubeRotate)
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
			if (Input.GetKeyDown(KeyCode.Return) || Time.time - fall >= fallSpeed||cubeDrop)
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
				DropButton();

			}
		} else if (FindObjectOfType<CameraFollow>().CameraNo == 3)
		{
			
			if (Input.GetKeyDown(KeyCode.S)||cubeDown)
			{
				cubeDown = false;
				transform.position += new Vector3(1, 0, 0);
				DropPosition = Round(transform.position);

				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(1, 0, 0);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}
			if (Input.GetKeyDown(KeyCode.W)||cubeUp)
			{
				cubeUp = false;
				transform.position += new Vector3(-1, 0, 0);
				DropPosition = Round(transform.position);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(-1, 0, 0);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			if (Input.GetKeyDown(KeyCode.D)||cubeRight)
			{
				cubeRight = false;
				transform.position += new Vector3(0, 0, 1);
				DropPosition = Round(transform.position);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, 1);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			if (Input.GetKeyDown(KeyCode.A)||cubeLeft)
			{
				cubeLeft = false;
				transform.position += new Vector3(0, 0, -1);
				DropPosition = Round(transform.position);
				if (!CheckIsValidPosition())
				{
					transform.position -= new Vector3(0, 0, -1);
					DropPosition = Round(transform.position);
				}
				else
				{
					FindObjectOfType<GameController>().UpdateGrid(this);
				}
			}

			if (Input.GetKeyDown(KeyCode.Space)||cubeRotate)
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
			if (Input.GetKeyDown(KeyCode.Return) || Time.time - fall >= fallSpeed||cubeDrop)
			{
				DropButton();

			}
		}
		UpdateSimulatedCube();
	}
	public Vector3 Round(Vector3 pos)
	{
		return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
	}

	private void DownTheCube()
	{
		if (cubeDrop)
		{
			transform.position = Vector3.Lerp(transform.position, DropPosition, Time.deltaTime * 3f);
			if (((transform.position.y - DropPosition.y) < 0.05f) && cubeDrop)
			{
				transform.position = Round(DropPosition);

				FindObjectOfType<GameController>().UpdateGrid(this);
				enabled = false;
				cubeDrop = false;
				FindObjectOfType<GameController>().DropSpawn();
			}
		}
	

		////transform.position = Vector3.Lerp(transform.position, DropPosition, Time.deltaTime * 1f);
		//DropPosition = transform.position - new Vector3(0, FindObjectOfType<GameController>().GetDistanceToButtom(this), 0);
		//transform.position = DropPosition;
	}

	public void DropButton()
	{
		DropPosition= transform.position - new Vector3(0, FindObjectOfType<GameController>().GetDistanceToButtom(this), 0);
		cubeDrop = true;
		//transform.position = DropPosition;
		DownTheCube();
		
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
