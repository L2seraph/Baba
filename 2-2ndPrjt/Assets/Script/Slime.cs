using UnityEngine;
using System.Collections;

public class Slime : MonoBehaviour
{

    public float DamageCounter; //체력 대신
    public Rigidbody2D rigid;
    public bool Magneted;
    float duration;
    public bool nowAir;

    public float ComboDelay;
    public int ComboCounter;
    public int MaxCombo;

    public float LastDamage;

    bool firstHitByFastDraw;

    public Animator Anim;

    public bool damageAtBoom;

    public float flyGrabty;

    public static DamagePopUp Dam;

    public GameObject[] HitSound_Sword;

    private int SoundSelect;

    public int NeedCombo;
    public int NeedDamage;

    public Poison poison;

    public ComboAndDamage CD;

    // Use this for initialization
    void Start()
    {

        Anim = GetComponent<Animator>();
        firstHitByFastDraw = false;
        rigid = transform.GetComponent<Rigidbody2D>();
        DamageController.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        SoundSelect = Random.Range(0, 1);

        if (ComboCounter >= MaxCombo)
            MaxCombo = ComboCounter;

        if (nowAir)
        {
            if (ComboDelay <= 0.3f)
                ComboDelay = 0.5f;

            if (flyGrabty <= 0)
            {
                flyGrabty = 5f;
            }

            flyGrabty -= 1 * Time.deltaTime;
            if (flyGrabty <= 0)
            {
                rigid.gravityScale = 5.0f;
            }


        }
        if (ComboCounter >= 1)
        {
            if (ComboDelay <= 0)
                ComboDelay = 0.7f;

            ComboDelay -= 1 * Time.smoothDeltaTime;

            if (ComboDelay <= 0)
            {
                ComboCounter = 0;
                CD.needCombo = 0;
                Anim.SetTrigger("Drop");
            }
        }

        if (Magneted)
        {
            if (duration <= 0)
                duration = 1.5f;
            if (transform.position.x - 1.5f < Manager.instance.pl.transform.position.x)
            {
                rigid.velocity = Vector3.zero;
                Magneted = false;
            }
            duration -= Time.deltaTime;
            if (duration <= 0)
                Magneted = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Effect"))
        {
            Effect eff = coll.GetComponent<Effect>();
            if (eff.Name == "RangeBoom")
                damageAtBoom = true;
            if (eff.Name == "FastDraw2")
            {
                Anim.speed = 0f;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                transform.position += new Vector3(0, .5f, 0);
                transform.GetComponent<Rigidbody2D>().isKinematic = true;
                if (!firstHitByFastDraw)
                    StartCoroutine("FDrawDisable");

                nowAir = true;

                ComboCounter += 1;
                CD.needCombo += 1;

                ComboDelay = 1.2f;
                Instantiate(HitSound_Sword[SoundSelect]);
                DamageSet(eff);

            }
            else if (eff.Name != "RangeBlade")
                rigid.velocity = Vector2.zero;
            else
                Magneted = true;

            if (coll.GetComponent<Effect>().activated)
            {
                if (Manager.instance.pl. transform.position.x > transform.position.x)
                {
                    rigid.AddForce(new Vector2(-eff.KnockBack.x, eff.KnockBack.y));
                    coll.GetComponent<Collider2D>().enabled = false;
                }
                else
                    rigid.AddForce(eff.KnockBack);
                eff.activated = false;

                ComboCounter += 1;
                CD.needCombo += 1;

                switch (Manager.instance.pl.W_State)
                {
                    case Player.weaponState.Sword:
                        ComboDelay = 1.5f;
                        Instantiate(HitSound_Sword[SoundSelect]);
                        break;
                    case Player.weaponState.Rifle:
                        ComboDelay = 1.5f;
                        break;
                    case Player.weaponState.Shotgun:
                        ComboDelay = 1.2f;
                        break;
                }
                Anim.SetTrigger("Damaged");
                DamageSet(eff);
            }


            poison.SetBar(eff.deletePoison);


        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Wall"))
        {
            if (coll.GetComponent<Wall>().isFloor)
            {
                rigid.gravityScale = 1.0f;
                flyGrabty = 0;
            }
        }
    }

    IEnumerator FDrawDisable()
    {
        firstHitByFastDraw = true;
        yield return new WaitForSeconds(45 / 30f);
        if (GetComponent<Rigidbody2D>().isKinematic == true)
        {
            Manager.instance.CreateEffect("Flash");
            GetComponent<Rigidbody2D>().isKinematic = false;
            StopCoroutine(FDrawDisable());
            firstHitByFastDraw = false;
        }

        Anim.SetTrigger("Damaged");
        Anim.speed = 1f;
    }

    void DamageSet(Effect eff)
    {
        DamageCounter += eff.Damage * (1 + (eff.ComboMulti() / 100));
        LastDamage = eff.Damage * (1 + (eff.ComboMulti() / 100));
        CD.needDamageSet(LastDamage);
        DamageTextSet(LastDamage);
    }

    public virtual void DamageTextSet(float Damage)
    {
        Damage = Mathf.Round(Damage * 100) / 100;
        DamageController.CreateDamagePopUp(Damage.ToString(), transform);
    }

}