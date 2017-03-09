using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameRestart : MonoBehaviour {
    public GameObject obj;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            obj.SetActive(true);
        }
    }
}
