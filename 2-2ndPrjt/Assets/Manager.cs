using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

    private static Manager _instance = null;
    
    public static Manager instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Instanciate fail");
            return _instance;
        }
    }

    public Player pl;
    public Slime sl;

    public GameObject FxCtrl;
    public GameObject Slash_Short;
    public GameObject Slash_Long;
    public GameObject Slash_Moon;
    public GameObject Slash_Half;
    public GameObject Flasher;
    public GameObject FastDraw;
    public GameObject FastDraw2;
    public GameObject GunShot;
    public GameObject ShotGun;
    public GameObject Pellet;
    public GameObject[] Nozzle = new GameObject[3];
    public GameObject Obj;
    public GameObject Stomper;
    public Camera cam;
    public Canvas Can;
    public Vector3 xyz = new Vector3();
    public GameObject[] TurningBlade = new GameObject[2];

    public bool doubleTap;
    public bool Tap;
    public float tapTime = 1f;
    public float duration;
    public float LastAxis;
    public float Axis;

    public int order = 0;
    public float stateChecker;

    public int BladeChanger;
    public int FlipAnim = 0;

    public string settingEffect;
    public int Count;
    public float SecDelay;
    public int rotMin;
    public int rotMax;

    public int BreakedWallCount; // 벽 파괴수

    public GameObject LeftWall;
    public GameObject RightWall;

    public GameObject[] Sfx;

    
    // Use this for initialization
    void Awake ()
    {
        sl = GameObject.Find("Slime").GetComponent<Slime>();
        //cam = Camera.main;

        DontDestroyOnLoad(this);
        _instance = this;

        
	}

    void Update()
    {
        InputManager();

        if (Tap)
        {
            duration -= tapTime * Time.deltaTime;

            if (duration <= 0)
                Tap = false;
            if (doubleTap)
            {
                if (Axis == 1)
                    pl.Dash(Axis, 10);
                else if (Axis == -1)
                    pl.Dash(Axis, 5);
                duration = 0;
                doubleTap = false;
            }
        }

        pl.Move(Axis);

        stateChecker = sl.transform.position.x - pl.transform.position.x;
        stateChecker = Mathf.Abs(stateChecker);

        if (stateChecker > 20f)
        {
            pl.State = Player.state.Far;
        }
        else if (stateChecker > 5f)
        {
            pl.State = Player.state.Range;
        }
        else
        {
            pl.State = Player.state.Melee;
        }

        if ((pl.nowDash) && Mathf.Abs(sl.transform.position.x - pl.transform.position.x) < 1.5f)
        {
            
            //pl.rigid.velocity = Vector2.zero;
            pl.nowDash = false;          
        }

    }
    //이펙트 생성/관리

    public void CreateEffect(string type, float AngleMin, float AngleMax, float KnockBackX, float KnockBackY) //타입, 로테이션, 넉백
    {
        Obj = null;
        float Rot = Random.Range(AngleMin, AngleMax);
        float Pos = Random.Range(0, 1.5f);
        
        if (type == "Short")
        {
            Obj = GameObject.Instantiate(Slash_Short, FxCtrl.transform) as GameObject;
            Obj.transform.Rotate(new Vector3(0, 0, (float)Rot));
            Obj.transform.position = new Vector2(pl. transform.position.x + Pos, pl. transform.position.y);
            if (Rot <= 0)
                Obj.GetComponent<SpriteRenderer>().flipY = true;

        }
        else if (type == "LongSlash")
        {
            Obj = GameObject.Instantiate(Slash_Long, FxCtrl.transform) as GameObject;

            Obj.transform.Rotate(new Vector3(0, 0, (float)Rot));
            Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);

            //CreateEffect("Tblade", Rot, Rot);
        }
        else if (type == "Flash")
        {
            Obj = GameObject.Instantiate(Flasher, FxCtrl.transform) as GameObject;
            //Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);
            Obj.GetComponent<Animator>().SetTrigger("Activate");
        }
        else if (type == "Tblade")
        {
            Obj = GameObject.Instantiate(TurningBlade[BladeChanger], FxCtrl.transform) as GameObject;
            Obj.transform.Rotate(new Vector3(0, 0, (float)Rot));
            Obj.GetComponent<Animator>().SetTrigger("Activate");
            Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);
            BladeChanger++;
            if (BladeChanger >= 2)
                BladeChanger = 1;
        }
        else if (type == "MoonSlash")
        {
            Obj = GameObject.Instantiate(Slash_Moon, FxCtrl.transform) as GameObject;

            Obj.transform.Rotate(new Vector3(0, 0, (float)Rot));
            Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);
            //CreateEffect("Flash", 0, 0);
            //CreateEffect("Tblade", Rot, Rot);
            if (FlipAnim == 1)
                Obj.GetComponent<SpriteRenderer>().flipY = true;
            FlipAnim++;
            if (FlipAnim >= 2)
                FlipAnim = 0;

        }
        else if (type == "HalfSlash")
        {
            Obj = GameObject.Instantiate(Slash_Half, FxCtrl.transform) as GameObject;

            Obj.transform.Rotate(new Vector3(0, 0, (float)Rot));
            Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);
            //CreateEffect("Flash", 0, 0);
            //CreateEffect("Tblade", Rot, Rot);
            if (FlipAnim == 1)
                Obj.GetComponent<SpriteRenderer>().flipX = true;
            FlipAnim++;
            if (FlipAnim >= 2)
                FlipAnim = 0;

        }
        else if (type == "FastDraw")
        {
            Obj = GameObject.Instantiate(FastDraw, FxCtrl.transform) as GameObject;

            Obj.transform.Rotate(new Vector3(0, 0, (float)Rot));
            Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);
        }

        if (type == "ShotGun3")
        {
            Vector3 v = sl.transform.position - pl.transform.position;
            float zOffset = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            Obj = GameObject.Instantiate(ShotGun, FxCtrl.transform) as GameObject;
            Obj.transform.position = new Vector2(pl. transform.position.x + 1.5f, pl. transform.position.y - 0.5f);
            Obj.transform.Rotate(new Vector3(0, 0, zOffset+AngleMax));
        }
        if (type == "Shot")
        {
            Obj = GameObject.Instantiate(GunShot, FxCtrl.transform) as GameObject;
            Vector3 v = sl.transform.position - pl.transform.position;
            Obj.transform.position = new Vector2(pl. transform.position.x + 1.5f, pl. transform.position.y - 0.5f);
            Obj.transform.Rotate(new Vector3(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg + Random.Range(-AngleMax, AngleMax)));
        }
        if (pl.transform.position.x > sl.transform.position.x && Obj != null)
            Obj.transform.Rotate(xyz);
        Obj.GetComponent<SpriteRenderer>().sortingOrder = order;
        order++;
        Obj.GetComponent<Effect>().SetKnockBack((int)KnockBackX, (int)KnockBackY);
        
    }

    public void CreateEffect(string type, float AngleMin, float AngleMax, float KnockBackX, float KnockBackY, int Damage) //타입, 로테이션, 넉백
    {
        Obj = null;
        float Rot = Random.Range(AngleMin, AngleMax);
        float Pos = Random.Range(0, 1.5f);
        if (type == "Short")
        {
            Obj = GameObject.Instantiate(Slash_Short, FxCtrl.transform) as GameObject;
            Obj.transform.Rotate(new Vector3(0, 0, (float)Rot));
            Obj.transform.position = new Vector2(pl. transform.position.x + Pos, pl. transform.position.y);
            Obj.GetComponent<Animator>().SetTrigger("Activate_Cresent");
            if (Rot <= 0)
                Obj.GetComponent<SpriteRenderer>().flipY = true;

        }
        else if (type == "LongSlash")
        {
            Obj = GameObject.Instantiate(Slash_Long, FxCtrl.transform) as GameObject;

            Obj.transform.Rotate(new Vector3(0, 0, (float)Rot));
            Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);
            //CreateEffect("Tblade", Rot, Rot);
        }
        else if (type == "Flash")
        {
            Obj = GameObject.Instantiate(Flasher, FxCtrl.transform) as GameObject;
            //Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);
            Obj.GetComponent<Animator>().SetTrigger("Activate");
        }
        else if (type == "Tblade")
        {
            Obj = GameObject.Instantiate(TurningBlade[BladeChanger], FxCtrl.transform) as GameObject;
            Obj.transform.Rotate(new Vector3(0, 0, (float)Rot));
            Obj.GetComponent<Animator>().SetTrigger("Activate");
            Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);
            BladeChanger++;
            if (BladeChanger >= 2)
                BladeChanger = 1;
        }
        else if (type == "MoonSlash")
        {
            Obj = GameObject.Instantiate(Slash_Moon, FxCtrl.transform) as GameObject;

            Obj.transform.Rotate(new Vector3(0, 0, (float)Rot));
            Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);
            //CreateEffect("Flash", 0, 0);
            //CreateEffect("Tblade", Rot, Rot);
            if (FlipAnim == 1)
                Obj.GetComponent<SpriteRenderer>().flipY = true;
            FlipAnim++;
            if (FlipAnim >= 2)
                FlipAnim = 0;

        }
        else if (type == "HalfSlash")
        {
            Obj = GameObject.Instantiate(Slash_Half, FxCtrl.transform) as GameObject;

            Obj.transform.Rotate(new Vector3(0, 0, (float)Rot));
            Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);
            //CreateEffect("Flash", 0, 0);
            //CreateEffect("Tblade", Rot, Rot);
            if (FlipAnim == 1)
                Obj.GetComponent<SpriteRenderer>().flipX = true;
            FlipAnim++;
            if (FlipAnim >= 2)
                FlipAnim = 0;

        }
        else if (type == "FastDraw")
        {
            Obj = GameObject.Instantiate(FastDraw, FxCtrl.transform) as GameObject;

            Obj.transform.Rotate(new Vector3(0, 0, (float)Rot));
            Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);
        }

        if (type == "ShotGun3")
        {
            Vector3 v = sl.transform.position - pl.transform.position;
            float zOffset = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            Obj = GameObject.Instantiate(ShotGun, FxCtrl.transform) as GameObject;
            Obj.transform.position = new Vector2(pl. transform.position.x + 1.5f, pl. transform.position.y - 0.5f);
            Obj.transform.Rotate(new Vector3(0, 0, zOffset + AngleMax));
        }
        if (type == "Shot")
        {
            Obj = GameObject.Instantiate(GunShot, FxCtrl.transform) as GameObject;
            Vector3 v = sl.transform.position - pl.transform.position;
            Obj.transform.position = new Vector2(pl. transform.position.x + 1.5f, pl. transform.position.y - 0.5f);
            Obj.transform.Rotate(new Vector3(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg + Random.Range(-AngleMax, AngleMax)));
        }
        if (pl.transform.position.x > sl.transform.position.x && Obj != null)
            Obj.transform.Rotate(xyz);
        Obj.GetComponent<SpriteRenderer>().sortingOrder = order;
        order++;
        Obj.GetComponent<Effect>().SetKnockBack((int)KnockBackX, (int)KnockBackY);
        Obj.GetComponent<Effect>().Damage = Damage;

    }


    public void CreateEffect(string type, int KnockBackX, int KnockBackY) //타입, 넉백만 넣어도 되는것.
    {
        Obj = null;
        if (type == "Shot")
        {
            Obj = GameObject.Instantiate(GunShot, FxCtrl.transform) as GameObject;
            Vector3 v = sl.transform.position - pl.transform.position;
            Obj.transform.position = new Vector2(pl. transform.position.x + 1.5f, pl. transform.position.y -0.5f);
            Obj.transform.Rotate(new Vector3(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg + Random.Range(-5.5f,5.5f)));
        }
        if(type == "ShotAim")
        {
            Obj = GameObject.Instantiate(GunShot, FxCtrl.transform) as GameObject;
            Vector3 v = sl.transform.position - pl.transform.position;
            Obj.transform.position = new Vector2(pl. transform.position.x + 1.5f, pl. transform.position.y - 0.5f);
            Obj.transform.Rotate(new Vector3(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg));

        }
        if(type == "ShotGun")
        {
            Obj = GameObject.Instantiate(ShotGun, FxCtrl.transform) as GameObject;
            Vector3 v = sl.transform.position - pl.transform.position;
            Obj.transform.position = new Vector2(pl. transform.position.x + 1.5f, pl. transform.position.y - 0.5f);
            Obj.transform.Rotate(new Vector3(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg));
        }

        Obj.GetComponent<SpriteRenderer>().sortingOrder = order;
        order++;
        Obj.GetComponent<Effect>().SetKnockBack((int)KnockBackX, (int)KnockBackY);
    }

    public void CreateEffect(string type) //타입만 넣어도 되는것. 넉백치 고정/위치 고정 or 고정위치 랜덤값
    {
        Obj = null;
        if(type == "Stomp")
        {
            Obj = GameObject.Instantiate(Stomper, FxCtrl.transform) as GameObject;
            Obj.transform.position = sl.transform.position;
        }
        if (type == "Flash")
        {
            Obj = GameObject.Instantiate(Flasher, cam.transform) as GameObject;
            //Obj.transform.position = new Vector2(pl. transform.position.x, pl. transform.position.y);
            Obj.transform.localPosition = new Vector3(0, 0, 5);
            Obj.GetComponent<Animator>().SetTrigger("Activate");
            Obj.transform.localScale = new Vector3(cam.orthographicSize / 2 * (Screen.width / Screen.height), cam.orthographicSize / 2, 0f);
        }
        else if (type == "FastDraw2")
        {
            Obj = GameObject.Instantiate(FastDraw2, cam.transform) as GameObject;
            Obj.GetComponent<SpriteRenderer>().sortingOrder = order;
            order++;
            Obj.transform.localPosition = new Vector3(0, 0, 10);
            Obj.transform.localScale = new Vector3(2.5f, 2.5f, 1);
            Obj.GetComponent<Effect>().SetKnockBack(0, 1000);
        }
        else if (type == "NozzleFlash")
        {
            Obj = GameObject.Instantiate(Nozzle[0], FxCtrl.transform) as GameObject;
            Vector3 v = sl.transform.position - pl.transform.position;
            Obj.transform.position = new Vector2(pl. transform.position.x + 1.5f, pl. transform.position.y-0.5f);
            Obj.transform.Rotate(new Vector3(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg));
        }
        else if (type == "NozzleFlash2")
        {
            Obj = GameObject.Instantiate(Nozzle[1], FxCtrl.transform) as GameObject;
            Vector3 v = sl.transform.position - pl.transform.position;
            Obj.transform.position = new Vector2(pl. transform.position.x + 1.5f, pl. transform.position.y - 0.5f);
            Obj.transform.Rotate(new Vector3(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg));
        }
        else if (type == "NozzleFlash3")
        {
            Obj = GameObject.Instantiate(Nozzle[2], FxCtrl.transform) as GameObject;
            Vector3 v = sl.transform.position - pl.transform.position;
            Obj.transform.position = new Vector2(pl. transform.position.x + 1.5f, pl. transform.position.y - 0.5f);
            Obj.transform.Rotate(new Vector3(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg));
        }
        else if (type =="Pellet")
        {
            Obj = GameObject.Instantiate(Pellet, FxCtrl.transform) as GameObject;
            Obj.transform.position = new Vector2(sl.transform.position.x + Random.Range(-3,5f), sl.transform.position.y + Random.Range(-4,4));
            Obj.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
        }

        order++;
    }

    public void AnimateCharacter()
    {

    }

    public void InputManager()
    {

        //기본적인 이동
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (LastAxis == 1)
            {
                duration = 0;
                Tap = false;
            }

            LastAxis = -1;
            if (Tap)
            {
                doubleTap = true;
            }
            else
            {
                Tap = true;
                duration = 0.5f;
            }

            Axis = -1;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (LastAxis == -1 && Axis == - 1)
            Axis = 0;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (LastAxis == -1)
            {
                duration = 0;
                Tap = false;
            }
            LastAxis = 1;
            if (Tap)
            {
                doubleTap = true;
            }
            else
            {
                Tap = true;
                duration = 0.5f;
            }
            Axis = 1;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (LastAxis == 1 && Axis == 1)
                Axis = 0;
        }

    }


    IEnumerator CreateEffectDelayed()
    {
        while(Count >= 1)
        {
            CreateEffect(settingEffect, rotMin, rotMax, 5, -30);
            pl.rigid.velocity += new Vector2(0, -4);
            Count--;       
            yield return new WaitForSeconds(SecDelay);
        }
       //sl.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //Obj.transform.position = sl.transform.position;
        Obj.GetComponent<Effect>().SetKnockBack(0, 300);
        CreateEffect("Flash");
        pl.Dash(-1, 5);
    }

    IEnumerator FastDrawDelayed()
    {
        pl.rigid.AddForce(new Vector2(0, 500f));

        yield return new WaitForSeconds(SecDelay * 2);
        while (Count >= 1)
        {
            pl. transform.position = (Vector2)sl.transform.position + new Vector2(1.5f, 0);
            CreateEffect(settingEffect, rotMin, rotMax, 30,50);
            sl.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Obj.GetComponent<Effect>().SetKnockBack(30, 50);
            Count--;
            yield return new WaitForSeconds(SecDelay);
            pl. transform.position = (Vector2)sl.transform.position + new Vector2(-1.5f, 0);
            CreateEffect(settingEffect, rotMin, rotMax, 30, 50);
            Obj.GetComponent<Effect>().SetKnockBack(30, 50);
            yield return new WaitForSeconds(SecDelay);
        }
        pl.Dash(-1, 5);
    }



    
}
