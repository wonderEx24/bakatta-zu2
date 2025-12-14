using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PostUI : MonoBehaviour
{
    public GameObject postUI;
    public GameObject showBtns;
    public TMP_InputField userNameInput;
    public TMP_InputField messageInput;
    public Image imagePreview;
    public PostManager postManager;
    public kakunin shower;

    // カメラ制御用
    public cameramove cameraMove;

    // プレイヤー移動制御用
    public Move playerMove;

    private Sprite selectedImage;

    // 投稿ボタンを押した時
    public void OnSubmitPost()
    {
        string userName = userNameInput.text;
        string message = messageInput.text;

        if (string.IsNullOrEmpty(message))
        {
            Debug.Log("メッセージが空です！");
            return;
        }

        postManager.CreatePost(userName, message, selectedImage);

        // 入力欄をクリア
        userNameInput.text = "";
        messageInput.text = "";
        imagePreview.sprite = null;
        selectedImage = null;
    }

    // 画像選択
    public void OnSelectImage(Sprite sprite)
    {
        selectedImage = sprite;
        imagePreview.sprite = sprite;
    }

    // 投稿画面を表示
    public void ShowPostUI()
    {
        postUI.SetActive(true);
        showBtns.SetActive(false);

        OnSelectImage(null);
        shower.showAllPicture();

        // ★ カメラ停止
        if (cameraMove != null)
            cameraMove.enabled = false;

        // ★ プレイヤー停止
        if (playerMove != null)
            playerMove.enabled = false;

        // ★ マウスを解放
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 投稿画面を隠す
    public void HidePostUI()
    {
        showBtns.SetActive(true);
        postUI.SetActive(false);

        if (cameraMove != null)
            cameraMove.enabled = true;

        if (playerMove != null)
            playerMove.enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
