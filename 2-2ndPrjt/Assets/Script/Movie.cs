using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Movie : MonoBehaviour {

    public MovieTexture movie;

    RawImage raw;

    public bool isCredit;
	// Use this for initialization
	void Start () {
        raw = GetComponent<RawImage>();
        PlayClip();
        Invoke("PlayAid", 1.0f);
        if (!isCredit)
            Invoke("Destroy", 5.0f);
        else
            Invoke("Pause", 110 / 30f);
	}
	
    void Destroy()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(1);
    }

    void Pause()
    {
        movie.Pause();
    }
	// Update is called once per frame

    void PlayClip()
    {
        raw.texture = movie;
        movie.Play();
    }

    void PlayAid()
    {
        GetComponent<AudioSource>().Play();

    }
	
}
