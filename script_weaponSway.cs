using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_weaponSway : MonoBehaviour
{
    public float amout; //搖擺幅度
    public float smoothAmout; //搖擺平滑度
    public float maxAmout; //最大幅度搖擺

    private Vector3 originPosition; //初始位置

    // Start is called before the first frame update
    void Start()
    {
        originPosition = transform.localPosition; //自身位置(相對於父物件)
    }

    // Update is called once per frame
    void Update()
    {
        //獲取滑鼠位置
        float movementX = Input.GetAxis("Mouse X") * amout;
        float movementY = Input.GetAxis("Mouse Y") * amout;

        //限制
        movementX = Mathf.Clamp(movementX, -maxAmout, maxAmout);
        movementY = Mathf.Clamp(movementY, -maxAmout, maxAmout);


        //手臂變化
        Vector3 finallyPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finallyPosition + originPosition, Time.deltaTime*smoothAmout);
    }
}