using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoController : MonoBehaviour {


    public MovieTexture movie_text_1_;
    public MovieTexture movie_text_2_;
    public Quaternion default_rotation_;

    private Material mat_;

    //desintegration anim
    enum AnimPhase
    {
        None,
        StartDesintegration,
        RunDesintegration,
        StartReconstruction,
        RunReconstruction,
        EndAnim
    };
    AnimPhase anim_phase_ = AnimPhase.None;

    float time_to_desintegrate = 0.5f;
    float time_to_desintegrate_pased_ = 0.0f;

    float time_to_reconstruction = 0.5f;
    float time_to_reconstruction_pased_ = 0.0f;


    // Use this for initialization
    void Start () {
		
	}

    void Awake()
    {
        MeshRenderer ren = gameObject.GetComponent<MeshRenderer>();
        mat_ = ren.material;
        movie_text_1_.loop = true;
        //mat_.mainTexture = movie_text_2_; 
        default_rotation_ = gameObject.transform.rotation;
    }
    // Update is called once per frame
    void Update () {

        // float y0 = transform.rotation.y;
        //transform.rotation.Set(0.0f,0.0f, 200.0f * Mathf.Sin(10.0f * Time.time),1.0f);

        UpdateTransitionAnim();
    }


    public void UpdateTransitionAnim()
    {
        
        switch (anim_phase_)
        {
            case AnimPhase.StartDesintegration:
                {
                    anim_phase_ = AnimPhase.RunDesintegration;
                    
                }
                break;
            case AnimPhase.RunDesintegration:
                {
                    UpdateDesintegration();
                }
                break;
            case AnimPhase.StartReconstruction:
                {
                    anim_phase_ = AnimPhase.RunReconstruction;
                    if (movie_text_1_ == mat_.mainTexture)
                    {
                        movie_text_1_.Stop();
                        mat_.mainTexture = movie_text_2_;
                    }
                    else
                    {
                        mat_.mainTexture = movie_text_1_;
                        movie_text_2_.Stop();
                    }
                }
                break;
            case AnimPhase.RunReconstruction:
                {
                    UpdateReconstruction();
                }
                break;
            case AnimPhase.EndAnim:
                {
                    anim_phase_ = AnimPhase.None;
                }
                break;

        }
        
    }

    void UpdateDesintegration()
    {
        time_to_desintegrate_pased_ += Time.deltaTime;

        float desintegration_value = Mathf.Lerp(0.0f, 1.0f, (time_to_desintegrate_pased_ / time_to_desintegrate));

        mat_.SetFloat("_Desintegracion", desintegration_value);

        if (time_to_desintegrate_pased_ > time_to_desintegrate)
        {
            time_to_desintegrate_pased_ = 0.0f;
            anim_phase_ = AnimPhase.StartReconstruction;
        }
    }

    void UpdateReconstruction()
    {
        time_to_reconstruction_pased_ += Time.deltaTime;

        float reconstruction_value = Mathf.Lerp(1.0f, 0.0f, (time_to_reconstruction_pased_ / time_to_reconstruction));

        mat_.SetFloat("_Desintegracion", reconstruction_value);

        if (time_to_reconstruction_pased_ > time_to_reconstruction)
        {
            time_to_reconstruction_pased_ = 0.0f;
            anim_phase_ = AnimPhase.EndAnim;
        }
    }

    public void PlayVideoAnim()
    {
        anim_phase_ = AnimPhase.StartDesintegration;
    }

    public void PlayVideo()
    {
        if (!movie_text_1_.isPlaying)
        {
            Debug.Log("Playing video");
            movie_text_1_.Play();
        }
    }

    public void StopVideo()
    {
        movie_text_1_.Stop();
        movie_text_2_.Stop();
        anim_phase_ = AnimPhase.None;
        mat_.SetFloat("_Desintegracion", 0.0f);
        mat_.mainTexture = movie_text_1_;
    }
}
