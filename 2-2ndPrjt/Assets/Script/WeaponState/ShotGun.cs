using UnityEngine;
using System.Collections;

public class ShotGun : MonoBehaviour
{
    Player pl;
    Manager instance;

    public int Ammo;
    public int MaxBullet;
    public int bulletCounter;
    public float ReloadDelay;
    public bool CanShot;
    public int shotCount;
    public bool pelletUp;
    public bool MotionDelay;
    float timer;
    public float timerG;
    bool reloading;
    public GameObject ReloadSound;
    public GameObject[] ShotSound; // 0 - Normal 1 - withPump

    void Start ()
    {
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
        if (bulletCounter <= 0 && !reloading)
        {
            CanShot = false;
            StartCoroutine(Reload(ReloadDelay));
            reloading = true;
        }

        switch (pl.State)
        {
            case Player.state.Far:
                pl.GetComponent<Animator>().SetBool("ShotgunIdle", true);
                Skill_Far();
                break;
            case Player.state.Range:
                pl.GetComponent<Animator>().SetBool("ShotgunIdle", true);
                Skill_Range();
                break;
            case Player.state.Melee:
                pl.GetComponent<Animator>().SetBool("ShotgunIdle", false);
                Skill_Melee();
                break;
        }     
    }

    public void Skill_Far()
    {
        if (!CanShot)
            return;
        if (Input.GetKey(KeyCode.Space) && CanShot)
        {
            if (pl.nowWalk) //이동사격
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
                instance.CreateEffect("ShotGun3", 0, 0, 600, 400);
                instance.Obj.transform.localScale += Vector3.right * 1.1f;
                Instantiate(ShotSound[1]);
                StartCoroutine(GlobalDelay(0.3f));
                StartCoroutine(motionDelay(0.2f));
                BulletMinus();
                //총격 애니메이션
            }
            else if (pl.backStep) //일반 사격
            {
                StartCoroutine(RapidShot(4, 0.1f));
                pl.GetComponent<Animator>().SetTrigger("ShotgunShot");

                StartCoroutine(GlobalDelay(0.3f));
                StartCoroutine(motionDelay(0.2f));
            }

            else if (Input.GetKey(KeyCode.DownArrow)) //수류탄
            {
                //수류탄 투척 애니메이션
                //수류탄 애니메이션
                //폭발 애니메이션
            }
            else //일반 사격
            {

                pl.GetComponent<Animator>().SetTrigger("ShotgunShot");
                instance.CreateEffect("NozzleFlash3");
                instance.CreateEffect("ShotGun3", 0, -5 + Random.Range(-3, 3), 0, 0);

                instance.CreateEffect("ShotGun3", 0, 0, 0, 0);

                instance.CreateEffect("ShotGun3", 0, 5 + Random.Range(-3, 3), 0, 0);

                for (int i = 0; i < 4; i++)
                    instance.CreateEffect("Pellet");

                Instantiate(ShotSound[0]);

                StartCoroutine(GlobalDelay(0.3f));
                StartCoroutine(motionDelay(0.2f));

                BulletMinus();
            }
        }
    }
    public void Skill_Range()
    {
        if (!CanShot)
            return;
        if (Input.GetKey(KeyCode.Space) && CanShot)
        {
            if (pl.nowWalk) //이동사격
            {
                //플레이어 이동사격 애니메이션
                //총알 회전각 계산
                //총격 애니메이션
            }
            if (pl.nowDash) //전방 대쉬 후 2연속 사격 / 펠렛 각 4개씩
            {
                pl.nowDash = false;
                pl.Dash(1, 50);
                StartCoroutine(Range_Dash());
                
                StartCoroutine(GlobalDelay(0.3f));
                StartCoroutine(motionDelay(1.2f));
            }
            else if (pl.backStep) //펠렛이 나가지 않는 공격.
            {
                pl.GetComponent<Animator>().SetTrigger("ShotgunShot");
                StartCoroutine(motionDelay(0.6f));
                StartCoroutine(GlobalDelay(0.3f));
                instance.CreateEffect("NozzleFlash3");
                instance.CreateEffect("ShotGun3", 0, -8 + Random.Range(-3, 3), 0, 0);
                instance.Obj.transform.localScale *= 1.2f;
                instance.CreateEffect("ShotGun3", 0, 0, 0, 0);
                instance.Obj.transform.localScale *= 1.2f;
                instance.CreateEffect("ShotGun3", 0, 8 + Random.Range(-3, 3), 0, 0);
                instance.Obj.transform.localScale *= 1.2f;

                Instantiate(ShotSound[0]);

                BulletMinus();
                //사격 후 멀리 백스텝
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                StartCoroutine(motionDelay(0.5f));
                instance.CreateEffect("NozzleFlash3");
                pelletUp = true;
                StartCoroutine(pelletSetting());


                StartCoroutine(GlobalDelay(0.5f));
                StartCoroutine(motionDelay(0.3f));
            }
            else //교정사격 - 명중률이 더 높음
            {
                pl.GetComponent<Animator>().SetTrigger("ShotgunShot");
                StartCoroutine(GlobalDelay(0.7f));
                StartCoroutine(motionDelay(0.5f));
                instance.CreateEffect("NozzleFlash3");
                StartCoroutine(pelletSetting());

            }
        }
    }
    public void Skill_Melee()
    {
        if (!CanShot)
            return;
        if (Input.GetKeyDown(KeyCode.Space) && CanShot)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {

            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                StartCoroutine(motionDelay(0.5f));
                instance.CreateEffect("NozzleFlash3");
                pelletUp = true;
                StartCoroutine(pelletSetting());
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || pl.backStep)
            {
                pl.GetComponent<Animator>().SetTrigger("ShotgunShot");
                StartCoroutine(GlobalDelay(0.3f));
                StartCoroutine(motionDelay(0.2f));
                instance.CreateEffect("NozzleFlash3");
                instance.CreateEffect("ShotGun3", 0, -8 + Random.Range(-3, 3), 0, 0);
                instance.Obj.transform.localScale *= 1.2f;
                instance.CreateEffect("ShotGun3", 0, 0, 0, 0);
                instance.Obj.transform.localScale *= 1.2f;
                instance.CreateEffect("ShotGun3", 0, 8 + Random.Range(-3, 3), 0, 0);
                instance.Obj.transform.localScale *= 1.2f;

                Instantiate(ShotSound[0]);
                BulletMinus();
            }
            else
            {
                pl.GetComponent<Animator>().SetTrigger("ShotgunShot");
                StartCoroutine(GlobalDelay(1.3f));
                StartCoroutine(motionDelay(1.2f));
                shotCount = 2;
                StartCoroutine(Meele_Left());

            }

        }
    }

    IEnumerator RapidShot(int ShotCount, float delay)
    {
        instance.CreateEffect("NozzleFlash3");
        instance.CreateEffect("ShotGun3", 0, 2, 0, 0);
        instance.CreateEffect("ShotGun3", 0, 0, 0, 0);
        instance.CreateEffect("ShotGun3", 0, -2, 0, 0);
        pl.GetComponent<Animator>().SetTrigger("ShotgunShot");

        Instantiate(ShotSound[0]);
        BulletMinus();

        while (ShotCount >= 1)
        {
            instance.CreateEffect("Pellet");
            instance.Obj.GetComponent<Effect>().Damage *= 5;
            yield return new WaitForSeconds(delay);
            ShotCount--;
            if (bulletCounter <= 0)
                shotCount = 0;
        }

        if (bulletCounter <= 0)
            StopCoroutine("RapidShot");

        instance.CreateEffect("ShotGun", 50, 100);
        instance.CreateEffect("Pellet");

        Instantiate(ShotSound[1]);
        instance.Obj.transform.position = instance.sl.transform.position;
        instance.Obj.GetComponent<Effect>().SetKnockBack(0, 500);
        BulletMinus();
    }

    IEnumerator Reload(float Delay)
    {
        CanShot = false;
        if (Ammo >= 1)
        {
            yield return new WaitForSeconds(0.1f);
            Instantiate(ReloadSound);
            yield return new WaitForSeconds(Delay -0.1f);
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



    IEnumerator Range_Dash()
    {

        yield return new WaitForSeconds(0.1f);

        pl.rigid.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.3f);


        pl. transform.position -= new Vector3(2, 0, 0);

        instance.CreateEffect("ShotGun3", 0, 2, 0, 80);
        instance.CreateEffect("ShotGun3", 0, 0, 0, 80);
        instance.CreateEffect("ShotGun3", 0, -2, 0, 80);
        pl.GetComponent<Animator>().SetTrigger("ShotgunShot");

        Instantiate(ShotSound[0]);
        BulletMinus();
        yield return new WaitForSeconds(0.2f);

        pl. transform.position -= new Vector3(2, 0, 0);
        instance.CreateEffect("ShotGun3", 0, 2, 0, 0);
        instance.CreateEffect("ShotGun3", 0, 0, 0, 0);
        instance.CreateEffect("ShotGun3", 0, -2, 0, 0);
        pl.GetComponent<Animator>().SetTrigger("ShotgunShot");

        Instantiate(ShotSound[0]);
        BulletMinus();
        for (int i = 0; i < 4; i++)
        {
            instance.CreateEffect("Pellet");
            instance.Obj.GetComponent<Effect>().SetKnockBack(10, 250);
        }

        pl.rigid.velocity = new Vector2(-5, 0);
    }
    IEnumerator Meele_Left()
    {
        pl. transform.position -= new Vector3(2, 0, 0);
        for (int i = 0; i < 4; i++)
        {
            instance.CreateEffect("Pellet");
            instance.Obj.GetComponent<Effect>().SetKnockBack(10, 10);
        }
        while (shotCount >= 1)
        {
            yield return new WaitForSeconds(0.2f);

            Instantiate(ShotSound[0]);
            pl. transform.position -= new Vector3(2, 0, 0);
            instance.CreateEffect("ShotGun3", 0, 2, 0, 0);
            instance.CreateEffect("ShotGun3", 0, 0, 0, 0);
            instance.CreateEffect("ShotGun3", 0, -2, 0, 0);
            BulletMinus();
            for (int i = 0; i < 4; i++)
            {
                instance.CreateEffect("Pellet");
                instance.Obj.GetComponent<Effect>().SetKnockBack(10, 10);
            }

            shotCount--;
        }
        pl.rigid.velocity = new Vector2(-8, 0);
        Instantiate(ReloadSound);
    }

    IEnumerator motionDelay(float sec)
    {
        timer += sec;
        MotionDelay = true;
        while (timer > 0)
        {
            pl.GetComponent<Animator>().SetBool("Run", false);
            timer -= Time.deltaTime;
            yield return null;
        }
        MotionDelay = false;
    }

    IEnumerator GlobalDelay(float Delay)
    {
        CanShot = false;
        timerG += Delay;
        while (timerG > 0)
        {
            timerG -= Time.deltaTime;
            yield return null;
        }
        if (bulletCounter >= 1)
        {
            CanShot = true;
            reloading = false;
        }
    }

 

    IEnumerator pelletSetting()
    {
        if (pelletUp == false)
        {
            BulletMinus();
            Instantiate(ShotSound[0]);
            for (int j = 0; j < 4; j++)
            {
                instance.CreateEffect("Pellet");
                instance.Obj.GetComponent<Effect>().Name = "RangeBoom";
                instance.Obj.transform.position = new Vector3(pl. transform.position.x + 3 + j * 3.5f, pl. transform.position.y + Random.Range(-3, 3));
                instance.Obj.transform.localScale += new Vector3(j * 0.3f, j * 0.3f, 0);
                yield return new WaitForSeconds(0.03f);
            }

            yield return new WaitForSeconds(0.2f);

            if (instance.sl.damageAtBoom)
            {
                instance.CreateEffect("Pellet");
                instance.Obj.transform.position = instance.sl.transform.position - new Vector3(0, 3, 0);
                instance.Obj.transform.localScale += new Vector3(1.2f, 1.2f, 0);
                Vector3 savePos = instance.Obj.transform.position;
                Instantiate(ShotSound[2]);
                for (int i = 0; i < 7; i++)
                {
                    instance.CreateEffect("Pellet");
                    instance.Obj.transform.position = new Vector3(savePos.x, savePos.y + i * 2 + Random.Range(-1, 1));
                    instance.Obj.transform.localScale += new Vector3(i * 0.2f, i * 0.2f, 0);
                    instance.Obj.GetComponent<Effect>().SetKnockBack(40, 120);
                    yield return new WaitForSeconds(0.04f);
                }
            }
            instance.sl.damageAtBoom = false;
        }
        else if (pelletUp == true)
        {
            BulletMinus();
            instance.CreateEffect("Pellet");
            instance.Obj.transform.position = instance.sl.transform.position - new Vector3(0, 3, 0);
            instance.Obj.transform.localScale += new Vector3(1.2f, 1.2f, 0);
            Vector3 savePos = instance.Obj.transform.position;
            Instantiate(ShotSound[2]);
            yield return new WaitForSeconds(0.2f);

            for (int i = 0; i < 6; i++)
            {
                instance.CreateEffect("Pellet");
                instance.Obj.transform.position = new Vector3(savePos.x,savePos.y + i * 2 + Random.Range(-1, 1));
                instance.Obj.transform.localScale += new Vector3(i * 0.3f, i * 0.3f, 0);
                instance.Obj.GetComponent<Effect>().SetKnockBack(40, 120);
                yield return new WaitForSeconds(0.07f);
            }
            pelletUp = false;
        }
    }

    void BulletMinus()
    {
        if (bulletCounter <= 0)
            return;
        bulletCounter--;
        pl.WeaponCont.MinusDuration();
    }
}

