using UnityEngine;
using System.Collections;

public class RifleAndGranade : MonoBehaviour
{
    Player pl;
    Manager instance;

    public int Ammo;
    public int MaxBullet;
    public int bulletCounter;
    public float ReloadDelay;
    public bool CanShot;
    bool reloading;

    public float aim;
    public float resetDelay;

    bool rifleDash;
    bool canUpper;
    public bool MotionDelay;
    float timer;


    public GameObject ReloadSound;
	// Use this for initialization
	void Start () {
        CanShot = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (instance == null)
            instance = Manager.instance;
        if (pl == null)
            pl = Manager.instance.pl;

        if (pl.nowchange)
            return;
        if (rifleDash)
        {

            pl. GetComponent<Animator>().SetBool("Run", true);
            if (pl.transform.position.x + 2.5f > Manager.instance.sl.transform.position.x)
            {
                pl.GetComponent<Animator>().SetBool("Run", false);
                pl.rigid.velocity = Vector2.zero;
                rifleDash = false;
            }
            
        }


        if (aim >= 0 && resetDelay < 0)
            aim -= 0.1f;
        
        if(resetDelay >= 0)
        {
            resetDelay -= 1 * Time.smoothDeltaTime;
        }

        if (bulletCounter <= 0 && !reloading)
        {
            StartCoroutine(Reload(ReloadDelay));
            reloading = true;
        }

        switch (pl.State)
        {
            case Player.state.Far:
                GetComponent<Animator>().SetBool("GunIdle", true);
                Skill_Far();
                break;
            case Player.state.Range:
                GetComponent<Animator>().SetBool("GunIdle", true);
                Skill_Range();
                break;
            case Player.state.Melee:
                GetComponent<Animator>().SetBool("GunIdle", false);
                Skill_Melee();
                break;
        }
    }


