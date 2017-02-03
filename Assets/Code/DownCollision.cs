using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {

		Rigidbody body = collision.gameObject.GetComponent<Rigidbody> ();

		if (body != null) {

			float force_x = Random.Range (-35.0f,35.0f);
			float force_y = Random.Range (80.0f,185.0f);
			float force_z = Random.Range (-35.0f,35.0f);
			body.AddForce (force_x, force_y, force_z);
			//Debug.Log ("Choca");
		}

	}
}
