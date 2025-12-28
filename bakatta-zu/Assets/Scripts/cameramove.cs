using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramove : MonoBehaviour
{
    public GameObject player; // プレイヤーオブジェクト
    public float rotationSpeed = 5f;  // 回転速度
    public float verticalAngleLimit = 80f; // 上下回転の角度制限
    public Vector3 cameraOffset = new Vector3(0, 2, -5); // カメラ位置オフセット

    public bool isUIOpen = false; // ★UI表示中フラグ

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        // ★UI表示中はカメラを動かさない
        if (isUIOpen) return;

        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        yaw += mx * rotationSpeed;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            yaw += rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            yaw -= rotationSpeed * Time.deltaTime;
        }

        player.transform.rotation = Quaternion.Euler(0, yaw, 0);

        pitch -= my * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -verticalAngleLimit, verticalAngleLimit);

        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = player.transform.position + cameraRotation * cameraOffset;
        transform.rotation = cameraRotation;
    }
}
