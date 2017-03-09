using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboAndDamage : MonoBehaviour {

    public bool isCombo;
    public bool isDamage;
    public int ComboLevel;
    public int ComboRate;
    public int DamageLevel;
    public float DamageRate;
    public Image Bar;
    public float needDamage;
    public float needCombo;
    Slime sl;
    Player pl;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {

        if (ComboLevel <= 0)
            ComboLevel = 0;
        if (DamageLevel <= 0)
            DamageLevel = 0;

        if(sl == null)
        sl = Manager.instance.sl;
        if(pl == null)
        pl = Manager.instance.pl;

        if (isCombo)
        {
            SetUpCombo();
           

            switch (ComboLevel)
            {
                case 0:
                    ComboRate = 50;
                    break;
                case 1:
                    ComboRate = 100;
                    break;
                case 2:
                    ComboRate = 150;
                    break;
                case 3:
                    ComboRate = 200;
                    break;
                case 4:
                    ComboRate = 300;
                    break;
                case 5:
                    ComboRate = 500;
                    break;
                case 6:
                    ComboRate = 700;
                    break;
                case 7:
                    ComboRate = 1000;
                    break;
                default:
                    ComboRate = 1500;
                    break;
            }
            if (Manager.instance.sl.MaxCombo >= ComboRate)
            {
                ComboLevel++;
                pl.WeaponStock++;
            }
        }
        else if (isDamage)
        {
            SetUpDamage();
           

            switch (DamageLevel)
            {
                case 0:
                    DamageRate = 1000;
                    break;
                case 1:
                    DamageRate = 3000;
                    break;
                case 2:
                    DamageRate = 7000;
                    break;
                case 3:
                    DamageRate = 12000;
                    break;
                case 4:
                    DamageRate = 30000;
                    break;
                case 5:
                    DamageRate = 40000;
                    break;
                case 6:
                    DamageRate = 150000;
                    break;
                case 7:
                    DamageRate = 500000;
                    break;
                default:
                    DamageRate = 1000000;
                    break;
            }
            if (needDamage >= DamageRate)
            {
                needDamage -= DamageRate;
                DamageLevel++;
                pl.WeaponStock++;
            }
        }
    }

    void SetUpCombo()
    {
        ComboCheck(Manager.instance.sl.MaxCombo);
        Vector2 screenPos = Manager.instance.cam.WorldToScreenPoint(sl.transform.position);
        transform.SetParent(Manager.instance.Can.transform, false);
        transform.position = screenPos + (Vector2.up * 60);
    }

    void SetUpDamage()
    {
        DamageCheck();
        Vector2 screenPos = Manager.instance.cam.WorldToScreenPoint(sl.transform.position);
        transform.SetParent(Manager.instance.Can.transform, false);
        transform.position = screenPos + (Vector2.up * 40);
    }

    void ComboCheck(float m_Combo)
    {
        Bar.fillAmount = m_Combo / ComboRate;    
    }
    public void needDamageSet(float D)
    {
        needDamage += D;
    }
    void DamageCheck()
    {
        Bar.fillAmount = needDamage / DamageRate;        
    }
}