    public void Skill_Far()
    {
        if (!CanShot)
            return;
        if (Input.GetKey(KeyCode.Space))
        {
            if(pl.nowWalk) //이동사격
            {
                
                //플레이어 이동사격 애니메이션
                //총알 회전각 계산
                //총격 애니메이션
            }
            if (pl.nowDash) //구르기 - 사격
            {
                //플레이어 구르기 애니메이션
                //구르기 끝 프레임 호출, 총쏘기 애니메이션
                instance.CreateEffect("NozzleFlash2");
                instance.CreateEffect("ShotAim", 120, 800);
                instance.CreateEffect("Pellet");
                instance.Obj.transform.position = Manager.instance.sl.transform.position;
                BulletMinus();
                GetComponent<Animator>().SetBool("GunShot", true);
                BulletMinus();

                StartCoroutine(GlobalDelay(0.5f));
                //총격 애니메이션
            }
            else if (pl.backStep) //일반 사격
            {
                StartCoroutine(RapidShot(4, 0.1f));

                StartCoroutine(GlobalDelay(1.2f));
            }

            else if (Input.GetKey(KeyCode.DownArrow)) //수동 장전
            {

                //수류탄 투척 애니메이션
                //수류탄 애니메이션
                //폭발 애니메이션
            }
            else //일반 사격
            {
                //instance.CreateEffect("NozzleFlash3");
                //instance.CreateEffect("ShotGun3", 0, -5, 0, 0);

                //instance.CreateEffect("ShotGun3", 0, 0, 0, 0);

                //instance.CreateEffect("ShotGun3", 0, 5, 0, 0);
                //for (int i = 0; i < 6; i++)
                //    instance.CreateEffect("Pellet");

                instance.CreateEffect("NozzleFlash");
                instance.CreateEffect("Shot", 0, aim, 50, 80);
                BulletMinus();
                GetComponent<Animator>().SetBool("GunShot", true);
                if (aim <= 6f)
                    aim += 0.2f;

                resetDelay = 0.2f;
                StartCoroutine(GlobalDelay(0.05f));
            }
        }
    }
    public void Skill_Range()
    {
        if (!CanShot)
            return;
        if (Input.GetKey(KeyCode.Space))
        {
            if (pl.nowWalk) //이동사격
            {
                //플레이어 이동사격 애니메이션
                //총알 회전각 계산
                //총격 애니메이션
            }
            if (pl.nowDash) //착검 찌르기
            {
                pl.rigid.velocity += new Vector2(5, 0);//플레이어 대쉬 애니메이션
                rifleDash = true;

                canUpper = true;
                
                //구르기 끝 프레임 호출, 찌르기 애니메이션
                //총격 애니메이션
            }
            else if (pl.backStep) //후방이동 사격
            {
                pl.rigid.velocity += new Vector2(-2, 0);
                StartCoroutine(RapidShot(1, 0));
                instance.CreateEffect("NozzleFlash2");
                instance.CreateEffect("Shot", 0, aim, 50, 80, 5);
                instance.CreateEffect("Pellet");
                instance.Obj.transform.position = instance.sl.transform.position;
                GetComponent<Animator>().SetBool("GunShot", true);
                instance.Obj.GetComponent<Effect>().Damage = 15;
                StartCoroutine(GlobalDelay(0.8f));
                //일반적인 사격
            }
            else
            {
                instance.CreateEffect("NozzleFlash");
                instance.CreateEffect("Shot", 0, aim, 50, 80);
                BulletMinus();

                if (aim <= 3f)
                    aim += 0.2f;

                resetDelay = 0.2f;
                StartCoroutine(GlobalDelay(0.05f));
            }
        }
    }
    public void Skill_Melee()
    {
        if (!CanShot)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(canUpper)
            {

                instance.FlipAnim = 1;
                instance.CreateEffect("MoonSlash", 0, 0, 50, 600);
                
                canUpper = false;
                pl.rigid.velocity = new Vector2(-3, 0);

            }
            if (Input.GetKey(KeyCode.RightArrow))
            {

            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                pl.rigid.AddForce(new Vector2(-5, 2));
            }
            else
            {
                pl.Dash(-1, 5);
            }

        }
    }


    public float ComboCheck(int ComboCounter, int MaxCombo)
    {
        float Rate = 0;
        if (ComboCounter > MaxCombo)
            MaxCombo = ComboCounter;

        if (ComboCounter <= 100)
        {
            Rate = ComboCounter * 0.6f;
        }

        else if (ComboCounter <= 200)
        {
            Rate = ComboCounter * 1f;
        }

        else if (ComboCounter <= 500)
        {
            Rate = ComboCounter * 1.5f;
        }

        else if (ComboCounter >= 1000)
        {
            Rate = 1000;
        }

        return Rate;
    }


    IEnumerator RapidShot(int ShotCount, float delay)
    {
        if (delay == 0)
        {
            yield return new WaitForSeconds(0.8f);
            instance.CreateEffect("NozzleFlash");
            instance.CreateEffect("Shot", 0, 0, 50, 100);

        }
        while (ShotCount >= 1)
        {
            if (bulletCounter <= 0)
            {
                ShotCount = 0;
                break;
            }
            instance.CreateEffect("NozzleFlash");
            instance.CreateEffect("Shot",0, aim, 50, 100);
            yield return new WaitForSeconds(delay);
            ShotCount--;
            BulletMinus();
        }

    }

    IEnumerator Reload(float Delay)
    {
        aim = 0;
        CanShot = false;
        GetComponent<Animator>().SetBool("GunShot", false);
        if (Ammo >= 1)
        {
            Instantiate(ReloadSound);
            yield return new WaitForSeconds(Delay);
            Ammo--;
            bulletCounter = MaxBullet;
            CanShot = true;
            reloading = false;
        }
        else
        {
            pl.Changing();
        }
    }

    IEnumerator GlobalDelay(float Delay)
    {
        CanShot = false;
        GetComponent<Animator>().SetBool("GunShot", false);
        yield return new WaitForSeconds(Delay);
        if (bulletCounter >= 1)
        {
            CanShot = true;
            reloading = false;
        }
    }

    IEnumerator motionDelay(float sec)
    {
        timer += sec;
        MotionDelay = true;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        MotionDelay = false;
    }


    void BulletMinus()
    {
        if (bulletCounter <= 0)
            return;
        bulletCounter--;
        pl.WeaponCont.MinusDuration();
    }
}
