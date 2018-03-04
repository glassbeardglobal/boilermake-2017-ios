using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
	public Bounds bounds;
	// Use this for initialization
	void Start () {
		var collider = gameObject.GetComponent<BoxCollider> ();
		if (collider == null) {
			collider = gameObject.AddComponent<BoxCollider>();
			Debug.Log("No Collider is Detected");
		}
		bounds = collider.bounds;
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(bounds.center,Vector3.up,1);
	}
}
