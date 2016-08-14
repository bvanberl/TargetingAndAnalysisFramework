using UnityEngine;
using System.Collections;

public class GetCenterPoint : MonoBehaviour {

	public Renderer rend;
	void Start() {
		rend = GetComponent<Renderer>();
	}

	void Update()
	{
		Vector3 center = rend.bounds.center;
		float radius = rend.bounds.extents.magnitude;
		UnityEngine.Debug.Log ("Game Object name ... "+gameObject.name + " and center point is ..." + center + "Radius is .." + radius);
	}
}
