using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Poison : MonoBehaviour
{
    public Image Bar;

    float NowBar;
    float MaxBar;

    float TimeLimit;

    // Use this for initialization
    void Start()
    {
        MaxBar = 60;
    }

    // Update is called once per frame
    void Update()
    {
        TimeLimit += 1 * Time.deltaTime;

        if(TimeLimit <= 30)
        {
            NowBar += 2 * Time.deltaTime;
        }
        if(TimeLimit >= 30)
        {
            NowBar += 4 * Time.deltaTime;
        }
        else if(TimeLimit >= 60)
        {
            NowBar += 12f * Time.deltaTime;
        }


        else if (Input.GetKey(KeyCode.T))
            Bar.fillAmount += 0.1f;

        Bar.fillAmount = (NowBar / MaxBar);
        if (Manager.instance.pl.WeaponStock < 0 || Bar.fillAmount == 1)
            SceneManager.LoadScene(2);


        if (Input.GetKey(KeyCode.R))
        {
            NowBar -= 1;

            if (NowBar < 0)
                NowBar = 0;
        }
        if(Input.GetKey(KeyCode.V))
        {
            Manager.instance.pl.WeaponStock += 1;
        }
    }

    public void SetBar(float N)
    {
        NowBar -= N;
        if (NowBar < 0)
            NowBar = 0;
    }

    public void PlusMaxBar()
    {
        TimeLimit = (TimeLimit * 0.3f) - 10;
        if (TimeLimit < 0)
            TimeLimit = 0;
        MaxBar += 60;
    }
}
