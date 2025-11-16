using UnityEngine;
using System.Collections;

public class warpdoor1 : MonoBehaviour
{
    public Transform warpDestination;
    public float disableTriggerDuration = 0.5f;  // 無効時間

    private Collider doorCollider;

    void Start()
    {
        doorCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ドアの当たり判定を無効化してワープ
            StartCoroutine(TemporarilyDisableTrigger(other));
        }
    }

    private IEnumerator TemporarilyDisableTrigger(Collider player)
    {
        doorCollider.enabled = false;

        // プレイヤーをワープ先に移動
        player.transform.position = warpDestination.position;
        player.transform.rotation = warpDestination.rotation;

        // 無効時間待機
        yield return new WaitForSeconds(disableTriggerDuration);

        doorCollider.enabled = true;
    }
}