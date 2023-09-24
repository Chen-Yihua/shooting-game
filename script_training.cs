using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class script_training : MonoBehaviour
{
    [Header("槍枝設置")]
    public Transform shooterPoint; //射擊位置
    public int range = 100; //武器射程
    public float fireRate = 0.1f; //射速
    private float fireTimer; //計時器
    public Transform casingPoint; //彈殼拋出位置
    public Transform casingPrefab; //彈殼預制體
    public GameObject pistol_muzzle; //槍口位置

    [Header("特效設置")]
    public ParticleSystem muzzleFlash; //槍口火焰特效
    public Light muzzleFlashLight; //槍口火焰燈光
    public GameObject bulletHole; //彈孔

    [Header("音效設置")]
    public AudioSource audioSource1;
    public AudioClip shoot; //射擊音效

    public Text score;
    int num;
    private Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        audioSource1 = GetComponent<AudioSource>();
        ani = GetComponent<Animator>();
        num = 0;
        score.text = " " + num;
    }

    // Update is called once per frame
    void Update()
    {
        //瞄準動畫
        if (Input.GetMouseButton(1)) // 1 表示滑鼠右鍵
        {
            ani.SetBool("isAim", true);
            if (Input.GetMouseButton(0))
            {
                ani.SetBool("isAimShoot", true);
            }
            else
            {
                ani.SetBool("isAimShoot", false);
            }
        }
        else ani.SetBool("isAim", false);

        /*開鏡開始*/
        if (Input.GetMouseButton(1)) // 1 表示滑鼠右鍵
        {
            Camera.main.fieldOfView = 25;
        }
        else
        {
            Camera.main.fieldOfView = 70;
        }

        if(Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene("game");
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("menu");
        }
    }

    void FixedUpdate()
    {
        //開槍
        if (Input.GetMouseButtonDown(0))
        {
            Gunfire();
            ani.SetBool("isFire", true);
        }
        else
        {
            muzzleFlashLight.enabled = false;
            ani.SetBool("isFire", false);
        }

        //計算能開槍時間
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }


    /*開槍開始*/
    public void Gunfire()
    {
        if (fireTimer < fireRate) return;

        RaycastHit hit;
        if (Physics.Raycast(pistol_muzzle.transform.position, -pistol_muzzle.transform.forward, out hit, range))
        {
            for (int i = 1; i < 8; i++)
            {
                if (hit.collider.gameObject.name == "Target(" + i + ")")
                {
                    num += 10;
                    score.text = " "+ num;
                    GameObject bulletHoleEffect = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); //彈孔
                    Destroy(bulletHoleEffect, 0.2f);
                    GameObject.Find(hit.collider.gameObject.name).GetComponent<script_target>().isHit = true;
                }
            }

            Rigidbody hitRigidbody = hit.collider.gameObject.GetComponent<Rigidbody>();
            if (hitRigidbody != null)
            {
                hitRigidbody.AddForce(transform.forward * 500);
                //Instantiate(flare, hit.point, this.transform.rotation);//射擊特效
            }
        }
        Instantiate(casingPrefab, casingPoint.transform.position, casingPoint.transform.rotation); //彈殼彈出
        fireTimer = 0f; //重製計時器

        muzzleFlash.Play(); //播放火焰特效
        muzzleFlashLight.enabled = true; //播放火光特效
        audioSource1.clip = shoot;
        audioSource1.Play(); //播放射擊音效
    }
    /*開槍結束*/
}

