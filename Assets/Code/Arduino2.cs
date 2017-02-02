using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Collections;
using System.IO;
using System.IO.Ports;


public class Arduino2 : MonoBehaviour {

	int time_out_ = 5;

	/* The serial port where the Arduino is connected. */
	[Tooltip("The serial port where the Arduino is connected")]
	private string port = "/dev/cu.usbmodem1411";
	/* The baudrate of the serial port. */
	[Tooltip("The baudrate of the serial port")]
	private int baudrate = 9600;

	private SerialPort stream;

	public enum SensorID {
		Sensor0,
		Sensor1,
		Sensor2,
		Sensor3,
		Sensor4,
		Sensor5,
		Sensor6,
		Sensor7,
		Sensor8,
		Sensor9,
		Sensor10,
		Sensor11,
		Sensor12,
		Sensor13,
		Sensor14,
		Sensor15,
		None
	};

	public int[] sensor_values_;

	//cabeceras
	byte c0 = 0xFF;
	byte c1 = 0xBE;
	byte c2 = 0xFF;

	Thread hilo; 

	uint time_to_reed_ = 100000;
	uint time_pased_to_reed_ = 0;

	//sistema multihilo..
	private void ReadFromArduino() 
	{
		while (true) {
			
			if (stream.IsOpen) 
			{
				time_pased_to_reed_++;
				if(time_pased_to_reed_ > time_to_reed_)
				{
					time_pased_to_reed_ = 0;
					try
					{
						int bytes = stream.BytesToRead;
						//Debug.Log("Numero de bytes: " + bytes);
						byte[] buffer = new byte[bytes];
						stream.Read (buffer, 0, bytes);
						if(buffer [0]==c0 && buffer [1]==c1 && buffer [2]==c2)
						{
							for (int i = 3; i < bytes; ++i) {
								sensor_values_ [i-3] = (int)buffer [i];
							}
						}
						//stream.BaseStream.Flush();
					
					} catch (System.Exception) {
						//Debug.Log ("error");
					}
				}
			}
		}

	}



	public void Open () {
		// Opens the serial port
		stream = new SerialPort(port, baudrate);
		stream.ReadTimeout = time_out_;
		stream.Open();
	
	}


	public int GetSensorValue (SensorID sensor)
	{
		return sensor_values_[(int)sensor];
	}
		

	// Use this for initialization
	void Start () {
		
		sensor_values_ = new int[16];
		Open ();

		//Creamos el delegado 
		ThreadStart delegado = new ThreadStart(ReadFromArduino); 
		//Creamos la instancia del hilo 
		hilo = new Thread(delegado); 
		//Iniciamos el hilo 
		//hilo.Start();
	}
		

	void OnApplicationQuit() {
		hilo.Abort ();
		stream.Close ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (stream.IsOpen) 
		{
			try
			{
				/*
				int bytes = stream.BytesToRead;
				//Debug.Log("Numero de bytes: " + bytes);
				byte[] buffer = new byte[bytes];
				stream.Read (buffer, 0, bytes);
				if(buffer [0]==c0 && buffer [1]==c1 && buffer [2]==c2)
				{
					for (int i = 3; i < sensor_values_.Length; ++i) {
						sensor_values_ [i-3] = (int)buffer [i];
					}
				}
				//stream.BaseStream.Flush();
				*/

				//Debug.Log(stream.ReadLine().ToString());
				sensor_values_[0] = Int32.Parse( stream.ReadLine().ToString());
				sensor_values_[1] = Int32.Parse( stream.ReadLine().ToString());

				Debug.Log("Sensor 1: " + sensor_values_[0]);
				Debug.Log("Sensor 2: " + sensor_values_[1]);

			} catch (System.Exception) {

			}
		}


	}
}
