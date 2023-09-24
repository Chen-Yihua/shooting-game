using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class script_training : MonoBehaviour
{
    [Header("�j�K�]�m")]
    public Transform shooterPoint; //�g����m
    public int range = 100; //�Z���g�{
    public float fireRate = 0.1f; //�g�t
    private float fireTimer; //�p�ɾ�
    public Transform casingPoint; //�u�ߩߥX��m
    public Transform casingPrefab; //�u�߹w����
    public GameObject pistol_muzzle; //�j�f��m

    [Header("�S�ĳ]�m")]
    public ParticleSystem muzzleFlash; //�j�f���K�S��
    public Light muzzleFlashLight; //�j�f���K�O��
    public GameObject bulletHole; //�u��

    [Header("���ĳ]�m")]
    public AudioSource audioSource1;
    public AudioClip shoot; //�g������

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
        //�˷ǰʵe
        if (Input.GetMouseButton(1)) // 1 ��ܷƹ��k��
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

        /*�}��}�l*/
        if (Input.GetMouseButton(1)) // 1 ��ܷƹ��k��
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
        //�}�j
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

        //�p���}�j�ɶ�
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }


    /*�}�j�}�l*/
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
                    GameObject bulletHoleEffect = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); //�u��
                    Destroy(bulletHoleEffect, 0.2f);
                    GameObject.Find(hit.collider.gameObject.name).GetComponent<script_target>().isHit = true;
                }
            }

            Rigidbody hitRigidbody = hit.collider.gameObject.GetComponent<Rigidbody>();
            if (hitRigidbody != null)
            {
                hitRigidbody.AddForce(transform.forward * 500);
                //Instantiate(flare, hit.point, this.transform.rotation);//�g���S��
            }
        }
        Instantiate(casingPrefab, casingPoint.transform.position, casingPoint.transform.rotation); //�u�߼u�X
        fireTimer = 0f; //���s�p�ɾ�

        muzzleFlash.Play(); //������K�S��
        muzzleFlashLight.enabled = true; //��������S��
        audioSource1.clip = shoot;
        audioSource1.Play(); //����g������
    }
    /*�}�j����*/
}

