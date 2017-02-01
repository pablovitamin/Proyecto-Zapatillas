using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarasScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "FloatingObject") {
			Rigidbody body = other.gameObject.GetComponent<Rigidbody> ();

			float force_x = Random.Range (-25.0f,25.0f);
			float force_y = Random.Range (-25.0f,45.0f);
			float force_z = Random.Range (-25.0f,25.0f);

			body.AddForce (force_x, force_y, force_z);

		}
	}
}
