using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

    public Text Combo;
    public Text Damage;
    public int FontSize;
    public int NowCombo;
    public int NowDamage;
    
	void Start ()
    {

    }
	
	void Update ()
    {
        if (NowDamage < Manager.instance.sl.DamageCounter)
        {
            if (Manager.instance.sl.DamageCounter - NowDamage < 100)
            {
                NowDamage += 1;
            }
            else if (Manager.instance.sl.DamageCounter - NowDamage < 1000)
            {
                NowDamage += 10;
            }
            else
                NowDamage += 100;
            
            Damages(NowDamage.ToString());
        }

        if(Manager.instance.sl.ComboCounter == 0)
        {
            NowCombo = 0;
            Combo.fontSize = 55;
            Combo.text = "0";
        }

        if (NowCombo < Manager.instance.sl.ComboCounter)
        {
            NowCombo = Manager.instance.sl.ComboCounter;
            Combos(NowCombo.ToString());
        }

        if (Damage.fontSize >= 55)
            Damage.fontSize -= 1;

        if (Combo.fontSize >= 55)
            Combo.fontSize -= 1;
    }

    public void Combos(string tex)
    {
        Combo.text = tex;
        if(Combo.fontSize <= 65)
        Combo.fontSize += 5;
    }

    public void Damages(string tex)
    {
        Damage.text = tex;
        if (Damage.fontSize <= 60)
            Damage.fontSize += 5;

    }
}
