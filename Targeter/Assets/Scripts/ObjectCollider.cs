using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;

public class ObjectCollider : MonoBehaviour
{
	Vector3 screenPoint;
	string timeStamp;
	string currentEvent;
	float currentRayDistance;
	float defaultDistance = 100;
	string distanceFormated;
	float gameObjectWidth;


	RaycastHit hitFirst;
	Vector3 up;
	Vector3 pos;
	Vector3 extents;
	string caustiveObjectEvent;
	string receptiveObjectEvent;

	Vector3 contactPoint;
	Vector3 currentDir;
	float incidenceAngle;

	UserInterface guiObj;
	Stopwatch stopWatch = new Stopwatch ();
	public bool targetHit;

	void Start ()
	{
		GameObject cam = GameObject.Find ("Camera");
		guiObj = cam.GetComponent<UserInterface> ();
		up = transform.TransformDirection (Vector3.up);
		stopWatch.Start ();
		targetHit = false;
	}

	void Update ()
	{
		if (Physics.Raycast (transform.position, up * defaultDistance, out hitFirst)) {
			//UnityEngine.Debug.DrawRay (transform.position, up * defaultDistance, Color.green);

			currentRayDistance = hitFirst.distance;
			distanceFormated = String.Format ("{0:F2}", currentRayDistance);

			//if (hit.collider.tag == "Targets") {
			//contactPoint = collisionInfo.contacts [0].point - transform.localPosition; 
			currentDir = gameObject.transform.forward;
			incidenceAngle = Vector3.Angle (currentDir, hitFirst.normal);
			
			caustiveObjectEvent = gameObject.name +
			" ; " + gameObject.transform.position.x / 100 +
			" ; " + gameObject.transform.position.y / 100 +
			" ; " + gameObject.transform.position.z / 100 +
			" ; " + gameObject.transform.rotation.x +
			" ; " + gameObject.transform.rotation.y +
			" ; " + gameObject.transform.rotation.z +
			" ; " + distanceFormated;
			
			receptiveObjectEvent = hitFirst.collider.name +
			" ; " + hitFirst.collider.transform.position.x / 100 +
			" ; " + hitFirst.collider.transform.position.y / 100 +
			" ; " + hitFirst.collider.transform.position.z / 100 +
			" ; " + hitFirst.collider.transform.rotation.x +
			" ; " + hitFirst.collider.transform.rotation.y +
			" ; " + hitFirst.collider.transform.rotation.z +
			" ; " + distanceFormated +
			" ; " + incidenceAngle +
			" ; " + hitFirst.collider.bounds.extents.x * -1;

			currentEvent = " ; " + guiObj.userID + " ; " + SystemInfo.deviceModel + " ; Objects At Single Line ; " + caustiveObjectEvent + " ; " + receptiveObjectEvent;
			//	guiObj.CharaterField(gameObject.name + "at single with line " + hit.collider.name);
			Dbg.TraceWithTimeStamp (currentEvent);
		}
	}


	void OnCollisionEnter (Collision collisionInfo)
//	void OnTriggerEnter(Collision  collisionInfo)
	{
		try {
			Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);

			//	if (gameObject.name == "Endscopy_Head" && collisionInfo.collider.tag == "Targets") {
			if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.up) * defaultDistance, out hitFirst)) {
				currentRayDistance = hitFirst.distance;

				distanceFormated = String.Format ("{0:F2}", currentRayDistance);

				if (collisionInfo.collider.tag == "Targets") {
					currentDir = gameObject.transform.forward;
					incidenceAngle = Vector3.Angle (currentDir, hitFirst.normal);

					caustiveObjectEvent = gameObject.name +
					" ; " + gameObject.transform.position.x / 100 +
					" ; " + gameObject.transform.position.y / 100 +
					" ; " + gameObject.transform.position.z / 100 +
					" ; " + gameObject.transform.rotation.x +
					" ; " + gameObject.transform.rotation.y +
					" ; " + gameObject.transform.rotation.z +
					" ; " + distanceFormated;

					receptiveObjectEvent = collisionInfo.collider.name +
					" ; " + collisionInfo.collider.transform.position.x / 100 +
					" ; " + collisionInfo.collider.transform.position.y / 100 +
					" ; " + collisionInfo.collider.transform.position.z / 100 +
					" ; " + collisionInfo.collider.transform.rotation.x +
					" ; " + collisionInfo.collider.transform.rotation.y +
					" ; " + collisionInfo.collider.transform.rotation.z +
					" ; " + distanceFormated +
					" ; " + incidenceAngle +
					" ; " + collisionInfo.collider.transform.localScale.x;

					stopWatch.Stop ();
					// Get the elapsed time as a TimeSpan value.
					TimeSpan ts = stopWatch.Elapsed;

					// Format and display the TimeSpan value.
					string elapsedTime = String.Format ("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

					currentEvent = " ; " + guiObj.userID + " ; " + SystemInfo.deviceModel + " ; Interaction Detected ; " + caustiveObjectEvent + " ; " + receptiveObjectEvent + " ; " + elapsedTime;
					Dbg.TraceWithTimeStamp (currentEvent);
					Destroy (collisionInfo.gameObject);
					guiObj.state = collisionInfo.gameObject.name + " was hitted successfaully !";

					// EditorUtility.DisplayDialog ("Message", "The " + collisionInfo.gameObject.name + " has been targeted", "Okay");
					//	guiObj.currentStatueLable = "Interaction detected between "+ gameObject.name   + "and " + collisionInfo.collider.name ;
				}
			}
		} catch (Exception e) {
			UnityEngine.Debug.LogError (e);
		}
	}


	//	void OnCollisionStay (Collision collisionInfo)
	////	void OnTriggerStay(Collision  collisionInfo)
	//	{
	//		try {
	//			Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
	//			Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);
	//
	//			if (collisionInfo.collider.tag == "Targets") {
	//				currentDir = gameObject.transform.forward;
	//				incidenceAngle = Vector3.Angle (currentDir, hit.normal);
	//
	//				caustiveObjectEvent = gameObject.name +
	//				" ; " + gameObject.transform.localPosition.x +
	//				" ; " + gameObject.transform.localPosition.y +
	//				" ; " + gameObject.transform.localPosition.z +
	//				" ; " + gameObject.transform.localRotation.x +
	//				" ; " + gameObject.transform.localRotation.y +
	//				" ; " + gameObject.transform.localRotation.z +
	//				" ; " + distanceFormated;
	//
	//				receptiveObjectEvent = collisionInfo.collider.name +
	//				" ; " + collisionInfo.collider.transform.localPosition.x +
	//				" ; " + collisionInfo.collider.transform.localPosition.y +
	//				" ; " + collisionInfo.collider.transform.localPosition.z +
	//				" ; " + collisionInfo.collider.transform.localRotation.x +
	//				" ; " + collisionInfo.collider.transform.localRotation.y +
	//				" ; " + collisionInfo.collider.transform.localRotation.z +
	//				" ; " + distanceFormated +
	//				" ; " + incidenceAngle;
	//
	//				currentEvent = " ; " + guiObj.userID + " ; " + SystemInfo.deviceModel + " ; Interaction Active ; " + caustiveObjectEvent + " ; " + receptiveObjectEvent + " ; ";
	//				// guiObj.currentStatueLable = "Interaction active between "+ gameObject.name + "and " + collisionInfo.collider.name;
	//				Dbg.TraceWithTimeStamp (currentEvent);
	//			}
	//		} catch (Exception e) {
	//			UnityEngine.Debug.LogError (e);
	//		}
	//	}
}
