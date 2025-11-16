using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

// 生成AIとはいえ、よく出来てる！！！手直しの必要なくてすごく助かる！！
[System.Serializable]
public class Post {
    public string userName;
    public string message;
    public Sprite image;
    public System.DateTime time;
}

public class PostManager : MonoBehaviour {
    public GameObject timeLine;
    public GameObject showBtns;
    public Transform contentParent; // ScrollViewのContent
    public GameObject postPrefab;   // 投稿カードのPrefab
    private List<Post> posts = new List<Post>();

    public void CreatePost(string userName, string message, Sprite image = null) {
        Post newPost = new Post {
            userName = userName,
            message = message,
            image = image,
            time = System.DateTime.Now
        };

        posts.Insert(0, newPost); // 上に追加
        // UpdateTimeline(); // エディタの任意タイミングで実行したいので不要
    }

    void UpdateTimeline() {
        foreach (Transform child in contentParent) {
            Destroy(child.gameObject);
        }
        foreach (var post in posts) {
            GameObject obj = Instantiate(postPrefab, contentParent);
            obj.transform.Find("UserName").GetComponent<TextMeshProUGUI>().text = post.userName;
            obj.transform.Find("Message").GetComponent<TextMeshProUGUI>().text = post.message;
            obj.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = "Posted at: " + post.time.ToString();
            if (post.image != null) {
                obj.transform.Find("Image").GetComponent<Image>().sprite = post.image;
            } else {
                obj.transform.Find("Image").gameObject.SetActive(false);
            }
        }
    }
    // タイムラインを表示する(更新もここで行う)
    public void ShowTimeline()
    {
        timeLine.SetActive(true);
        UpdateTimeline();
        showBtns.SetActive(false);
    }
    // タイムラインを隠す
    public void HideTimeLine()
    {
        showBtns.SetActive(true);
        timeLine.SetActive(false);
    }
}

