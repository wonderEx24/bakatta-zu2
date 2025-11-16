using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class odenhatei : MonoBehaviour
{
 public float interactionRange = 3f;  // おでんとのインタラクション範囲
    private GameObject oden;  // おでんオブジェクト
    private bool isNearOden = false;  // おでんの近くにいるかどうかのフラグ

    // プレイヤーが近づいたときにおでんのオブジェクトを取得
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Oden"))  // おでんのオブジェクトに接触した場合
        {
            oden = other.gameObject;  // おでんを記録
            isNearOden = true;  // おでんの近くにいる
        }
    }

    // プレイヤーが離れたときにおでんのオブジェクトを解除
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Oden"))  // おでんのオブジェクトから離れた場合
        {
            oden = null;  // おでんを解除
            isNearOden = false;  // おでんの近くから離れた
        }
    }

    // Update関数でEキーが押されるのをチェック
    void Update()
    {
        if (isNearOden && Input.GetKeyDown(KeyCode.E))  // おでんの近くでEキーを押す
        {
            Debug.Log("おでんに触った！");  // デバッグログ出力
        }
    }
}