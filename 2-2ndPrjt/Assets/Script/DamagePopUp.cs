using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour {
    
    public Animator A;


	// Use this for initialization
	void OnEnable ()
    {
        AnimatorClipInfo[] Cl = A.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, Cl[0].clip.length);
	}
	
	// Update is called once per frame

    public void DamageText(string Damage)
    {
        A.GetComponent<Text>().text = Damage;
    }
}
