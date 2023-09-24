using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class script_shotgun : MonoBehaviour
{
    [Header("敵人血條設置")]
    int j;
    public Transform hpBarbg_enemy1; //敵人血量條背景
    public RectTransform hpBar_enemy1; //敵人血量條
    public Transform hpBarbg_enemy2; 
    public RectTransform hpBar_enemy2; 
    public Transform hpBarbg_enemy3; 
    public RectTransform hpBar_enemy3; 
    public Transform hpBarbg_enemy4; 
    public RectTransform hpBar_enemy4; 

    public Transform hpBarbg_e1; //敵人血量條背景
    public RectTransform hpBar_e1; //敵人血量條
    public Transform hpBarbg_e2; 
    public RectTransform hpBar_e2; 
    public Transform hpBarbg_e3; 
    public RectTransform hpBar_e3; 
    public Transform hpBarbg_e4; 
    public RectTransform hpBar_e4; 
    public Transform hpBarbg_e5; 
    public RectTransform hpBar_e5; 

    [Header("玩家血條設置")]
    public Transform hpBarbg_player; //玩家血量條背景
    public RectTransform hpBar_player; //玩家血量條

    [Header("槍枝設置")]
    public Transform shooterPoint; //射擊位置
    public int bulletsMag = 30; //一個彈匣的子彈數
    public int range = 100; //武器射程
    static public int bulletLeft = 60; //備彈
    public int currentBullets; //當前子彈數
    public float fireRate = 0.1f; //射速
    private float fireTimer; //計時器
    public GameObject pistol_muzzle; //槍口位置
    public Transform casingPoint; //彈殼拋出位置
    public Transform casingPrefab; //彈殼預制體

    [Header("寶箱設置")]
    public GameObject bullet_pistol;
    public GameObject bullet_shotgun;
    public GameObject bullet_smg;
    public GameObject treasure;
    public GameObject treasure1;
    public GameObject treasure2;
    public GameObject treasure3;
    public GameObject blood;
    GameObject[] treasure_array;
    int i;

    [Header("UI設置")]
    public Text AmmoTextUI;
    public Text bulletsMagTextUI1;
    public Text bulletsMagTextUI2;
    public Text bulletsMagTextUI3;
    public Text bloodTextUI;

    [Header("特效設置")]
    public ParticleSystem muzzleFlash; //槍口火焰特效
    public Light muzzleFlashLight; //槍口火焰燈光
    public GameObject bulletHole; //彈孔
    public GameObject bulletHoleEnemy;

    [Header("音效設置")]
    public AudioSource audioSource;
    public AudioClip clip; //射擊音效
    public AudioClip loadedbullet;

    private Animator ani;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ani = GetComponent<Animator>();
        currentBullets = bulletsMag;
        treasure_array = new GameObject[] { blood, bullet_pistol, bullet_shotgun, bullet_smg }; //寶箱設置
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmoUI();

        //奔跑動畫
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ani.SetBool("isRun", true);
        }
        else ani.SetBool("isRun", false);

        //瞄準動畫
        if (Input.GetMouseButton(1)) // 1 表示滑鼠右鍵
        {
            ani.SetBool("isAim", true);
            if (Input.GetKey(KeyCode.Mouse0))
            {
                ani.SetBool("isAimShoot", true);
                Debug.Log("123");
            }
            else ani.SetBool("isAimShoot", false);
        }
        else ani.SetBool("isAim", false);

        //拿寶箱物品
        RaycastHit hitTreasure;

        if (Physics.Raycast(pistol_muzzle.transform.position, -pistol_muzzle.transform.forward, out hitTreasure, 2.0f))
        {
            if (hitTreasure.collider.gameObject.name == "bullet_pistol(Clone)")
            {
                if (Input.GetKey(KeyCode.E))
                {
                    if ((script_pistol.bulletLeft + 10) <= 300)
                    {
                        script_pistol.bulletLeft += 10;
                    }
                    else script_pistol.bulletLeft = 300;
                    bulletsMagTextUI1.text = " " + script_pistol.bulletLeft;
                    Destroy(GameObject.Find("bullet_pistol(Clone)"));
                }
            }

            if (hitTreasure.collider.gameObject.name == "bullet_shotgun(Clone)")
            {
                if (Input.GetKey(KeyCode.E))
                {
                    if ((bulletLeft + 10) <= 300)
                    {
                        bulletLeft += 10;
                    }
                    else bulletLeft = 300;
                    UpdateAmmoUI();
                    Destroy(GameObject.Find("bullet_shotgun(Clone)"));
                }
            }

            if (hitTreasure.collider.gameObject.name == "bullet_smg(Clone)")
            {
                if (Input.GetKey(KeyCode.E))
                {
                    if ((script_smg.bulletLeft + 10) <= 300)
                    {
                        script_smg.bulletLeft += 10;
                    }
                    else script_smg.bulletLeft = 300;
                    bulletsMagTextUI3.text = " " + script_smg.bulletLeft;
                    Destroy(GameObject.Find("bullet_smg(Clone)"));
                }
            }

            if (hitTreasure.collider.gameObject.name == "blood(Clone)")
            {
                if (Input.GetKey(KeyCode.E))
                {
                    script_player.aid += 1;
                    bloodTextUI.text = " " + script_player.aid;
                    Destroy(GameObject.Find("blood(Clone)"));
                }
            }
        }
    }

    void FixedUpdate()
    {
        //開槍
        if (Input.GetKey(KeyCode.Mouse0) && currentBullets > 0)
        {
            Gunfire();
            ani.SetBool("isFire", true);
        }
        else
        {
            muzzleFlashLight.enabled = false;
            ani.SetBool("isFire", false);
        }

        //裝子彈
        if (Input.GetKeyDown(KeyCode.R) && currentBullets < bulletsMag && bulletLeft > 0)
        {
            ReloadBullet();
            ani.SetBool("isReload", true);
        }
        else ani.SetBool("isReload", false);

        //計算能開槍時間
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }




    /*開槍開始*/
    public void Gunfire()
    {
        if (fireTimer < fireRate || currentBullets <= 0) return;

        RaycastHit hit;

        Vector3 shootDirection = shooterPoint.forward; //射擊方向
        if (Physics.Raycast(pistol_muzzle.transform.position, -pistol_muzzle.transform.forward, out hit, range))
        {
            for (j = 1; j < 6; j++)
            {
                if (hit.collider.gameObject.name == "enemy2(" + j + ")" || hit.collider.gameObject.name == "e1(" + j + ")")
                {
                    GameObject bulletHoleEffect = Instantiate(bulletHoleEnemy, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); //彈孔
                    Destroy(bulletHoleEffect, 0.7f);
                }
                else
                {
                    GameObject bulletHoleEffect = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); //彈孔
                    Destroy(bulletHoleEffect, 0.7f);
                }
            }

            Rigidbody hitRigidbody = hit.collider.gameObject.GetComponent<Rigidbody>();
            if (hitRigidbody != null)
            {
                hitRigidbody.AddForce(transform.forward * 500);
            }

            if (hit.collider.gameObject.name == "enemy2(1)")
            {
                //血量減少
                hpBar_enemy1.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_enemy1.sizeDelta = new Vector2(100, hpBar_enemy1.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "enemy2(2)")
            {
                //血量減少
                hpBar_enemy2.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_enemy2.sizeDelta = new Vector2(100, hpBar_enemy2.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "enemy2(3)")
            {
                //血量減少
                hpBar_enemy3.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_enemy3.sizeDelta = new Vector2(100, hpBar_enemy3.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "enemy2(4)")
            {
                //血量減少
                hpBar_enemy4.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_enemy4.sizeDelta = new Vector2(100, hpBar_enemy4.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "e1(1)")
            {
                //血量減少
                hpBar_e1.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_e1.sizeDelta = new Vector2(100, hpBar_e1.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "e1(2)")
            {
                //血量減少
                hpBar_e2.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_e2.sizeDelta = new Vector2(100, hpBar_e2.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "e1(3)")
            {
                //血量減少
                hpBar_e3.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_e3.sizeDelta = new Vector2(100, hpBar_e3.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "e1(4)")
            {
                //血量減少
                hpBar_e4.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_e4.sizeDelta = new Vector2(100, hpBar_e4.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "e1(5)")
            {
                //血量減少
                hpBar_e5.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_e1.sizeDelta = new Vector2(100, hpBar_e5.sizeDelta.y);
            }

            if (hit.collider.gameObject.name == "treasure")
            {
                i = Random.Range(0, 4);
                Destroy(treasure.gameObject);
                Instantiate(treasure_array[i], treasure.transform.position, treasure.transform.rotation);
            }
            if (hit.collider.gameObject.name == "treasure (1)")
            {
                i = Random.Range(0, 4);
                Destroy(treasure1.gameObject);
                Instantiate(treasure_array[i], treasure1.transform.position, treasure1.transform.rotation);
            }
            if (hit.collider.gameObject.name == "treasure (2)")
            {
                i = Random.Range(0, 4);
                Destroy(treasure2.gameObject);
                Instantiate(treasure_array[i], treasure2.transform.position, treasure2.transform.rotation);
            }
            if (hit.collider.gameObject.name == "treasure (3)")
            {
                i = Random.Range(0, 4);
                Destroy(treasure3.gameObject);
                Instantiate(treasure_array[i], treasure3.transform.position, treasure3.transform.rotation);
            }
        }
        Instantiate(casingPrefab, casingPoint.transform.position, casingPoint.transform.rotation); //彈殼彈出
        currentBullets--; //子彈減一
        UpdateAmmoUI(); //更新子彈數文字
        fireTimer = 0f; //重製計時器

        muzzleFlash.Play(); //播放火焰特效
        muzzleFlashLight.enabled = true; //播放火光特效
        audioSource.clip = clip;
        audioSource.Play(); //播放射擊音效
    }
    /*開槍結束*/

    public void UpdateAmmoUI()
    {
        AmmoTextUI.text = currentBullets + "/" + bulletsMag;
        bulletsMagTextUI2.text = " " + bulletLeft;
    }

    /*裝子彈開始*/
    public void ReloadBullet()
    {
        audioSource.clip = loadedbullet;
        audioSource.Play();
        //計算需要槍枝要補的子彈數
        int bullectToLoad = bulletsMag - currentBullets;
        //計算備彈要扣的子彈數
        int bullectToReduce = (bulletLeft >= bullectToLoad) ? bullectToLoad : bulletLeft;
        bulletLeft = bulletLeft - bullectToReduce;
        currentBullets = currentBullets + bullectToReduce;
        UpdateAmmoUI();
    }
    /*裝子彈結束*/
}