using UnityEngine;
using System.Collections;

public class Hammer : MonoBehaviour
{
    public bool red;
    public Wall RightWall;
    public bool UseOnes;


   
	// Use this for initialization
	void Start () {
        red = false;
    }

    void OnEnable()
    {
        UseOnes = true;
    }
	

    void OnDisable()
    {
        UseOnes = true;
    }
	// Update is called once per frame
	void Update () {
        if (UseOnes)
        {
            Manager.instance.sl.ComboDelay = 3.5f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HammerStrike();
            UseOnes = false;
            }
            
        }
           
	}
    

    public void HammerStrike()
    {
        StartCoroutine(Hammer1());

    }

    IEnumerator Hammer1()
    {
        Manager.instance.pl.nowchange = true;
        GetComponent<Animator>().SetTrigger("Hammer1");
        Manager.instance.CreateEffect("Stomp");
        Manager.instance.CreateEffect("Flash");
        yield return new WaitForSeconds(0.5f);
        Manager.instance.sl.rigid.velocity = Vector2.zero;
        Manager.instance.sl.rigid.AddForce(new Vector2(0, 650));
        yield return new WaitForSeconds(1.0f);
        Manager.instance.pl.transform.position = Manager.instance.sl.transform.position - new Vector3(4,0,0);
        Manager.instance.pl.GetComponent<Animator>().SetTrigger("Hammer2");
        yield return new WaitForSeconds(0.5f);
        Manager.instance.CreateEffect("Flash");
        red = true;
        RightWall.HammerChange();
        UseOnes = true;
        RightWall.Came = true;
        Manager.instance.sl.flyGrabty = 5f;
        Manager.instance.sl.rigid.gravityScale = 1f;
        Manager.instance.pl.WeaponStock += 1;
        this.enabled = false;
        //

        
    }
    
}
