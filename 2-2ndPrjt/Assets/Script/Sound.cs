using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound : MonoBehaviour {
    AudioSource As;
    public AudioClip Asc;
	// Use this for initialization
	void Start () {
        As = GetComponent<AudioSource>();
        Asc = As.clip;
        Invoke("Destroy", Asc.length);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Destroy()
    {
        Destroy(gameObject);
    }
}
