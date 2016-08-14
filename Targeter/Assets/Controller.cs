using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ScenarioSim.Core.Entities;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {

	public Button loginButton, newPerformerButton, submitButton;
	public Canvas loadCanvas, loginCanvas, newPerformerCanvas;
	public InputField emailIF, passwordIF, confirmPasswordIF, FNameIF, LNameIF, usernameInput, passwordInput;
	public static UserData userData;

	// Use this for initialization
	void Start () {
		loginCanvas.enabled = false;
		newPerformerCanvas.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onLoginButtonTouched()
	{
		loadCanvas.enabled = false;
		loginCanvas.enabled = true;
	}

	public void login()
	{
		Debug.Log("Logging in.");
		string url = "http://scenariosim.azurewebsites.net/token";
		string postData = string.Format("grant_type=password&username={0}&password={1}", usernameInput.text, passwordInput.text);
		byte[] data = Encoding.ASCII.GetBytes(postData);

		HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
		request.ContentType = "application/x-www-form-urlencoded";
		request.Method = "POST";
		request.ContentLength = data.Length;

		using (Stream stream = request.GetRequestStream())
		{
			stream.Write(data, 0, data.Length);
		}

		Debug.Log("Sending Request.");

		HttpWebResponse response = (HttpWebResponse)request.GetResponse();

		string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

		//userData = JsonConvert.DeserializeObject<UserData>(responseString);
		userData = UserData.CreateFromJSON(responseString);

		switchScenes (userData);
	}

	public void onNewPerformerButtonTouched()
	{
		loadCanvas.enabled = false;
		newPerformerCanvas.enabled = true;
	}

	public void submitNewPerformer()
	{
		if((passwordIF.text == confirmPasswordIF.text) && (passwordIF.text.Length >= 6) && (passwordIF.text.Any(c => char.IsUpper(c)))
			&& (passwordIF.text.Any(c => char.IsDigit(c))) && !(passwordIF.text.All(c => char.IsLetterOrDigit(c))))
		{
			
			string url = "http://scenariosim.azurewebsites.net/api/account/register";
			HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
			request.ContentType = "application/json";
			request.Method = "POST";

			using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
			{
				string json = "{" + 
					@"""Email"":""" + emailIF.text + @"""," +
					@"""Password"":""" + passwordIF.text + @"""," +
					@"""ConfirmPassword"":""" + confirmPasswordIF.text + @"""," +
					@"""FirstName"":""" + FNameIF.text + @"""," +
					@"""LastName"":""" + LNameIF.text + @"""" +
					"}";
				Debug.Log (json);
				writer.Write(json);
				writer.Flush();
				writer.Close();

				newPerformerCanvas.enabled = false;
				loadCanvas.enabled = true;
			}
		}

	}

	public void switchScenes(UserData UD)
	{
		DontDestroyOnLoad (transform.gameObject);
		SceneManager.LoadScene ("TargeterScene");
	}

}




[Serializable]
public class UserData
{
	public string access_token;
	public string id;
	public string userName;
	public string firstName;
	public string lastName;

	public static UserData CreateFromJSON(string json)
	{
		return JsonUtility.FromJson<UserData> (json);
	}
}
