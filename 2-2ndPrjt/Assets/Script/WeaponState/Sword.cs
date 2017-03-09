using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
    Player pl;

    public bool nowAttack;
    public bool MotionDelay;
    //여기서부터는 연계기 연계시간 체크
    public bool canUpper;
    public bool canMagnet;
    public bool canFall;
    public float Duration; //내구도
    float Tt;

    public GameObject[] Sounds;

    bool coolingSword;
    Manager instance;

    string SkillOn;
    private bool FastDrawDelay;

    float attackCount;
    public float timer;
    void start()
    {
        Duration = 150;
    }

    public void DurationReset()
    {
        Duration = 150;
    }

    void Update()
    {
        if(attackCount >= 1)
        {
            if(attackCount == 1)
                Tt = 1.1f;
            Tt -= Time.deltaTime;
            if(Tt <= 0)
            {
                attackCount = 0;
            }
        }

        if (pl == null)
            pl = Manager.instance.pl;
        if (instance == null)
            instance = Manager.instance;


        if (pl.nowchange)
            return;

        if (Duration <= 0)
        {
            pl.Changing();
            this.enabled = false;
        }


        //instance = Manager.instance;
        switch (pl.State)
        {
            case Player.state.Far:
                Skill_Far();
                break;
            case Player.state.Range:
                Skill_Range();
                break;
            case Player.state.Melee:
                Skill_Melee();
                break;
        }

        if (MotionDelay)
            GetComponent<Animator>().SetBool("Run", false);

    }

    //Player Skill Using -> State Check -> Weapon Skill -> VFX + Anim

    public void Skill_Far()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !nowAttack)
        {
            attackCount = 0;

            if (pl.nowDash) //일섬
            {

                pl. GetComponent<Animator>().SetTrigger("RangeBlade");
                pl.nowDash = false;
                instance.CreateEffect("LongSlash", -4, 4, 50, 20);
                pl.rigid.velocity = Vector2.zero;
                pl.nowDash = false;
                pl. transform.position = new Vector2(instance.sl.transform.position.x - 1, instance.sl.transform.position.y);
                instance.Obj.GetComponent<Effect>().SetKnockBack(50, 20);
                instance.CreateEffect("Flash");
                SkillOn = "canUpper";
                StartCoroutine("nextSkill");
                StartCoroutine(attackCooling(0.2f));
                StopCoroutine("motionDelay");
                StartCoroutine(motionDelay(1.2f));
                Duration -= 5;
            }
            else if(pl.backStep) //발도
            {
                instance.rotMin = -40;
                instance.rotMax = 180;
                instance.Count = 2;
                instance.SecDelay = 0.08f;
                instance.settingEffect = "MoonSlash";
                instance.CreateEffect("Flash");
                instance.CreateEffect("FastDraw", 0, 0, 10, 5);
                instance.Obj.GetComponent<Effect>().SetKnockBack(0, 30);
                instance.StartCoroutine("FastDrawDelayed");
                pl.rigid.velocity = Vector2.zero;
                pl.rigid.velocity = new Vector2(0, -5);
                SkillOn = "canUpper";
                FastDrawDelay = true;
                StartCoroutine("nextSkill");
                StartCoroutine(attackCooling(0.4f));
                StopCoroutine("motionDelay");
                StartCoroutine(motionDelay(1.2f));

                Instantiate(Sounds[1]);
                Duration -= 5;
            }
            
            else if(Input.GetKey(KeyCode.DownArrow)) //발도(필살기)
            {
                instance.CreateEffect("FastDraw2");
                Instantiate(Sounds[2]);
                StartCoroutine(attackCooling(1.0f));
                Duration -= 50;
            }
            else if(canMagnet) //끌어오는 검
            {
                canMagnet = false;
                instance.CreateEffect("LongSlash", 0, 0, 0, 0);
                instance.Obj.transform.Rotate(0, 180, 0);
                instance.Obj.GetComponent<Effect>().Name = "RangeBlade";
                instance.Obj.transform.position = instance.sl.transform.position + new Vector3(2, 0, 0); //-10                   10
                Manager.instance.sl.GetComponent<Rigidbody2D>().velocity = new Vector2(-(Manager.instance.sl.transform.position.x - pl.transform.position.x), 0);
                StartCoroutine(attackCooling(0.5f));

                StartCoroutine(motionDelay(1.50f));
                Instantiate(Sounds[0]);
                Duration -= 2;
            }
            else if(Input.GetKey(KeyCode.UpArrow))
            {
                StartCoroutine(Senkou());
                StartCoroutine(motionDelay(2.5f));
                StartCoroutine(attackCooling(2.5f));

            }
            else if (!coolingSword)//검기
            {
                pl. GetComponent<Animator>().SetTrigger("RangeBlade");
                instance.CreateEffect("Tblade", 0, 0, 1000, 25);
                instance.Obj.transform.localScale *= 2;
                SkillOn = "canMagnet";
                StartCoroutine("nextSkill");
                StartCoroutine(attackCooling(0.5f));

                StartCoroutine(motionDelay(1.5f));

                Instantiate(Sounds[0]);
                Duration -= 3;
            }
        }
    }

    IEnumerator Senkou()
    {
        pl.GetComponent<Animator>().SetTrigger("RangeBlade");
        instance.CreateEffect("Flash");
        instance.CreateEffect("LongSlash", 0, 0, 0, 600, 150);
        Instantiate(Sounds[3]);
        yield return new WaitForSeconds(0.8f);

        pl.GetComponent<Animator>().SetTrigger("NormalUpper");
        instance.sl.rigid.isKinematic = true;

        instance.CreateEffect("Flash");

        instance.CreateEffect("LongSlash", 0, 0, 0, 0, 200);
        Instantiate(Sounds[3]);
        Vector3 v = instance.sl.transform.position - pl.body.transform.position;

        //instance.Obj.transform.Rotate(new Vector3(0, 0, 20));
        instance.Obj.transform.Rotate(new Vector3(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg));

        yield return new WaitForSeconds(1.0f);

        pl.GetComponent<Animator>().SetTrigger("AirNormal");

        instance.sl.rigid.isKinematic = false;

        instance.CreateEffect("Flash");

        instance.CreateEffect("LongSlash", -60, -60, 0, -1000, 250);
        Instantiate(Sounds[3]);
        instance.Obj.transform.position = instance.sl.transform.position;

        Duration -= 30;
    }
    public void Skill_Range()
    {
        Skill_Far();
    }
    public void Skill_Melee()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !nowAttack)
        {
            if (canUpper)
            {
                pl.nowJump = true;
                pl. GetComponent<Animator>().SetTrigger("Upper");
                attackCount = 0;
                pl.nowDash = false;
                pl.rigid.velocity = Vector2.zero;
                pl.rigid.velocity += new Vector2(0, 10);
                instance.CreateEffect("LongSlash", 85, 85, 0, 550);
                instance.Obj.transform.position += new Vector3(1, 0);
                instance.CreateEffect("Flash");
                instance.Count = 0;
                canUpper = false;
                SkillOn = "canFall";
                StartCoroutine("nextSkill");
                StartCoroutine(attackCooling(0.5f));

                StartCoroutine(motionDelay(2f));
                Duration -= 3;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && canFall)// && Input.GetKeyDown(KeyCode.D))
            {
                attackCount = 0;
                instance.rotMin = 0;
                instance.rotMax = 60;
                instance.Count = 12;
                instance.SecDelay = 0.05f;
                instance.settingEffect = "HalfSlash";
                instance.StartCoroutine("CreateEffectDelayed");
                pl.GetComponent<Animator>().SetTrigger("Fall");
                pl.rigid.velocity = Vector2.zero;
                pl.rigid.velocity = new Vector2(0, 15);

                canFall = false;
                StartCoroutine(attackCooling(1f));
                StopCoroutine("motionDelay");
                StartCoroutine(motionDelay(0.8f));
                Duration -= 8;
            }
            else if(Input.GetKey(KeyCode.UpArrow))
            {
                pl.GetComponent<Animator>().SetTrigger("NormalUpper");
                attackCount = 0;
                instance.CreateEffect("LongSlash", 85, 85, 0, 350, 15);
                instance.Obj.transform.position += new Vector3(1, 0);
                instance.CreateEffect("Flash");
                StartCoroutine(attackCooling(0.8f));
                StopCoroutine("motionDelay");
                StartCoroutine(motionDelay(0.3f));
                Duration -= 2;
            }
            else if(attackCount >= 4)
            {
                pl. GetComponent<Animator>().SetTrigger("RangeBlade");
                instance.CreateEffect("LongSlash", 0, 0, 750, 10, 25);
                instance.Obj.transform.position += new Vector3(1, 0, 0);
                
                instance.CreateEffect("Flash");
                StartCoroutine(attackCooling(1f));
                attackCount = 0;
                Tt = 0;
                StopCoroutine("motionDelay");
                StartCoroutine(motionDelay(1.2f));
                Duration -= 3;
            }
            else if (pl.nowJump)
            {
                pl.GetComponent<Animator>().SetTrigger("AirNormal");
                attackCount = 0;
                instance.CreateEffect("Short", 0, 45, 0, 45, 8);
                instance.Obj.transform.localScale = new Vector3(2f, 0.4f, 1);
                instance.Obj.transform.position += new Vector3(1, 0);
                instance.CreateEffect("Flash");
                StartCoroutine(attackCooling(0.1f));
                StopCoroutine("motionDelay");
                StartCoroutine(motionDelay(0.1f));
                Duration -= 1;
            }
            else
            {
                //pl.rigid.velocity = Vector2.zero;
                instance.CreateEffect("MoonSlash", -50, 50, 10, 100, 6);
                instance.Obj.transform.position += new Vector3(1f, 0, 0);
                StartCoroutine(attackCooling(0.1f));
                StopCoroutine("motionDelay");
                StartCoroutine(motionDelay(0.7f));
                attackCount++;
                Duration -= 1;
                if (attackCount % 2 == 0)
                    pl.GetComponent<Animator>().SetTrigger("NormalUpper");
                else
                    pl.GetComponent<Animator>().SetTrigger("RangeBlade");
            }
        }

    }

    IEnumerator nextSkill()
    {
        if (SkillOn == "canUpper")
        {
            if (FastDrawDelay)
                yield return new WaitForSeconds(0.5f);
            else
                yield return new WaitForSeconds(0.2f);

            canUpper = true;
            yield return new WaitForSeconds(0.5f);
            canUpper = false;
            FastDrawDelay = false;
        }
        if (SkillOn == "canFall")
        {
            canFall = true;
            yield return new WaitForSeconds(1.5f);
            canFall = false;
        }
        if (SkillOn == "canMagnet")
        {
            coolingSword = true;
            yield return new WaitForSeconds(1f);
            canMagnet = true;
            yield return new WaitForSeconds(0.5f);
            canMagnet = false;
            yield return new WaitForSeconds(1.0f);
            coolingSword = false;
        }
    }

    IEnumerator attackCooling(float sec)
    {
        nowAttack = true;
        yield return new WaitForSeconds(sec);
        nowAttack = false;
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
}




   
