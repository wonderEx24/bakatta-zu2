using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class satuei : MonoBehaviour
{
    private bool isKeyPressed = false;  // Qキーが押されたかどうかを記録するフラグ
    public MeshRenderer targetRenderer;  // 操作対象のMeshRenderer
    private Coroutine currentCoroutine = null;  // 現在のコルーチンを管理
    public itemkirikae MonoBehaviour;  // MotimonoManagerスクリプトを参照する

    // Start is called before the first frame update
    void Start()
    {
        targetRenderer.enabled = false;  // 初期状態でオブジェクトは非表示にする
    }

    // Update is called once per frame
    void Update()
    {
        if (MonoBehaviour.motimono == 1)
        {
            // Qキーが押されるときにのみ動作
            if (Input.GetKeyDown(KeyCode.Q) && !isKeyPressed)
            {
                isKeyPressed = true;  // フラグを立てて、Qキーが押されていることを記録
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);  // 既存のコルーチンがあれば停止
                }
                currentCoroutine = StartCoroutine(ShowAndHideMesh());  // 新しいコルーチンを開始
            }

            // Qキーが離されたときにフラグをリセット
            if (Input.GetKeyUp(KeyCode.Q))
            {
                isKeyPressed = false;
            }
        }
    }

    // メッシュを表示し、一定時間後に非表示にするコルーチン
    IEnumerator ShowAndHideMesh()
    {
        targetRenderer.enabled = true;  // オブジェクトを表示
        yield return new WaitForSeconds(0.5f);  // 0.5秒待機
        targetRenderer.enabled = false;  // オブジェクトを非表示
    }
}