using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ATKImageController : MonoBehaviour {

    public Image WeaponImage;
    public Sprite[] WeaponSprite;
    public float Radius;
    public int NineCut;
    int BulletCounters;
    int nowBullet;
    float swordDur;

	// Use this for initialization
	void Start () {
        NineCut = 4;
	}
	
	// Update is called once per frame
	void Update ()
    {
        swordDur = Manager.instance.pl.GetComponent<Sword>().Duration;
        if (Manager.instance.pl.GetComponent<Sword>().enabled)
        {
            if (swordDur < 20)
            {
                WeaponImage.sprite = WeaponSprite[6];
            }
            else if (swordDur < 50)
            {
                WeaponImage.sprite = WeaponSprite[5];
            }
            else if (swordDur < 80)
            {
                WeaponImage.sprite = WeaponSprite[4];
            }
            else if (swordDur < 120)
            {
                WeaponImage.sprite = WeaponSprite[3];
            }
        }
	}

    public void MinusDuration()
    {
        WeaponImage.fillAmount -= Radius / 360;
        nowBullet--;
        if(nowBullet == 0)
        {
            WeaponImage.fillAmount = (float)(NineCut-1)/4;
            NineCut--;
            nowBullet = BulletCounters;
            WeaponImage.fillAmount -= Radius / 360 / 2;
        }
        
    }

    void WeaponChanged()
    {

    }

    public void ImageSet(int N)
    {
        WeaponImage.sprite = WeaponSprite[N];
        if (N == 1)
            BulletCounters = 15;
        else if (N == 2)
            BulletCounters = 4;

        WeaponImage.fillAmount = 1 - Radius / 360 / 2;

        nowBullet = BulletCounters;

        NineCut = 4;
    }
}
