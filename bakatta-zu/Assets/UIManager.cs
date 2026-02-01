using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject postUI;
    public GameObject timeLine;
    public GameObject showBtns;
    public TMP_InputField userNameInput;
    public TMP_InputField messageInput;
    public Image imagePreview;
    public GameObject postPrefab;
    public Transform contentParent;

    [Header("カメラ・プレイヤー制御")]
    public cameramove cameraMove;
    public Move playerMove;

    [Header("その他")]
    public kakunin shower;

    // 既存の Post クラスを利用
    private List<Post> posts = new List<Post>();
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

        posts.Insert(0, new Post
        {
            userName = userName,
            message = message,
            image = selectedImage,
            time = System.DateTime.Now
        });

        // 入力欄リセット
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
    // 投稿UI 表示/非表示
    // =========================
    public void ShowPostUI()
    {
        postUI.SetActive(true);
        showBtns.SetActive(false);

        // カメラ・プレイヤー停止
        if (cameraMove != null) cameraMove.enabled = false;
        if (playerMove != null) playerMove.enabled = false;

        // マウス表示
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        OnSelectImage(null);
        shower.showAllPicture();
    }

    public void HidePostUI()
    {
        postUI.SetActive(false);
        showBtns.SetActive(true);

        // カメラ・プレイヤー再開
        if (cameraMove != null) cameraMove.enabled = true;
        if (playerMove != null) playerMove.enabled = true;

        // マウス非表示
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // =========================
    // Timeline 表示/非表示
    // =========================
    public void ShowTimeline()
    {
        timeLine.SetActive(true);
        showBtns.SetActive(false);
        UpdateTimeline();

        // カメラ・プレイヤー停止
        if (cameraMove != null) cameraMove.enabled = false;
        if (playerMove != null) playerMove.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideTimeline()
    {
        timeLine.SetActive(false);
        showBtns.SetActive(true);

        // カメラ・プレイヤー再開
        if (cameraMove != null) cameraMove.enabled = true;
        if (playerMove != null) playerMove.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // =========================
    // Timeline 更新
    // =========================
    private void UpdateTimeline()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var post in posts)
        {
            GameObject obj = Instantiate(postPrefab, contentParent);

            obj.transform.Find("UserName")
                .GetComponent<TextMeshProUGUI>().text = post.userName;

            obj.transform.Find("Message")
                .GetComponent<TextMeshProUGUI>().text = post.message;

            obj.transform.Find("Time")
                .GetComponent<TextMeshProUGUI>().text =
                    post.time.ToString("yyyy/MM/dd HH:mm");

            Image img = obj.transform.Find("Image").GetComponent<Image>();
            img.gameObject.SetActive(post.image != null);
            if (post.image != null) img.sprite = post.image;
        }
    }
}
