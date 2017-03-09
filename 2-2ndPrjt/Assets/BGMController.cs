using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour {
    AudioSource sound;
	// Use this for initialization
	void Start () {
        sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            sound.volume -= 0.1f;
        if (Input.GetKeyDown(KeyCode.E))
            sound.volume += 0.1f;
        if (Input.GetKeyDown(KeyCode.T))
            sound.pitch += 0.1f;
        if (Input.GetKeyDown(KeyCode.W))
            sound.pitch -= 0.1f;
    }
}
