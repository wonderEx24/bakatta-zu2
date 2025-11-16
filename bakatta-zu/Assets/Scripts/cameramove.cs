using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramove : MonoBehaviour
{
    public GameObject player; // プレイヤーオブジェクト
    public float rotationSpeed = 5f;  // 回転速度（プレイヤーとカメラ共通）
    public float verticalAngleLimit = 80f; // 上下回転の角度制限（度）
    public Vector3 cameraOffset = new Vector3(0, 2, -5); // カメラの位置オフセット

    private float yaw = 0f; // 水平回転角度
    private float pitch = 0f; // 垂直回転角度

    void Start()
    {
        // 初期角度を取得
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        // マウス入力を取得
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // マウスによる水平回転（Y軸）
        yaw += mx * rotationSpeed;

        // 矢印キーによる水平回転
        if (Input.GetKey(KeyCode.RightArrow))
        {
            yaw += rotationSpeed * Time.deltaTime; // 右矢印で右回転
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            yaw -= rotationSpeed * Time.deltaTime; // 左矢印で左回転
        }

        // プレイヤーの回転を反映
        player.transform.rotation = Quaternion.Euler(0, yaw, 0);

        // 垂直回転（カメラのみ）
        pitch -= my * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -verticalAngleLimit, verticalAngleLimit);

        // カメラの回転を計算
        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0);

        // カメラ位置をプレイヤー位置に合わせる
        transform.position = player.transform.position + cameraRotation * cameraOffset;
        transform.rotation = cameraRotation;
    }
}