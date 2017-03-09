using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public bool nowchange;
    Sword sword;
    RifleAndGranade rifle;
    ShotGun shotGun;
    Hammer hammer;

    public Text WeaponStockCounter;

    public ATKImageController WeaponCont;

    public int WeaponStock;

    public enum state
    {
        Melee = 1,
        Range = 2,
        Far = 3,
    }

    public enum weaponState
    {
        Sword = 0,
        Rifle = 1,
        Shotgun = 2,
        Rocket = 3,
        Hammer = 4,
    }

    public int ComboCounter;
    public int MaxCombo;
    public float ComboDelay; //딜레이 안에 타격 실패시 콤보 제거
    public Rigidbody2D rigid;
    public Transform body;

    public float Speed;
    public float Speed_Slowed;

    //state
    public bool backStep;
    public bool nowDash;
    public bool nowJump;
    public bool nowWalk;
    public bool nowStanding;

    public int Axis;
    
    public state State;
    public weaponState W_State;

    public string WeaponName;

    public int Damage; //무기 내구도

    public int[] NextWeaopn;
    public int CollectWeapon;

    public bool nowChoosing;

    public GameObject[] Buttons;
	// Use this for initialization

    void Start ()
    {
        nowchange = false;
        WeaponStock = 1;
        rigid = GetComponent<Rigidbody2D>();
        sword = GetComponent<Sword>();
        rifle = GetComponent<RifleAndGranade>();
        shotGun = GetComponent<ShotGun>();
        hammer = GetComponent<Hammer>();
        //Wp = GameObject.Find("Weapon").GetComponent<Weapon>();
	}

    void Update()
    {
        WeaponStockCounter.text = WeaponStock.ToString();
        if (Buttons[2].activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                WeaponLeft();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                WeaponRight();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Changing();
        }
        if (!nowDash && rigid.velocity.x > 3)
        {
            rigid.velocity -= new Vector2(3, 0);
        }

        if (hammer.enabled == true)
            return;
        if (nowJump)
        {
            GetComponent<Animator>().SetBool("OnAir", true);
        }
        else
            GetComponent<Animator>().SetBool("OnAir", false);

        if (Axis == 1 || Axis == -1)
        {
            nowWalk = true;
            nowStanding = false;
        }
        else if (Axis == 0)
        {
            nowWalk = false;
            if (!nowJump && !nowDash)
                nowStanding = true;

            GetComponent<Animator>().SetBool("Run", false);
        }

    }

    public void Move(float axis)
    {
        if (sword.MotionDelay || rifle.MotionDelay || shotGun.MotionDelay || nowchange)
            return;

        Axis = (int)axis;
            
        if (nowDash)
        {
            Axis = 0;
            nowWalk = false;
        }
        else
        {
            if (nowJump || nowDash || backStep)
            {
                 transform.Translate(new Vector2(Axis * Speed_Slowed * Time.deltaTime, 0));
            }
            else
            {
                if (Speed == 0)
                    return;
                 transform.Translate(new Vector2(Axis * Speed * Time.deltaTime, 0));

                if (Axis != 0)
                    GetComponent<Animator>().SetBool("Run", true);
            }
        }
    }

    public void OnCollisionExit2d(Collision2D Coll)
    {
        //if (Coll.transform.CompareTag("Wall"))
            //nowJump = true;
    }

    public void Jump(float velop)
    {
        if (nowJump)
            return;
        nowJump = true;
        rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y + velop);
    }

    public void Dash(float axis, float power)
    {

        float flip;
        flip = 1;
        if (Manager.instance.sl.transform.position.x < transform.position.x)
            flip = -1;

        if (flip == 1)
        {
            if (axis == 1) //Dash
            {
                rigid.velocity = new Vector2(power, 1);
                nowDash = true;
            }
            else if (axis == -1) //backstep
            {
                rigid.velocity = (new Vector2(-power, 3));
                backStep = true;
            }
        }
        else if (flip == -1)
        {
            if (axis == -1) //Dash
            {
                rigid.velocity = new Vector2(-power, 1);
                nowDash = true;
            }
            else if (axis == 1) //backstep
            {
                rigid.velocity = (new Vector2(power, 3));
                backStep = true;
            }
        }
       
        StartCoroutine("DashCancel");
    }

    IEnumerator DashCancel()
    {
        Speed = 0;
        if (nowDash)
        {

        }
        if (backStep)
        {

        }
        yield return new WaitForSeconds(0.4f);
        if (nowDash)
        {
            nowDash = false;
        }
        else if (backStep)
        {
            backStep = false;
        }
        Speed = 10;
    }

    public void ChangeWeapon()
    {
        switch (CollectWeapon)
        {
            case 0:
                W_State = weaponState.Sword;

                GetComponent<Animator>().SetBool("GunIdle", false);
                GetComponent<Animator>().SetBool("ShotgunIdle", false);
                sword.enabled = true;
                rifle.enabled = false;
                shotGun.enabled = false;
                sword.DurationReset();
                WeaponCont.Radius = 0;
                WeaponCont.ImageSet(0);
                break;
            case 1:
                W_State = weaponState.Rifle;

                GetComponent<Animator>().SetBool("ShotgunIdle", false);
                GetComponent<Animator>().SetBool("GunIdle", true);
                sword.enabled = false;
                rifle.enabled = true;
                shotGun.enabled = false;
                rifle.CanShot = true;
                rifle.bulletCounter = 15;
                rifle.Ammo = 3;
                WeaponCont.Radius = 90f / 16f;
                WeaponCont.ImageSet(1);
                break;
            case 2:
                W_State = weaponState.Shotgun;
                GetComponent<Animator>().SetBool("ShotgunIdle", true);
                GetComponent<Animator>().SetBool("GunIdle", false);
                sword.enabled = false;
                rifle.enabled = false;
                shotGun.enabled = true;
                shotGun.CanShot = true;
                
                shotGun.bulletCounter = 4;
                shotGun.Ammo = 3;
                WeaponCont.Radius = 90f / 5f;
                WeaponCont.ImageSet(2);
                break;
            case 3:
                W_State = weaponState.Rocket;
                sword.enabled = false;
                rifle.enabled = false;
                shotGun.enabled = false;
                break;
            case 4:
                W_State = weaponState.Hammer;
                GetComponent<Animator>().SetBool("isShotGun", false);
                GetComponent<Animator>().SetBool("isGun", false);
                sword.enabled = false;
                rifle.enabled = false;
                shotGun.enabled = false;
                hammer.enabled = true;
                break;
        }
        nowchange = false;
    }

    public void Changing()
    {
        if (hammer.enabled)
        {
            hammer.enabled = false;
            WeaponStock += 1;

        }
        
        NextWeaopn[0] = NextWeaopn[1];

        while (NextWeaopn[0] == NextWeaopn[1])
        {
            NextWeaopn[0] = Random.Range(0, 3);
            NextWeaopn[1] = Random.Range(0, 3);
        }

        Buttons[2].SetActive(true);

        switch (NextWeaopn[0])
        {
            case 0:
                Buttons[0].transform.FindChild("Image").GetComponent<Text>().text = "Sword";
                break;
            case 1:
                Buttons[0].transform.FindChild("Image").GetComponent<Text>().text = "Rifle";
                break;
            case 2:
                Buttons[0].transform.FindChild("Image").GetComponent<Text>().text = "ShotGun";
                break;
        }

        switch (NextWeaopn[1])
        {
            case 0:
                Buttons[1].transform.FindChild("Image").GetComponent<Text>().text = "Sword";
                break;
            case 1:
                Buttons[1].transform.FindChild("Image").GetComponent<Text>().text = "Rifle";
                break;
            case 2:
                Buttons[1].transform.FindChild("Image").GetComponent<Text>().text = "ShotGun";
                break;
        }

        Time.timeScale = 0.1f;
        WeaponStock--;
        nowchange = true;
    }

    public void WeaponLeft()
    {
        CollectWeapon = NextWeaopn[0];
        ChangeWeapon();

        Buttons[2].SetActive(false);

        Time.timeScale = 1f;
    }

    public void WeaponRight()
    {
        CollectWeapon = NextWeaopn[1];
        ChangeWeapon();

        Buttons[2].SetActive(false);

        Time.timeScale = 1f;
    }

}
