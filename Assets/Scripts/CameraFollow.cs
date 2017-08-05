using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public float smoothing = 3f;
	public int CameraNo = 0;
	GameObject cube;

	void Update()
	{
		CheckCameraInput();
	}

	void CheckCameraInput()
	{
		
		//Quaternion rotation = Quaternion.LookRotation(relativePos);
		//Quaternion current = transform.localRotation;

		//uaternion targetAng = Quaternion.Euler(0, 90, 0);
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			
			//transform.rotation = Quaternion.Euler(0f, 90f, 0f);
			//transform.rotation = Quaternion.Slerp(current, targetAng, Time.deltaTime*smoothing);
			transform.Rotate(0, 90, 0);
			CameraNo = (CameraNo + 5) % 4;


		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			transform.Rotate(0, -90, 0);
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0), Time.deltaTime);
			//transform.rotation = Quaternion.Euler(0, -90, 0);
			CameraNo = (CameraNo +3 ) % 4;

		}
	}

}
