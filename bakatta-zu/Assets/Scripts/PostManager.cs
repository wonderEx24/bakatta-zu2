using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Post
{
    public string userName;
    public string message;
    public Sprite image;
    public System.DateTime time;
}

public class PostManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject timeLine;
    public GameObject showBtns;
    public Transform contentParent;
    public GameObject postPrefab;

    [Header("操作制御")]
    public cameramove cameraMove;
    public Move playerMove;

    private List<Post> posts = new List<Post>();

    // =========================
    // 投稿作成
    // =========================
    public void CreatePost(string userName, string message, Sprite image = null)
    {
        posts.Insert(0, new Post
        {
            userName = userName,
            message = message,
            image = image,
            time = System.DateTime.Now
        });
    }

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

    // =========================
    // UI表示
    // =========================
    public void ShowTimeline()
    {
        timeLine.SetActive(true);
        showBtns.SetActive(false);

        UpdateTimeline();
        DisableControl();
    }

    public void HideTimeline()
    {
        timeLine.SetActive(false);
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
