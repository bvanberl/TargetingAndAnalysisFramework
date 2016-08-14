using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ScenarioSim.Core.Entities;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine.UI;
using ScenarioAsset = ScenarioSim.Core.Entities.ScenarioAsset;
using Newtonsoft.Json.Converters;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public const int noTargets = 50; // Total number of targets a subject must hit per trial
	public int targetsHit = 0; // Keeps track of how many targets have been hit
	public GameObject sphere; // The Game Object that is targeted
	public Canvas startCanvas; // The canvas with the "start game" button
	//public Canvas newPerformerCanvas; // The canvas for creating new performers
	//public Canvas keyboardCanvas; // The canvas for displaying the keyboard
	public StringBuilder csv_string; // For building CSV data
	public System.DateTime prevDT; // Time at which the last correct targeting took place
	public float timeDiff; // Difference in seconds between placement of a sphere and correct targeting
	public float dValue; // The 'w' in the rotational analog for Fitts' Law
	public float QValue; // The 'A' in the rotational analog for Fitts' Law
	public Vector3 origReticleDirection; // The direction vector of the ray indicating the original position of the reticle
	public float ID; // Index of difficulty of the rotational task
	public GvrReticle reticle;
	public ScenarioPerformance performance;
	public Performer performer;
	public Text performerName;


	// Use this for initialization
	void Start () {
		// Change colour of sphere
		sphere.GetComponent<Renderer>().material.color = Color.yellow;

		// Set up string builder for data recording
		csv_string = new StringBuilder();
		csv_string.AppendLine ("Timestamp,W-value,X-value,Y-value,Z-value,Index of Difficulty,Time To Target");

		prevDT = System.DateTime.Now; // Get current datetime 

		performerName.text = Controller.userData.firstName + " " + Controller.userData.lastName;


	}


	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(reticle.transform.position);
		Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);
	}


	public void newEllipsoid()
	{
		if (targetsHit > 0) {
			Ray ray = Camera.main.ScreenPointToRay (reticle.transform.position);
			UnityEngine.Quaternion q = UnityEngine.Quaternion.LookRotation (ray.direction); // Get direction of head rotation represented as a quaternion

			// Get time difference (in seconds) between positioning of sphere and correct targeting
			//System.DateTime currDT = System.DateTime.Now;
			//timeDiff = (float)(currDT - prevDT).TotalSeconds;
			//prevDT = currDT;

			// Get direction of sphere from camera
			Vector3 sphereDirection = Camera.main.ScreenPointToRay (sphere.GetComponent<SphereCollider>().center).direction;
			float radius = sphere.GetComponent<SphereCollider> ().radius;

			// Calculate Q-value (in radians)
			QValue = Mathf.Abs((Mathf.PI)/180*Vector3.Angle(origReticleDirection, sphereDirection));

			// Calculate d-value (in radians)
			dValue = Mathf.Abs((Mathf.PI)/180*Vector3.Angle(
				Camera.main.ScreenPointToRay(sphere.GetComponent<SphereCollider>().center + new Vector3(radius*sphere.transform.localScale.x, 0f, 0f)).direction,
				Camera.main.ScreenPointToRay(sphere.GetComponent<SphereCollider>().center + new Vector3(-radius*sphere.transform.localScale.x, 0f, 0f)).direction));

			// Calculate Index of Difficulty (in bits)
			ID = Mathf.Log (QValue / dValue + 1, 2);

			// Add data from this trial to the CSV string
			csv_string.AppendLine (System.DateTime.Now.ToShortDateString () + " " + System.DateTime.Now.ToLongTimeString () +
				"." + System.DateTime.Now.Millisecond + "," + q.w + "," + q.x + "," + q.y + "," + q.z + "," + ID + "," + timeDiff);

			// Add new event to the performance object
			performance.Events.Add(generateEvent(q, ID));

			performerName.text = performer.Name + " - " + targetsHit;
		}

		// Remember where the reticle was originally located at the start of a new trial.
		origReticleDirection = Camera.main.ScreenPointToRay (reticle.transform.position).direction;

		// Randomly position sphere
		Vector3 pos = sphere.transform.position;
		UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
		pos.x = (float)UnityEngine.Random.Range (100, 400);
		pos.y = (float)UnityEngine.Random.Range (70, 130);
		pos.z = (float)UnityEngine.Random.Range (100, 200);
		sphere.transform.position = pos;

		// Randomly choose scale for sphere.  Ensure that 2 dimensions are the same to give appearance of ellipsoid.
		float newRadius = (float)UnityEngine.Random.Range(10, 20);
		sphere.transform.localScale = new Vector3(newRadius, newRadius, newRadius);

		// Randomly rotate newly formed ellipsoid
		//sphere.transform.rotation = Random.rotation;

		++targetsHit; // User hit another target

		// Send data if total number of targets have been hit
		if (targetsHit > noTargets) {
			performance.EndTime = DateTimeOffset.Now;
			targetsHit = 0;
			sendData ();
			SceneManager.LoadScene ("LoginScene");
		}
	}


	// Opens an email containing the data collected in the trial in the body as CSV format
	public void sendData()
	{
		//string toSend = WWW.EscapeURL (csv_string.ToString ());
		//Application.OpenURL ("mailto:bvanberl@uwo.ca?subject=data&body=" + toSend);
		performance.EndTime = DateTime.Now;

		string url = "http://scenariosim.azurewebsites.net/api/performance";
		HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
		request.ContentType = "application/json";
		request.Method = "POST";

		using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
		{
			string json = JsonConvert.SerializeObject(performance, GetSerializerSettings());
			Debug.Log (json);
			writer.Write(json);
			writer.Flush();
			writer.Close();
		}
			
	}


	// Start the clock and put in the first ellipsoid
	public void startClock()
	{
		prevDT = System.DateTime.Now; // Get current datetime 
		newEllipsoid ();
		startCanvas.enabled = false;

		// Generate scenario for the task
		Scenario scenario = new Scenario ();
		scenario.Name = "TargetSphere";
		scenario.SchemaId = Guid.NewGuid();

		// Create a performance object for this trial
		performance = new ScenarioPerformance {
			Id = Guid.NewGuid (),
			PerformerId = new Guid(Controller.userData.id),
			Scenario = scenario,
			TaskPerformances = new Dictionary<Guid, TaskPerformance> (),
			Events = new List<ScenarioSim.Core.Entities.Event> (),
			StartTime = DateTimeOffset.Now
		};

	}


	// Generate an event/UserAction for a successful sphere targeting
	public ScenarioSim.Core.Entities.Event generateEvent(UnityEngine.Quaternion q, float ID){
		DateTime currDT = DateTime.Now;

		ScenarioSim.Core.Entities.Event e = new ScenarioSim.Core.Entities.Event ();
		e.Name = "Target1Sphere";
		e.Description = "Targeted a sphere";
		e.Timestamp = currDT;

		ScenarioSim.Core.Entities.Quaternion qu = new ScenarioSim.Core.Entities.Quaternion (q.x, q.x, q.y, q.z);
		EventParameter param1 = new EventParameter ();
		param1.Name = "headRotation";
		param1.Value = qu;
		e.AddParameter ("headRotation", param1);

		EventParameter param2 = new EventParameter ();
		param2.Name = "indexOfDifficulty";
		param2.Value = ID;
		e.AddParameter ("indexOfDifficulty", param2);

		EventParameter param3 = new EventParameter ();
		param3.Name = "timeToTarget";
		param3.Value = (float)(currDT - prevDT).TotalSeconds;
		e.AddParameter ("timeToTarget", param3);
		prevDT = currDT;

		return e;
	}


	private JsonSerializerSettings GetSerializerSettings()
	{
		return new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Objects,
			ContractResolver = new WriteablePropertiesOnlyResolver()
		};
	}


	public class WriteablePropertiesOnlyResolver : DefaultContractResolver
	{
		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
			return props.Where(p => p.Writable).ToList();
		}
	}

	/*
	public void createNewPerformer()
	{
		startCanvas.enabled = false;
		newPerformerCanvas.enabled = true;
		keyboardCanvas.enabled = true;
	}

	// Creates new Performer object based off of entered name and posts it to API
	public void submitNewPerformer()
	{
		newPerformerCanvas.enabled = false;
		keyboardCanvas.enabled = false;
		startCanvas.enabled = true;

		string newName = inputButton.GetComponentInChildren<Text> ().text;
		Performer p = new Performer {
			Id = Guid.NewGuid (),
			Name = newName
		};

		string url = "http://scenariosim.azurewebsites.net/api/performer";
		HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
		request.ContentType = "application/json";
		request.Method = "POST";

		using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
		{
			string json = JsonConvert.SerializeObject(performer, GetSerializerSettings());
			Debug.Log (json);
			writer.Write(json);
			writer.Flush();
			writer.Close();
		}
	}


	// Return to main menu without creating new performer
	public void cancelNewPerformer()
	{
		newPerformerCanvas.enabled = false;
		keyboardCanvas.enabled = false;
		startCanvas.enabled = true;
	}


	public void selectPerformer()
	{
	}
*/
}
