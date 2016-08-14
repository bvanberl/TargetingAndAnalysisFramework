using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
	//public string currentStatueLable = "Session started ... ";
	public string userID = "USER ID";
	public string userDirectionToPatient = "North";
	public string userHand = "Right";

	public string state = "Session is active .. ";
	GUIStyle localStyle;

	private Rect windowRect = new Rect (10, 15, 150, 105);

	void Start ()
	{
		Dbg.TraceWithOutTimeStampe ("Session Started at ;" + DateTime.Now + ";");
		Dbg.TraceWithOutTimeStampe ("User ID ;" + userID + ";");
		Dbg.TraceWithOutTimeStampe ("User Direction to the Patient ;" + userDirectionToPatient + ";");
		Dbg.TraceWithOutTimeStampe ("User Hand ;" + userHand + ";");
		Dbg.TraceWithOutTimeStampe ("Device Name ;" + SystemInfo.deviceName + ";");
		Dbg.TraceWithOutTimeStampe ("Device Model ;" + SystemInfo.deviceModel + ";");
		Dbg.TraceWithOutTimeStampe ("Device Type ;" + SystemInfo.deviceType + ";");
		Dbg.TraceWithOutTimeStampe ("Device Unique Identifier ;" + SystemInfo.deviceUniqueIdentifier + ";");
		Dbg.TraceWithOutTimeStampe ("Operting System ;" + SystemInfo.operatingSystem + ";");
		Dbg.TraceWithOutTimeStampe ("Processor Type ;" + SystemInfo.processorType + ";");

		Dbg.TraceWithOutTimeStampe ("TimeStamp ; User ID ; System Model ; Event Type ; " +
			"Caustive Object ; Caustive Object Position X ; Caustive Object Position Y ; Caustive Object Position Z ; Caustive Object Rotation X ; Caustive Object Rotation Y ; Caustive Object Rotation Z ; Distance ; " +
			"Receptive Object Name ; Receptive Object Position X ; Receptive Object Position Y ; Receptive Object Position Z ; Receptive Object Rotation X ; Receptive Object Rotation Y ; Receptive Object Rotation Z ; Distance ; Incidence Angle ; Receptive Object Width ; Sesion Elapsed Time ;");

	}

	void OnApplicationQuit ()
	{
		Dbg.TraceWithTimeStamp ("; Session ended ;");
		Application.Quit ();
	}

	void OnGUI ()
	{
		windowRect = GUI.Window (0, windowRect, WindowFunction, "Session State");
	}

	void WindowFunction (int windowID)
	{
		// Draw any Controls inside the window here
		localStyle = new GUIStyle (GUI.skin.label);

		localStyle.normal.textColor = Color.yellow;
		localStyle.fontSize = 12;
		localStyle.alignment = TextAnchor.UpperLeft;
		localStyle.fontStyle = FontStyle.Normal;
		//
		//        GUI.Label(new Rect (0,0,200,200), "User Id: "+userID, localStyle);
		//        GUI.Label(new Rect (0,15,200,200), "User Direction To Patient: "+userDirectionToPatient, localStyle);
		//        GUI.Label(new Rect (0,30,200,200), "User Hand Used To Patient: "+userHand, localStyle);
		//        GUI.Label(new Rect (0,45,1000,200), "Session State: "+state, localStyle);
		GUI.Label (new Rect (10, 30, 180, 150), state, localStyle);
	}
}