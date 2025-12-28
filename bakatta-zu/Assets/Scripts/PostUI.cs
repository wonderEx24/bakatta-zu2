using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PostUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject postUI;
    public GameObject showBtns;

    public TMP_InputField userNameInput;
    public TMP_InputField messageInput;
    public Image imagePreview;

    [Header("管理")]
    public PostManager postManager;
    public kakunin shower;

    [Header("操作制御")]
    public cameramove cameraMove;
    public Move playerMove;

    private Sprite selectedImage;

    // =========================
    // 投稿処理
    // =========================
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

        userNameInput.text = "";
        messageInput.text = "";
        OnSelectImage(null);
    }

    public void OnSelectImage(Sprite sprite)
    {
        selectedImage = sprite;
        imagePreview.sprite = sprite;
    }

    // =========================
    // UI表示
    // =========================
    public void ShowPostUI()
    {
        postUI.SetActive(true);
        showBtns.SetActive(false);

        OnSelectImage(null);
        shower.showAllPicture();

        DisableControl();
    }

    public void HidePostUI()
    {
        postUI.SetActive(false);
        showBtns.SetActive(true);

        StartCoroutine(EnableControlNextFrame());
    }

    // =========================
    // 操作制御
    // =========================
    private void DisableControl()
    {
        if (cameraMove != null) cameraMove.enabled = false;
        if (playerMove != null) playerMove.enabled = false;

        CursorManager.Instance.LockForUI();
    }

    private void EnableControl()
    {
        if (cameraMove != null) cameraMove.enabled = true;
        if (playerMove != null) playerMove.enabled = true;

        CursorManager.Instance.Restore();
    }

    private IEnumerator EnableControlNextFrame()
    {
        yield return null;
        EnableControl();
    }
}
