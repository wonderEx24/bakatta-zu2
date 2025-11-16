using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toumei2 : MonoBehaviour
{
    public GameObject toumei;
    [SerializeField] private CanvasGroup toumei3;
    // Start is called before the first frame update
    void Start()
    {
        toumei.SetActive(false);
        
    }

    // Update is called once per frame

    IEnumerator Transparent()
    {
        for (int i = 0; i < 100; i = i + 1)
            {
                toumei3.alpha = toumei3.alpha + 0.1f;
                yield return new WaitForSeconds(0.01f);
            }
    }
        
    
    public void OnClick()
        {   
            Debug.Log("a");
            toumei.SetActive(true);
            toumei3.alpha = 0;
            StartCoroutine("Transparent");
        }

    void Update()
    {
        
    }
}
