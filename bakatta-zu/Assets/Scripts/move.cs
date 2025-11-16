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
    public Transform oden;  // おでんの位置
    [Range(0.1f, 10f)] // 0.1から10の範囲で距離を設定できるようにする
    public float interactDistance = 2f;  // おでんとのインタラクション距離
    private bool isNearOden = false;  // おでんの近くにいるかどうか

    // 行動中かどうかを示すフラグ
    private static bool isActioning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // プレイヤーの足元の位置を取得（またはColliderの中心）
        Vector3 playerFeetPosition = transform.position;

        // おでんとの距離を計算
        float distanceToOden = Vector3.Distance(playerFeetPosition, oden.position);

        // おでんとのインタラクション判定
        if (distanceToOden <= interactDistance && !isActioning) // おでんに近い、かつ行動していない場合
        {
            isNearOden = true;
            if (isNearOden)
            {
                //Debug.Log("おでんに近い状態です！");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                InteractWithOden();  // おでんとのインタラクション
                isActioning = true;  // 行動中フラグを立てる
                StartCoroutine(ResetActionFlag()); // 1秒後にフラグを戻す
            }
        }
        else
        {
            isNearOden = false;
        }


        // プレイヤーの移動処理
        float movementSpeed = Input.GetKey(KeyCode.LeftShift) ? keyMovementSpeed * 2 : keyMovementSpeed;

        if (Input.GetKey(KeyCode.A)) transform.Translate(-movementSpeed, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.D)) transform.Translate(movementSpeed, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.W)) transform.Translate(0.0f, 0.0f, movementSpeed);
        if (Input.GetKey(KeyCode.S)) transform.Translate(0.0f, 0.0f, -movementSpeed);

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
        // おでんとのインタラクション処理（例えば、ログを出力）
        Debug.Log("おでんに触った！");
    }

    // 地面判定
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

    // 行動フラグを1秒後にリセットするコルーチン
    private IEnumerator ResetActionFlag()
    {
        yield return new WaitForSeconds(1f);  // 1秒待機
        isActioning = false;  // フラグをリセット
    }
}
//player speed 0.12