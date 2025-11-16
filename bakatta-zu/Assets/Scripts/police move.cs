using UnityEngine;
using System.Collections;

public class PatrolAndChase : MonoBehaviour
{
    public Move playermove;
    public Transform[] patrolPoints;      // 巡回するポイント
    public float patrolSpeed = 3f;        // 巡回時の速度
    public float chaseSpeedMultiplier = 1f; // 追尾時の速度倍率（プレイヤー速度の何倍か）
    public Transform target;              // プレイヤー
    public float detectionRange = 10f;    // プレイヤーを発見する範囲
    public float fieldOfView = 60f;       // 視野角
    public float lostSightGraceTime = 3f; // 見失い猶予時間 (秒)

    public float playerMovementSpeed = 0.2f;

    // 🔁 複製関連の追加項目
    public bool shouldDuplicate = true;         // このオブジェクトが増殖を開始するか
    public GameObject clonePrefab;              // 自分自身のプレハブ（増殖に使用）

    private int currentPatrolIndex = 0;         // 現在の巡回ポイント
    private bool chasing = false;               // 追尾中かどうか
    private float lostSightTimer = 0f;          // 見失い猶予タイマー

    void Start()
    {
        // 🔁 クローン増殖開始
        //if (shouldDuplicate)
        //{
            //StartCoroutine(CloneSelfRoutine());
        //}
    }

    void Update()
    {
        if (chasing)
        {
            ChaseTarget();
        }
        else
        {
            Patrol();
            CheckForTarget();
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform patrolPoint = patrolPoints[currentPatrolIndex];
        Vector3 direction = (patrolPoint.position - transform.position).normalized;
        transform.position += direction * patrolSpeed * Time.deltaTime;
        transform.LookAt(patrolPoint);

        if (Vector3.Distance(transform.position, patrolPoint.position) < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void CheckForTarget()
    {
        if (target == null) return;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        if (distanceToTarget <= detectionRange && angleToTarget <= fieldOfView / 2)
        {
            if (HasLineOfSight(target))
            {
                chasing = true;
                lostSightTimer = lostSightGraceTime;
            }
        }
    }

    private void ChaseTarget()
    {
        if (target == null)
        {
            chasing = false;
            return;
        }

        playerMovementSpeed = playermove.keyMovementSpeed;
        float chaseSpeed = playerMovementSpeed * chaseSpeedMultiplier;

        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * chaseSpeed * Time.deltaTime;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.MovePosition(newPosition);
        }
        else
        {
            transform.position = newPosition;
        }

        transform.LookAt(target);

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        float angleToTarget = Vector3.Angle(transform.forward, direction);

        if (distanceToTarget > detectionRange || angleToTarget > fieldOfView / 2 || !HasLineOfSight(target))
        {
            lostSightTimer -= Time.deltaTime;
            if (lostSightTimer <= 0)
            {
                chasing = false;
            }
        }
        else
        {
            lostSightTimer = lostSightGraceTime;
        }
    }

    private bool HasLineOfSight(Transform target)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToTarget, out hit, distanceToTarget))
        {
            return hit.transform == target;
        }
        return true;
    }

    // 🔁 クローンを1秒ごとに生成するコルーチン
    //private IEnumerator CloneSelfRoutine()
    //{
    //while (true)
    //{
        //yield return new WaitForSeconds(1f);

        //if (clonePrefab != null)
        //{
            //GameObject clone = Instantiate(clonePrefab, transform.position + new Vector3(1f, 0f, 0f), transform.rotation);

            //PatrolAndChase cloneScript = clone.GetComponent<PatrolAndChase>();
            //if (cloneScript != null)
            //{
                //cloneScript.shouldDuplicate = false; // クローンは複製しない
            //}
        //}
    //}
    //}
}