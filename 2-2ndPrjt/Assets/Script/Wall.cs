using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    public float Hp;
    public float DamageCounter;
    public bool isRightWall;
    public bool isFloor;
    public bool isBreaked;
    public Wall LeftWall;

    public GameObject RealCollider;

    public Sprite[] WallGold;

    public Vector3 CameraPos;
    public float LerpRange;
    public bool Came;
    // Use this for initialization
    void Start ()
    {
	
	}

    // Update is called once per frame

    void Update()
    {
        if (Came == true)
        {
            if (LerpRange <= 1)
            {
                Manager.instance.pl.nowchange = true;
                Manager.instance.sl.ComboDelay = 3.5f;
                LerpRange += Time.deltaTime;
                CameraMove(LerpRange);
            }
            else if (LerpRange >= 1)
            {
                Manager.instance.pl.nowchange = false;
                LerpRange = 1;
                Manager.instance.pl.transform.position = Manager.instance.sl.transform.position - (Vector3.right * 25);
                Manager.instance.CreateEffect("Flash");
                CameraMove(LerpRange);
                Came = false;
                LerpRange = 0;
                Manager.instance.pl.Changing();
            }
        }

        if(isRightWall==true && Input.GetKeyDown(KeyCode.Keypad0))
        {
            DamageCounter = Hp - 1;
        }

        if ((Hp - DamageCounter) / Hp > 0.6f) // 100~60%
        {
            GetComponent<SpriteRenderer>().sprite = WallGold[0];
        }
        else if ((Hp - DamageCounter) / Hp > 0.3f) //60~30%
        {
            GetComponent<SpriteRenderer>().sprite = WallGold[1];
        }
        else if ((Hp - DamageCounter) / Hp < 0.3f)
        {
            GetComponent<SpriteRenderer>().sprite = WallGold[2];
        }
        if (DamageCounter >= Hp && isRightWall)
        {
            Manager.instance.pl.CollectWeapon = 4;
            Manager.instance.pl.ChangeWeapon();
            CameraPos = Manager.instance.cam.transform.position;
        }
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (isFloor)
        { 
            if (coll.CompareTag("Slime"))
            {
                if(Manager.instance.sl.ComboDelay < 0.2f)
                    Manager.instance.sl.ComboDelay = 0.5f;

                if (Manager.instance.sl.nowAir == true)
                {
                    coll.GetComponent<Animator>().SetTrigger("Drop");
                    coll.GetComponent<Slime>().nowAir = false;
                    coll.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                }
            }
        }
        if (coll.CompareTag("Effect"))
        {
            if (isRightWall)
            {
                if (coll.GetComponent<Effect>().canAttackWall)
                {
                    DamageCounter += coll.GetComponent<Effect>().WallDamage;
                }
            }

        }
        if (coll.CompareTag("Pl"))
        {
            if (Manager.instance.pl.nowJump == true && isFloor)
                Manager.instance.pl.nowJump = false;
        }
    }

    void OnTriggerExit2D(Collider2D Coll)
    {
        if (isFloor)
        {
            if (Coll.transform.CompareTag("Pl"))
                if (Manager.instance.pl.nowJump == false)
                    Manager.instance.pl.nowJump = true;
            if (Coll.CompareTag("Slime"))
            {
                Manager.instance.sl.nowAir = true;
                Coll.GetComponent<Animator>().SetTrigger("Damaged");
            }
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Slime"))
        {
            if (isRightWall)
            {
                coll.GetComponent<Rigidbody2D>().velocity = new Vector2(-7f, coll.GetComponent<Rigidbody2D>().velocity.y);
                DamageCounter += 1;
            }
            if (isFloor)
            {
                
            }
        }

    }


    public void HammerChange()
    {
        RealCollider.GetComponent<BoxCollider2D>().enabled = false;
        Manager.instance.BreakedWallCount++;
        Manager.instance.sl.CD.DamageLevel -= 3;
        Manager.instance.sl.poison.PlusMaxBar();
        DamageCounter = 0;
        this.transform.position += new Vector3(40, 0, 0);
        LeftWall.transform.position += new Vector3(40, 50, 0);
        Manager.instance.sl.rigid.AddForce(new Vector2(2200, -100));
        RealCollider.GetComponent<BoxCollider2D>().enabled = true;
        int Break = Manager.instance.BreakedWallCount;
        if (Break < 3) // 0 1 2
        {
            Hp *= 1.15f;
        }
        else if (Break < 6) // 3 4 5
        {
            Hp *= 1.24f;
        }
        else if (Break < 9) // 6 7 8
        {
            Hp *= 1.35f;
        }
        else if (Break < 15) //  9 10 11 12 13 14
        {
            Hp *= 1.6f;
        }
        else if (Break < 18) // 13 14
        {
            Hp *= 1.1f;
        }
        else if (Break > 24)
        {
            Hp *= 1.05f;
        }
    }


    void CameraMove(float L)
    {
        Manager.instance.cam.transform.position = Vector3.Lerp(CameraPos, CameraPos + new Vector3(40, 0, 0), L);
    }
}
