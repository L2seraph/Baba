using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteColor : MonoBehaviour {
    SpriteRenderer Spr;
    Player pl;
    Slime sl;
	// Use this for initialization
	void Start () {
        Spr = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(pl == null)
        pl = Manager.instance.pl;
        if (sl == null)
            sl = Manager.instance.sl;

        Vector3 v = sl.transform.position - pl.transform.position;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg));
        if (pl.State == Player.state.Far)
        {
            Spr.color = new Vector4(255, 0, 0, 120);
        }
        else if(pl.State == Player.state.Range)
            Spr.color = new Vector4(255, 255, 0, 120);
        else if(pl.State == Player.state.Melee)
        Spr.color = new Vector4(255, 255, 255, 120);
    }
}
