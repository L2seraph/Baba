using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour {

    private static GameObject C;
    private static DamagePopUp Dam;
    
    public static void Initialize()
    {
        C = GameObject.Find("Canvas");
        if(!Dam)
        Dam = Resources.Load<DamagePopUp>("Prefabs/DamagePopupParent");
    }

    public static void CreateDamagePopUp(string T, Transform location)
    {
        DamagePopUp ins = Instantiate(Dam);
        Vector2 screenPos = Manager.instance.cam.WorldToScreenPoint(location.position);
        ins.transform.SetParent(C.transform, false);
        ins.transform.position = screenPos + (Vector2.up * 120);
        ins.DamageText(T);
    }
}

