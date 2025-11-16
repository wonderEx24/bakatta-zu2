using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuukounin : MonoBehaviour
{
    [Header("転倒設定")]
    public float tiltThreshold = 30f; // 転倒とみなす傾きの閾値（度数）
    public float recoveryDelay = 2f; // 回復までの遅延時間（秒）
    public float standingRecoveryTime = 1f; // 立ち上がりの回復時間
    public float velocityThreshold = 0.5f; // 吹っ飛んでいるかを判断する速度の閾値

    [Header("巡回設定")]
    public List<Transform> patrolPoints; // 巡回ポイント
    public float patrolSpeed = 2f; // 巡回速度

    private Vector3 fallenPosition;
    private Quaternion fallenRotation;
    private bool isRecovering = false;
    private bool hasFallen = false;
    private bool isRecovered = false;
    private Rigidbody rb;
    private float timeFallen = 0f;

    private int currentPointIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // パトロール中の移動
        if (!hasFallen && !isRecovering && !isRecovered && patrolPoints.Count > 0)
        {
            Patrol();
        }

        // 吹っ飛んでいる途中では転倒判定しない
        if (rb.velocity.magnitude < velocityThreshold && !hasFallen && !isRecovered)
        {
            if (IsFallen() && !hasFallen)
            {
                hasFallen = true;
                fallenPosition = transform.position;
                fallenRotation = transform.rotation;
                timeFallen = Time.time;
                //Debug.Log("転倒しました！");
            }
        }

        // 転倒後、一定時間経過で復帰処理
        if (hasFallen)
        {
            if (Time.time - timeFallen > recoveryDelay && !isRecovering)
            {
                RecoverFromRagdoll();
            }
        }
    }

    // 巡回処理
    void Patrol()
    {
        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        Vector3 move = direction * patrolSpeed * Time.deltaTime;

        // 移動（Y軸固定）
        transform.position += new Vector3(move.x, 0f, move.z);

        // 向き調整（滑らかに）
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 5f * Time.deltaTime);
        }

        // 次のポイントへ
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
        }
    }

    // 衝突処理（必要に応じて拡張）
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("プレイヤーが衝突しました！");
        }
    }

    // 転倒判定
    bool IsFallen()
    {
        float tiltAngleX = Mathf.Abs(transform.rotation.eulerAngles.x);
        float tiltAngleZ = Mathf.Abs(transform.rotation.eulerAngles.z);

        tiltAngleX = (tiltAngleX > 180) ? 360 - tiltAngleX : tiltAngleX;
        tiltAngleZ = (tiltAngleZ > 180) ? 360 - tiltAngleZ : tiltAngleZ;

        return tiltAngleX > tiltThreshold || tiltAngleZ > tiltThreshold;
    }

    // 起き上がり処理
    void RecoverFromRagdoll()
    {
        if (isRecovering || isRecovered) return;

        isRecovering = true;

        // 元の位置と回転に戻す
        transform.position = fallenPosition;
        transform.rotation = fallenRotation;

        StartCoroutine(StandUpAfterRecovery());

        isRecovering = false;
        isRecovered = true;
        hasFallen = false;
    }

    // 復帰処理
    IEnumerator StandUpAfterRecovery()
    {
        yield return new WaitForSeconds(standingRecoveryTime);

        // Y軸だけの回転に補正し、Y位置を0に戻す
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        isRecovered = false;
    }
}
