using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class script_shotgun : MonoBehaviour
{
    [Header("�ĤH����]�m")]
    int j;
    public Transform hpBarbg_enemy1; //�ĤH��q���I��
    public RectTransform hpBar_enemy1; //�ĤH��q��
    public Transform hpBarbg_enemy2; 
    public RectTransform hpBar_enemy2; 
    public Transform hpBarbg_enemy3; 
    public RectTransform hpBar_enemy3; 
    public Transform hpBarbg_enemy4; 
    public RectTransform hpBar_enemy4; 

    public Transform hpBarbg_e1; //�ĤH��q���I��
    public RectTransform hpBar_e1; //�ĤH��q��
    public Transform hpBarbg_e2; 
    public RectTransform hpBar_e2; 
    public Transform hpBarbg_e3; 
    public RectTransform hpBar_e3; 
    public Transform hpBarbg_e4; 
    public RectTransform hpBar_e4; 
    public Transform hpBarbg_e5; 
    public RectTransform hpBar_e5; 

    [Header("���a����]�m")]
    public Transform hpBarbg_player; //���a��q���I��
    public RectTransform hpBar_player; //���a��q��

    [Header("�j�K�]�m")]
    public Transform shooterPoint; //�g����m
    public int bulletsMag = 30; //�@�Ӽu�X���l�u��
    public int range = 100; //�Z���g�{
    static public int bulletLeft = 60; //�Ƽu
    public int currentBullets; //��e�l�u��
    public float fireRate = 0.1f; //�g�t
    private float fireTimer; //�p�ɾ�
    public GameObject pistol_muzzle; //�j�f��m
    public Transform casingPoint; //�u�ߩߥX��m
    public Transform casingPrefab; //�u�߹w����

    [Header("�_�c�]�m")]
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

    [Header("UI�]�m")]
    public Text AmmoTextUI;
    public Text bulletsMagTextUI1;
    public Text bulletsMagTextUI2;
    public Text bulletsMagTextUI3;
    public Text bloodTextUI;

    [Header("�S�ĳ]�m")]
    public ParticleSystem muzzleFlash; //�j�f���K�S��
    public Light muzzleFlashLight; //�j�f���K�O��
    public GameObject bulletHole; //�u��
    public GameObject bulletHoleEnemy;

    [Header("���ĳ]�m")]
    public AudioSource audioSource;
    public AudioClip clip; //�g������
    public AudioClip loadedbullet;

    private Animator ani;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ani = GetComponent<Animator>();
        currentBullets = bulletsMag;
        treasure_array = new GameObject[] { blood, bullet_pistol, bullet_shotgun, bullet_smg }; //�_�c�]�m
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmoUI();

        //�b�]�ʵe
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ani.SetBool("isRun", true);
        }
        else ani.SetBool("isRun", false);

        //�˷ǰʵe
        if (Input.GetMouseButton(1)) // 1 ��ܷƹ��k��
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

        //���_�c���~
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
        //�}�j
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

        //�ˤl�u
        if (Input.GetKeyDown(KeyCode.R) && currentBullets < bulletsMag && bulletLeft > 0)
        {
            ReloadBullet();
            ani.SetBool("isReload", true);
        }
        else ani.SetBool("isReload", false);

        //�p���}�j�ɶ�
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }




    /*�}�j�}�l*/
    public void Gunfire()
    {
        if (fireTimer < fireRate || currentBullets <= 0) return;

        RaycastHit hit;

        Vector3 shootDirection = shooterPoint.forward; //�g����V
        if (Physics.Raycast(pistol_muzzle.transform.position, -pistol_muzzle.transform.forward, out hit, range))
        {
            for (j = 1; j < 6; j++)
            {
                if (hit.collider.gameObject.name == "enemy2(" + j + ")" || hit.collider.gameObject.name == "e1(" + j + ")")
                {
                    GameObject bulletHoleEffect = Instantiate(bulletHoleEnemy, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); //�u��
                    Destroy(bulletHoleEffect, 0.7f);
                }
                else
                {
                    GameObject bulletHoleEffect = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); //�u��
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
                //��q���
                hpBar_enemy1.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_enemy1.sizeDelta = new Vector2(100, hpBar_enemy1.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "enemy2(2)")
            {
                //��q���
                hpBar_enemy2.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_enemy2.sizeDelta = new Vector2(100, hpBar_enemy2.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "enemy2(3)")
            {
                //��q���
                hpBar_enemy3.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_enemy3.sizeDelta = new Vector2(100, hpBar_enemy3.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "enemy2(4)")
            {
                //��q���
                hpBar_enemy4.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_enemy4.sizeDelta = new Vector2(100, hpBar_enemy4.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "e1(1)")
            {
                //��q���
                hpBar_e1.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_e1.sizeDelta = new Vector2(100, hpBar_e1.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "e1(2)")
            {
                //��q���
                hpBar_e2.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_e2.sizeDelta = new Vector2(100, hpBar_e2.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "e1(3)")
            {
                //��q���
                hpBar_e3.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_e3.sizeDelta = new Vector2(100, hpBar_e3.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "e1(4)")
            {
                //��q���
                hpBar_e4.GetComponent<Image>().fillAmount -= 0.1f;
                hpBar_e4.sizeDelta = new Vector2(100, hpBar_e4.sizeDelta.y);
            }
            if (hit.collider.gameObject.name == "e1(5)")
            {
                //��q���
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
        Instantiate(casingPrefab, casingPoint.transform.position, casingPoint.transform.rotation); //�u�߼u�X
        currentBullets--; //�l�u��@
        UpdateAmmoUI(); //��s�l�u�Ƥ�r
        fireTimer = 0f; //���s�p�ɾ�

        muzzleFlash.Play(); //������K�S��
        muzzleFlashLight.enabled = true; //��������S��
        audioSource.clip = clip;
        audioSource.Play(); //����g������
    }
    /*�}�j����*/

    public void UpdateAmmoUI()
    {
        AmmoTextUI.text = currentBullets + "/" + bulletsMag;
        bulletsMagTextUI2.text = " " + bulletLeft;
    }

    /*�ˤl�u�}�l*/
    public void ReloadBullet()
    {
        audioSource.clip = loadedbullet;
        audioSource.Play();
        //�p��ݭn�j�K�n�ɪ��l�u��
        int bullectToLoad = bulletsMag - currentBullets;
        //�p��Ƽu�n�����l�u��
        int bullectToReduce = (bulletLeft >= bullectToLoad) ? bullectToLoad : bulletLeft;
        bulletLeft = bulletLeft - bullectToReduce;
        currentBullets = currentBullets + bullectToReduce;
        UpdateAmmoUI();
    }
    /*�ˤl�u����*/
}