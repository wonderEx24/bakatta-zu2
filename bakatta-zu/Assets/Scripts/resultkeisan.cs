using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class resultkeisan : MonoBehaviour
{
    //テスト
    // Start is called before the first frame update
    public int times; //実際にかかった時間
    int timematu;　//リザルト画面で時間をゆっくり表示するために必要な変数
    void Start()
    {
        StartCoroutine("Transparent");
    }
   
        IEnumerator Transparent()
        {
            for (int i = 0; i > times; i = i + 8)
            {
                timematu = timematu + 1;
                yield return new WaitForSeconds(0.01f);
            }
        }

    // Update is called once per frame
    void Update()
    {
        
    }
}
