using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextoDistancia : MonoBehaviour {

	private Text texto_;

	public bool filtrate_ = false;

	public Arduino2 arduino_;

	public Arduino2.SensorID sensor_id_ = Arduino2.SensorID.None;

	// Use this for initialization
	void Start () {


	}

	void Awake()
	{
		texto_ = gameObject.GetComponent<Text> ();

	}

	// Update is called once per frame
	void Update () {
		if (!filtrate_)
			texto_.text = arduino_.GetSensorValue (sensor_id_).ToString ();
		else {
			int value = arduino_.GetSensorValue (sensor_id_);
			if (value > 100)
				value = 100;

			texto_.text = "Filtrado: " + value.ToString ();
		}
	}
}
