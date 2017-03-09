using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Effect : MonoBehaviour {

    public bool imEffect;
    public float Damage;
    public float WallDamage;
    public Vector2 KnockBack = new Vector2(); 
    public bool activated;
    public int ComboCounter;
    public string Name;

    public bool Sfx;
    
    public float Rate = 0;

    public bool canAttackWall; //벽 공격가능 여부 판단

    public float deletePoison;
    // Use this for initialization
    void Start()
    {
        if (!Sfx)
        {
            AnimatorClipInfo[] clipfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
            Destroy(gameObject, clipfo[0].clip.length);
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!Sfx)
        KnockBack.x *= 0.9f;
       else if(Sfx)
        {

        }
    }
    

    public void SetKnockBack(int x, int y)
    {
        KnockBack.x = x;
        KnockBack.y = y;
    }

    public float ComboMulti()
    {

        ComboCounter = Manager.instance.sl.ComboCounter;

        if (Manager.instance.pl.W_State == Player.weaponState.Sword)
        {
            if (ComboCounter <= 30)
            {
                Rate = ComboCounter * 1.2f;
            }
            else if(ComboCounter <= 60)
            {
                Rate = ComboCounter * 2.5f;
            }
            else if(ComboCounter <= 100)
            {
                Rate = ComboCounter * 4.2f;
            }
            else if (ComboCounter <= 200)
            {
                Rate = ComboCounter * 5f;
            }
            else if (ComboCounter <= 500)
            {
                Rate = ComboCounter * 8f;
            }

            else if (ComboCounter < 1000)
            {
                Rate = 5600;
            }

            else if (ComboCounter >= 1000)
            {
                Rate = 7280;
            }
        }
        if (Manager.instance.pl.W_State == Player.weaponState.Rifle)
        {
            if (ComboCounter <= 100)
            {
                Rate = ComboCounter * 1f;
            }

            else if (ComboCounter <= 200)
            {
                Rate = ComboCounter * 1.8f;
            }

            else if (ComboCounter <= 500)
            {
                Rate = ComboCounter * 3f;
            }

            else if (ComboCounter < 1000)
            {
                Rate = 1800;
            }

            else if (ComboCounter >= 1000)
            {
                Rate = 3000;
            }
        }
        if (Manager.instance.pl.W_State == Player.weaponState.Shotgun)
        {
            if (ComboCounter <= 100)
            {
                Rate = ComboCounter * 3f;
            }

            else if (ComboCounter <= 200)
            {
                Rate = ComboCounter * 5f;
            }

            else if (ComboCounter <= 500)
            {
                Rate = ComboCounter * 6f;
            }

            else if (ComboCounter < 1000)
            {
                Rate = 3500;
            }

            else if (ComboCounter >= 1000)
            {
                Rate = 6500;
            }
        }
        if (Manager.instance.pl.W_State == Player.weaponState.Hammer)
        {
            if (ComboCounter <= 100)
            {
                Rate = ComboCounter * 10f;
            }

            else if (ComboCounter <= 200)
            {
                Rate = ComboCounter * 25f;
            }

            else if (ComboCounter <= 500)
            {
                Rate = ComboCounter * 30f;
            }

            else if (ComboCounter < 1000)
            {
                Rate = 16800;
            }

            else if (ComboCounter >= 1000)
            {
                Rate = 23000;
            }
        }
        if (Manager.instance.pl.W_State == Player.weaponState.Rocket)
        {
            if (ComboCounter <= 100)
            {
                Rate = ComboCounter * 10f;
            }

            else if (ComboCounter <= 200)
            {
                Rate = ComboCounter * 20f;
            }

            else if (ComboCounter <= 500)
            {
                Rate = ComboCounter * 25f;
            }

            else if (ComboCounter < 1000)
            {
                Rate = 13500;
            }

            else if (ComboCounter >= 1000)
            {
                Rate = 15000;
            }
        }

        return Rate;

    }
}
