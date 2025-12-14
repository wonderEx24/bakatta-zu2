using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Rigidbody rb;
    public float jumpPower = 5f;
    public float mouseSensitivity = 2f;
    public float keyMovementSpeed = 0.2f;
    private bool isGrounded = false;

    // おでんオブジェクトとのインタラクション範囲
    public Transform oden;  
    [Range(0.1f, 10f)]
    public float interactDistance = 2f;  
    private bool isNearOden = false;  

    // 行動中かどうかを示すフラグ
    private static bool isActioning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 playerFeetPosition = transform.position;

        // ★ Nullチェックで安全に
        float distanceToOden = 0f;
        if (oden != null)
            distanceToOden = Vector3.Distance(playerFeetPosition, oden.position);

        // おでんとのインタラクション判定
        if (distanceToOden <= interactDistance && !isActioning)
        {
            isNearOden = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                InteractWithOden();
                isActioning = true;
                StartCoroutine(ResetActionFlag());
            }
        }
        else
        {
            isNearOden = false;
        }

        // 移動処理
        float movementSpeed = Input.GetKey(KeyCode.LeftShift) ? keyMovementSpeed * 2 : keyMovementSpeed;

        if (Input.GetKey(KeyCode.A)) transform.Translate(-movementSpeed, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.D)) transform.Translate(movementSpeed, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.W)) transform.Translate(0.0f, 0.0f, movementSpeed);
        if (Input.GetKey(KeyCode.S)) transform.Translate(0.0f, 0.0f, -movementSpeed);

        // マウス回転
        float mx = Input.GetAxis("Mouse X");
        if (Mathf.Abs(mx) > 0.001f) transform.Rotate(0, mx * mouseSensitivity, 0);

        // ジャンプ処理
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void InteractWithOden()
    {
        Debug.Log("おでんに触った！");
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Angle(contact.normal, Vector3.up) < 45f)
                {
                    isGrounded = true;
                    return;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private IEnumerator ResetActionFlag()
    {
        yield return new WaitForSeconds(1f);
        isActioning = false;
    }
}
