using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectImg : MonoBehaviour
{
    PostUI manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("PostUI").GetComponent<PostUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // ボタンが押されたときに、やるよ
    public void OnPushed()
    {
        Debug.Log("Pushed");
        Sprite sp = this.GetComponent<Image>().sprite;
        manager.OnSelectImage(sp);
    }
}
