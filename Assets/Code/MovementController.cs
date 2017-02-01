using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    //public VideoController video_quad_;

	public Arduino2 arduino_;
	public Arduino2.SensorID sensor_id_ = Arduino2.SensorID.None;

	public GameObject cara_cubo_;
	public GameObject cara_circulo_;

	public AnimationCurve curve_anim_volver_;
	public float anim_time_volver_ = 0.3f;
	public AnimationCurve curve_anim_50_100_;
	public float anim_time_50_100_ = 0.3f;

	public AnimationCurve curve_anim_0_49_;
	public float anim_time_0_49_ = 0.3f;

	private bool must_run_anim_volver_ = false;
	private bool must_run_anim_0_49_ = false;
	private bool must_run_anim_50_100_ = false;
	
	private float time_pased_ = 0.0f;
	private float begining_z_pos_ = 0.0f;

	private bool is_running_any_anim_ = false;

	float new_pos_z_;

	private List<int> list_0_;

	private Vector3 default_gravity_;

	// Use this for initialization
	void Start () {
		list_0_ = new List<int>();
		default_gravity_ = Physics.gravity;
	}

	void Awake()
	{
		begining_z_pos_ = cara_cubo_.transform.position.z;
	}

	// Update is called once per frame
	void Update () {

		new_pos_z_ = cara_cubo_.transform.position.z;

 
		int arduino_value = arduino_.GetSensorValue (sensor_id_);
		//if(arduino_value!=0)
			//Debug.Log ("value: " + arduino_value.ToString());
		//sistema que detecta si devuelve 0 de valor 
		if (arduino_value == 0) {
			list_0_.Add (arduino_value);
			//arduino_value = arduino_last_value_;
		} else {
			list_0_.Clear ();
		}

		//movimiento
		if (!is_running_any_anim_) {
			if ((list_0_.Count > 160 || arduino_value >= 150) && begining_z_pos_!= new_pos_z_) {
				must_run_anim_volver_ = true;
				is_running_any_anim_ = true;
				//new_pos.z = new_pos.z - (2.0f * Time.deltaTime);
			} else if ((arduino_value > 0 && arduino_value < 80) && 1.5f!= new_pos_z_){
				must_run_anim_0_49_ = true;
				is_running_any_anim_ = true;
				//new_pos.z = new_pos.z + (1.0f * Time.deltaTime);
			} else if (arduino_value >= 80 && arduino_value < 150) {
				must_run_anim_50_100_ = true;
				is_running_any_anim_ = true;
			} 
		}
		else
		{
			if (must_run_anim_volver_) {
				RunAnimVolver ();
			} else if (must_run_anim_0_49_) {
				RunAnim0_49 ();
			} else if (must_run_anim_50_100_) {
				RunAnim50_100 ();
			}
		}

		//arduino_last_value_ = arduino_value;
			


		//margenes
		if (new_pos_z_ < 0.0f)
		{
			new_pos_z_ = 0.0f;
		}

		if (new_pos_z_ > 1.5f)
		{
			new_pos_z_ = 1.5f;
		}

		//seteo pos cubo
		Vector3 new_pos = cara_cubo_.transform.position;
		new_pos.z = new_pos_z_;
		cara_cubo_.transform.position = new_pos;

		//seteo circulo
		new_pos = cara_circulo_.transform.position;
		new_pos.z = new_pos_z_;
		cara_circulo_.transform.position = new_pos;


        
        if (Input.GetKey(KeyCode.Q))
        {
            //text_controller_2_.SetVisible(true);
            //text_controller_.SetVisible(false);
            //video_quad_.PlayVideoAnim();

			Physics.gravity = new Vector3 (0.0f,0.0f,0.0f);
        }
        else 
        {
            //text_controller_2_.SetVisible(false);
            //text_controller_.SetVisible(true);
			Physics.gravity = default_gravity_;
        }
        

    }

	void RunAnimVolver()
	{
		time_pased_ += Time.deltaTime;
		new_pos_z_ = Mathf.Lerp (1.5f,begining_z_pos_, curve_anim_volver_.Evaluate(time_pased_/anim_time_volver_));
		//Debug.Log ("Ejecutando Animacion  ");
		if (time_pased_ > anim_time_volver_) 
		{
			//Debug.Log ("Animacion finalizada ");
			time_pased_ = 0.0f;
			must_run_anim_volver_ = false;
			is_running_any_anim_ = false;
		}
	}


	void RunAnim0_49()
	{
		time_pased_ += Time.deltaTime;
		new_pos_z_ = Mathf.Lerp (0.0f,1.5f, curve_anim_0_49_.Evaluate(time_pased_/anim_time_0_49_));
		//Debug.Log ("Ejecutando Animacion  ");
		if (time_pased_ > anim_time_0_49_) 
		{
			//Debug.Log ("Animacion finalizada ");
			time_pased_ = 0.0f;
			must_run_anim_0_49_ = false;
			is_running_any_anim_ = false;
		}
	}

	void RunAnim50_100()
	{
		time_pased_ += Time.deltaTime;
		new_pos_z_ = Mathf.Lerp (begining_z_pos_,1.0f, curve_anim_50_100_.Evaluate(time_pased_/anim_time_50_100_));
		//Debug.Log ("Ejecutando Animacion  ");
		if (time_pased_ > anim_time_50_100_) 
		{
			//Debug.Log ("Animacion finalizada ");
			time_pased_ = 0.0f;
			must_run_anim_50_100_ = false;
			is_running_any_anim_ = false;
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "FloatingObject") {
			Rigidbody body = other.gameObject.GetComponent<Rigidbody> ();
			body.AddForce (0.0f, 0.0f, 10.0f);
		}
	}
}
