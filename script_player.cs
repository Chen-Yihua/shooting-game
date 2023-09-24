using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class script_player : MonoBehaviour
{
    public GameObject[] guns;   //切換槍枝
    private int gunNum;         //標記當前使用槍枝

    [Header("玩家血條設置")]
    public Transform hpBarbg_player; //玩家血量條背景
    public RectTransform hpBar_player; //玩家血量條

    [Header("音效設置")]
    public AudioSource audioSource;
    public AudioClip changeweapon;

    [Header("物件設置")]
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject enemy5;
    public GameObject enemy6;
    public GameObject enemy7;
    public GameObject enemy8;
    public GameObject enemy9;
    public GameObject treasure;
    public GameObject shooterPoint1; //射擊位置
    public GameObject shooterPoint2; 
    public GameObject shooterPoint3; 
    public GameObject PauseWindow;
    public GameObject PauseButton;
    public Sprite but_continue;

    [Header("UI設置")]
    public Text shotgunMagTextUI;
    public Text smgMagTextUI;
    public Text bloodTextUI;
    public GameObject win;
    public GameObject totalTimeBox;
    public GameObject MenuButton;
    public Text time;
    public Text totalTime;
    int minute;
    int second;
    string totalTimeword;
    static public int aid = 0;

    // Start is called before the first frame update
    void Start()
    {
        guns[0].SetActive(true); //顯示預設槍枝
        shotgunMagTextUI.text = " 60";
        smgMagTextUI.text = " 30";
        bloodTextUI.text = " 0";
        time.text = "00:00";
        GameObject.Find("pistol").GetComponent<script_pistol>().UpdateAmmoUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUITime();
        SwitchGun(); //呼叫切換槍枝函數

        /*開鏡開始*/
        if (Input.GetMouseButton(1)) // 1 表示滑鼠右鍵
        {
            Camera.main.fieldOfView = 25;
        }
        else
        {
            Camera.main.fieldOfView = 70;
        }
        /*開鏡結束*/

        /*遊戲機制開始*/
        //win
        if (enemy1 == false && enemy2== false && enemy3 == false && enemy4 == false && enemy5 == false && enemy6 == false && enemy7 == false && enemy8 == false && enemy9 == false)
        {
            FirstPersonController.isCamera = false;
            win.SetActive(true);
        }
        //loss
        if (hpBar_player.GetComponent<Image>().fillAmount == 0 ||
            (script_pistol.bulletLeft == 0 && script_shotgun.bulletLeft == 0 && script_smg.bulletLeft == 0 && treasure == false))
        {
            SceneManager.LoadScene("dead");
        }
        /*遊戲機制結束*/

        if(Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            FirstPersonController.isCamera = false;
            PauseWindow.gameObject.SetActive(true);
            Time.timeScale = 0;
            PauseButton.GetComponent<Image>().sprite = but_continue;
        }

        /*使用血包開始*/
        if (aid > 0)
        {
            if (hpBar_player.GetComponent<Image>().fillAmount < 1.0f)
            {
                if (Input.GetKey(KeyCode.F))
                {
                    hpBar_player.GetComponent<Image>().fillAmount += 0.1f;
                    hpBar_player.sizeDelta = new Vector2(100, hpBar_player.sizeDelta.y);
                    aid -= 1;
                    bloodTextUI.text = " " + aid;
                }
            }
        }
        /*使用血包結束*/
    }

    /*切換槍枝開始*/
    void SwitchGun()
    {
        if (Input.GetKey("1"))
        {
            audioSource.clip = changeweapon;
            audioSource.Play();
            guns[gunNum].SetActive(false);
            gunNum = 0;
            guns[gunNum].SetActive(true);
        }
        if (Input.GetKey("2"))
        {
            audioSource.clip = changeweapon;
            audioSource.Play();
            guns[gunNum].SetActive(false);
            gunNum = 1;
            guns[gunNum].SetActive(true);
        }
        if (Input.GetKey("3"))
        {
            audioSource.clip = changeweapon;
            audioSource.Play();
            guns[gunNum].SetActive(false);
            gunNum = 2;
            guns[gunNum].SetActive(true);
        }
    }
    /*切換槍枝結束*/

    /*計時開始*/
    void UpdateUITime()
    {
        second = Mathf.FloorToInt(Time.time);
        minute = second / 60;
        second = second % 60;
       
        if(second>=10)
        {
            if(minute < 10)
            {
                time.text = "0" + minute + ":" + second;
            }
            else time.text = minute + ":" + second;
        }

        if (second < 10)
        {
            if (minute < 10)
            {
                time.text = "0" + minute + ":0" + second;
            }
            else time.text = minute + ":0" + second;
        }

        if (enemy1 == false && enemy2 == false && enemy3 == false && enemy4 == false && enemy5 == false && enemy6 == false && enemy7 == false && enemy8 == false && enemy9 == false)
        {
            Time.timeScale = 0;
            totalTimeword = "Total Time:" + time.text;
            totalTime.text = totalTimeword;
            shooterPoint1.SetActive(false);
            shooterPoint2.SetActive(false);
            shooterPoint3.SetActive(false);
            totalTimeBox.SetActive(true);
            MenuButton.SetActive(true);
        }
    }
    /*計時結束*/
}