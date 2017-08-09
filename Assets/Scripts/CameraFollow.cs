using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public float smoothing = 3f;
	public int CameraNo = 0;
	public GroundSliderTouchController touchController;
	public GameObject test;
	public Transform CameraFocusInGround;
	public Transform CameraFoucsInGlobal;
	public bool cubeTouch = false;

	private Vector3 CameraPosition;


	private void Awake()
	{
		CameraPosition = transform.position;
	}

	void Update()
	{
		CheckCameraInput();
		ChangePosition();
	}


	private void ChangePosition()
	{
		if (cubeTouch)
			CameraPosition = CameraFoucsInGlobal.position;

		if (!cubeTouch)
			CameraPosition = CameraFocusInGround.position;

		test.transform.position = Vector3.Lerp(test.transform.position, CameraPosition, Time.deltaTime*2);
	}

	void CheckCameraInput()
	{

		//Quaternion rotation = Quaternion.LookRotation(relativePos);
		//Quaternion current = transform.localRotation;

		//uaternion targetAng = Quaternion.Euler(0, 90, 0);
		if (Input.GetKeyDown(KeyCode.LeftArrow) || touchController.swipeRight == true)
		{
			touchController.swipeLeft = touchController.swipeRight = false;
			//transform.rotation = Quaternion.Euler(0f, 90f, 0f);
			//transform.rotation = Quaternion.Slerp(current, targetAng, Time.deltaTime*smoothing);
			transform.Rotate(0, 90, 0);
			CameraNo = (CameraNo + 5) % 4;


		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) || touchController.swipeLeft == true)
		{
			touchController.swipeLeft = touchController.swipeRight = false;
			transform.Rotate(0, -90, 0);
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0), Time.deltaTime);
			//transform.rotation = Quaternion.Euler(0, -90, 0);
			CameraNo = (CameraNo +3 ) % 4;

		}
	}

}
