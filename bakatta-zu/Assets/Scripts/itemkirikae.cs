using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemkirikae : MonoBehaviour
{
    public GameObject[] items;  // 持つアイテムを格納する配列
    public int motimono;
    public GameObject bulletPrefab;
    public GameObject fireworkPrefab;  // 花火のPrefab（爆発エフェクト）
    public Transform firePoint;  // 発射位置（持っている手の位置）
    public float fireRate = 0.005f; // 連射の間隔
    private float nextFireTime = 0f;

    // ハンマー回転用
    private bool isSwinging = false;
    private float swingTime = 0f;
    private float swingDuration = 0.3f;
    private Quaternion originalRotation;
    private Quaternion swingRotation;
    private Transform hammerTransform;

    void Start()
    {
        motimono = 10;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) motimono = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) motimono = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) motimono = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) motimono = 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) motimono = 5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) motimono = 6;
        if (Input.GetKeyDown(KeyCode.Alpha7)) motimono = 7;
        if (Input.GetKeyDown(KeyCode.Alpha8)) motimono = 8;
        if (Input.GetKeyDown(KeyCode.Alpha9)) motimono = 9;
        if (Input.GetKeyDown(KeyCode.Alpha0)) motimono = 10;

        ChangeItem();

        // 水鉄砲
        if (motimono == 7 && Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            ShootWaterGun();
        }

        // ハンマー振る
        if (motimono == 8 && Input.GetMouseButtonDown(0) && !isSwinging)
        {
            if (motimono - 1 < items.Length)
            {
                hammerTransform = items[motimono - 1].transform;
                originalRotation = hammerTransform.localRotation;
                swingRotation = originalRotation * Quaternion.Euler(0f, 0f, -90f);
                swingTime = 0f;
                isSwinging = true;
            }
        }

        if (isSwinging)
        {
            swingTime += Time.deltaTime / swingDuration;
            float swingLerp = Mathf.Sin(swingTime * Mathf.PI);
            hammerTransform.localRotation = Quaternion.Lerp(originalRotation, swingRotation, swingLerp);

            if (swingTime >= 1f)
            {
                isSwinging = false;
                hammerTransform.localRotation = originalRotation;
            }
        }

        // 花火発射
        if (motimono == 4 && Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            ShootFirework();
        }
    }

    void ChangeItem()
    {
        foreach (GameObject item in items) item.SetActive(false);

        if (motimono > 0 && motimono <= items.Length)
        {
            items[motimono - 1].SetActive(true);
        }
    }

    void ShootWaterGun()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(firePoint.forward * 40f, ForceMode.VelocityChange);
            }

            Collider bulletCollider = bullet.GetComponent<Collider>();
            if (bulletCollider != null)
            {
                foreach (GameObject existingBullet in GameObject.FindGameObjectsWithTag("Bullet"))
                {
                    Collider existingCollider = existingBullet.GetComponent<Collider>();
                    if (existingCollider != null)
                    {
                        Physics.IgnoreCollision(bulletCollider, existingCollider);
                    }
                }
            }

            bullet.tag = "Bullet";
            Destroy(bullet, 5f);
        }
    }

    // 花火発射処理
    void ShootFirework()
    {
        if (fireworkPrefab != null && firePoint != null)
        {
            // 花火を発射
            GameObject firework = Instantiate(fireworkPrefab, firePoint.position, firePoint.rotation);

            // 花火に力を加えて発射（向いている方向に飛ばす）
            Rigidbody rb = firework.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(firePoint.forward * 20f, ForceMode.VelocityChange);  // 20f は発射の強さ
            }

            Destroy(firework, 3f);  // 3秒後に花火を削除（爆発エフェクトの後など）
        }
    }
}

// 花火のスクリプト（爆発処理）
public class Firework : MonoBehaviour
{
    // 花火が地面に触れたら消える
    private void OnCollisionEnter(Collision collision)
    {
        // 地面やその他のオブジェクトに衝突したとき
        if (collision.gameObject.CompareTag("Ground"))  // 地面のTagは「Ground」に設定
        {
            // 爆発エフェクト（必要ならここに加えます）
            Destroy(gameObject);  // 花火を削除
        }
    }
}