using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class script_enemy_raycast : MonoBehaviour
{
    public float raycast_distance = 10.0f;

    public bool CanAttack;
    public float nextToAttack;
    public float cd_time = 5.0f;

    public Transform Target;   //目標
    public Animator ani;
    public Transform hpBarbg_player; //玩家血量條背景
    public RectTransform hpBar_player; //玩家血量條 
    public GameObject hit_blood;
    public GameObject die_blood;
    private float Timer=0.0f; //計時器

    public AudioSource shoot;

    // Start is called before the first frame update
    void Start()
    {
        this.CanAttack = true;
        nextToAttack = 0.0f;
        ani.SetBool("isFire", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer >= 4f)
        {
            hit_blood.SetActive(false);
            Timer = 0;
        }
        if (hit_blood == true && Timer < 4f)
        {
            Timer += Time.deltaTime;
        }
        if (die_blood == true && hpBar_player.GetComponent<Image>().fillAmount >= 0.3f)
        {
            die_blood.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, raycast_distance))
        {
            float dis = Vector3.Distance(Target.position, this.transform.position);
            if (dis < 10.0f) //小於10敵人攻擊
            {
                float fire_Time = Time.time;
                if (hit.collider.gameObject.name == "FPSController")
                {
                    if (Time.time >= nextToAttack)
                    {
                        this.CanAttack = true;
                        ani.SetBool("isFire", true);
                        //血量減少
                        if(hpBar_player.GetComponent<Image>().fillAmount>0.3)
                        {
                            hit_blood.SetActive(true);
                        }
                        
                        if (hpBar_player.GetComponent<Image>().fillAmount <= 0.3)
                        {
                            die_blood.SetActive(true);
                        }
                        
                        hpBar_player.GetComponent<Image>().fillAmount -= 0.05f;
                        hpBar_player.sizeDelta = new Vector2(100, hpBar_player.sizeDelta.y);
                        nextToAttack = fire_Time + cd_time;

                        shoot.Play(); //播放射擊音效
                    }
                    else
                    {
                        this.CanAttack = false;
                        ani.SetBool("isFire", false);
                    }
                }
            }
        }
    }
}
