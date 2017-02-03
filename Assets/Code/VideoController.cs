using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoController : MonoBehaviour {


    public MovieTexture movie_text_1_;

	Vector3 default_position_;

    private Material mat_;

	public float anim_time_ = 0.5f;
	private float anim_time_pased_ = 0.0f;


    // Use this for initialization
    void Start () {
		
	}

    void Awake()
    {
        MeshRenderer ren = gameObject.GetComponent<MeshRenderer>();
        mat_ = ren.material;
        movie_text_1_.loop = true;
		default_position_ = gameObject.transform.position;
       
    }
    // Update is called once per frame
    void Update () {

		if (Input.GetKey (KeyCode.Q) && !movie_text_1_.isPlaying)
		{
			PlayAnimVideo ();
		}
		else if(Input.GetKeyUp(KeyCode.Q))
		{
			ResetAnimVideo ();
		}

       
    }


	public void PlayAnimVideo()
	{
		anim_time_pased_ += Time.deltaTime;

		Vector3 pos = gameObject.transform.position;
		pos.y = Mathf.Lerp (default_position_.y, 0.0f, anim_time_pased_/ anim_time_ );

		gameObject.transform.position = pos;

		if (anim_time_pased_ > anim_time_) 
		{
			anim_time_pased_ = 0.0f;
			PlayVideo ();
		}
	}

	public void StopAnimVideo()
	{
		
	}

	public void ResetAnimVideo()
	{
		StopVideo ();
		gameObject.transform.position = default_position_;
	}
  
    public void PlayVideo()
    {
        if (!movie_text_1_.isPlaying)
        {
            //Debug.Log("Playing video");
            movie_text_1_.Play();
        }
    }

    public void StopVideo()
    {
        movie_text_1_.Stop();
        //mat_.SetFloat("_Desintegracion", 0.0f);
        mat_.mainTexture = movie_text_1_;
    }
}
