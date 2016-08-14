using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;

public class ObjectDragger : MonoBehaviour
{
	
	public Vector3 screenPoint;
	private string timeStamp;
	private string currentEvent;

	Ray ray;
	RaycastHit hit;
	public float defaultDistance = 10;

	UserInterface guiObj;
	Rigidbody rb;

	GameObject endscopyGameObject;

	Vector3 endscopyHeadZeroVector;
	Vector3 endscopyHeadTempVector;

	void Start ()
	{
		GameObject cam = GameObject.Find ("Camera");
		guiObj = cam.GetComponent<UserInterface> ();

		//	endscopyHeadZeroVector = new Vector3 (-4.965079e-07, 1.01, 7.644427e-13);

		// endscopyGameObject = GameObject.FindGameObjectWithTag ("Endscopy_Head");
		//	endscopyHeadTempVector = endscopyGameObject.transform.position;

	}

	void Update ()
	{
		if (gameObject.tag == "Endscopy_Head") {
			if (gameObject.transform.position != Vector3.zero) {
				currentEvent = " ; " + guiObj.userID +
				" ; " + SystemInfo.deviceModel +
				" ; Default Statue " +
				" ; " + gameObject.name +
				" ; " + gameObject.transform.position.x / 100 +
				" ; " + gameObject.transform.position.y / 100 +
				" ; " + gameObject.transform.position.z / 100 +
				" ; " + gameObject.transform.rotation.x +
				" ; " + gameObject.transform.rotation.y +
				" ; " + gameObject.transform.rotation.z;	
				Dbg.TraceWithTimeStamp (currentEvent);
			}	
		}
	}


	void OnMouseEnter ()
	{
		currentEvent = " ; " + guiObj.userID +
		" ; " + SystemInfo.deviceModel +
		" ; User Touched" +
		" ; " + gameObject.name +
		" ; " + gameObject.transform.position.x / 100 +
		" ; " + gameObject.transform.position.y / 100 +
		" ; " + gameObject.transform.position.z / 100 +
		" ; " + gameObject.transform.rotation.x +
		" ; " + gameObject.transform.rotation.y +
		" ; " + gameObject.transform.rotation.z;	
		
//		guiObj.CharaterField("User Touched " + gameObject.name);
		Dbg.TraceWithTimeStamp (currentEvent);
	}



	void OnMouseDown ()
	{
		try {
			screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
			Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);

			currentEvent = " ; " + guiObj.userID +
			" ; " + SystemInfo.deviceModel +
			" ; User Selected" +
			" ; " + gameObject.name +
			" ; " + gameObject.transform.position.x / 100 +
			" ; " + gameObject.transform.position.y / 100 +
			" ; " + gameObject.transform.position.z / 100 +
			" ; " + gameObject.transform.rotation.x +
			" ; " + gameObject.transform.rotation.y +
			" ; " + gameObject.transform.rotation.z;	
			
//			guiObj.CharaterField("User Selected " + gameObject.name);
			Dbg.TraceWithTimeStamp (currentEvent);
		} catch (Exception e) {
			UnityEngine.Debug.LogError (e);
		}
	}

	void OnMouseDrag ()
	{
		try {
			Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);
			transform.position = currentPos;

			currentEvent = " ; " + guiObj.userID +
			" ; " + SystemInfo.deviceModel +
			" ; User Dragged" +
			" ; " + gameObject.name +
			" ; " + gameObject.transform.position.x / 100 +
			" ; " + gameObject.transform.position.y / 100 +
			" ; " + gameObject.transform.position.z / 100 +
			" ; " + gameObject.transform.rotation.x +
			" ; " + gameObject.transform.rotation.y +
			" ; " + gameObject.transform.rotation.z;	

//				guiObj.CharaterField("User Dragged " + hit.collider.name);
			Dbg.TraceWithTimeStamp (currentEvent);
		} catch (Exception e) {
			UnityEngine.Debug.Log (e);
		}
	}
}
