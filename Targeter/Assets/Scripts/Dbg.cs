using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;

public class Dbg : MonoBehaviour
{

	static public DateTime now = DateTime.Now;
	static public string timeFormated = string.Format ("{0:MMM-dd-yyyy-HH-mm-ss-ff}", now);

	// Log file location for Unity Editor testing
	static public string dir = "/Users/Hamza/Library/Application Support/DefaultCompany/TTS/"; 
	// static public string dir = "/Users/Hamza/Library/Application Support/DefaultCompany/TTS/M-LAURINE/"; 
	//static public string dir = "/Users/Hamza/Library/Application Support/DefaultCompany/TTS/NM-BLAKE/"; 


	// Log file location for Android internal stroge
	//static public string dir = "/mnt/sdcard/TTS/Tumor01/"; 
	//static public string dir = "/mnt/sdcard/TTS/Tumor02/"; 
	//static public string dir = "/mnt/sdcard/TTS/Tumor03/"; 
	//static public string dir = "/mnt/sdcard/TTS/Tumor04/"; 
	//static public string dir = "/mnt/sdcard/TTS/Tumor05/"; 


	// public string dir = "var/mobile/Containers/Data/Application/5A6856FF-9C97-4B91-AB2C-F7087A760A1E/Documents/Log/"; // iPhone internal stroge

	static public string localLogFile = dir + timeFormated + ".csv";
	public bool EchoToConsole = true;
	public bool AddTimeStamp = true;
	private	StreamWriter	outputStream;
	static Dbg Singleton = null;

	public static Dbg Instance {
		get { return Singleton; }
	}

	void Start ()
	{
		try {
			if (!Directory.Exists (dir)) {
				Directory.CreateDirectory (dir);
				UnityEngine.Debug.Log (dir + " created !!");
			}
			if (Directory.Exists (dir)) {
				if (!File.Exists (localLogFile)) {
					File.Create (dir + localLogFile);
					UnityEngine.Debug.Log (localLogFile + " created !!");
				} 
			}
				
		} catch (Exception e) {
			UnityEngine.Debug.LogError (e.ToString ());
		}
	}

	//-------------------------------------------------------------------------------------------------------------------------
	void Awake ()
	{	
		if (Singleton != null) {
			UnityEngine.Debug.Log ("Multiple Dbg Singletons exist!");
			return;
		}

		Singleton = this;

#if !FINAL

		if (localLogFile != null) {
			outputStream = new System.IO.StreamWriter (localLogFile);
		}
#endif
	}

	//-------------------------------------------------------------------------------------------------------------------------
	void OnDestory ()
	{
#if !FINAL
		if (outputStream != null) {
			outputStream.Close ();
			outputStream = null;
		}
#endif
	}

	//-------------------------------------------------------------------------------------------------------------------------
	private void WriteWithTimeStamp (string message)
	{
#if !FINAL
		if (AddTimeStamp) {
			DateTime now = DateTime.Now;
			message = string.Format ("{0:MMM.dd.yyyy.HH.mm.ss.ffff} {1}", now, message);
		}

		if (outputStream != null) {
			outputStream.WriteLine (message);
			outputStream.Flush ();
		}
			
		if (EchoToConsole) {
			UnityEngine.Debug.Log (message);
		}
#endif
	}

	//-------------------------------------------------------------------------------------------------------------------------
	//[Conditional ("DEBUG"), Conditional ("PROFILE")]
	public static void TraceWithTimeStamp (string Message)
	{
#if !FINAL
		if (Dbg.Instance != null)
			Dbg.Instance.WriteWithTimeStamp (Message);
		else
			// Fallback if the debugging system hasn't been initialized yet.
			UnityEngine.Debug.Log ("Nothing written");
#endif
	}

	private void WriteWithOutTimeStamp (string message)
	{
		#if !FINAL
		if (outputStream != null) {
			outputStream.WriteLine (message);
			outputStream.Flush ();
		}

		if (EchoToConsole) {
			UnityEngine.Debug.Log (message);
		}
		#endif
	}

	public static void TraceWithOutTimeStampe (string Message)
	{
		#if !FINAL
		if (Dbg.Instance != null)
			Dbg.Instance.WriteWithOutTimeStamp (Message);
		else
			// Fallback if the debugging system hasn't been initialized yet.
			UnityEngine.Debug.Log ("Nothing written");
		#endif
	}
}