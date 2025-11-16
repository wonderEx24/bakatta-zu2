using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textidou : MonoBehaviour
{
    public GameObject toumei;
    [SerializeField] private CanvasGroup toumei3;
    public RectTransform about;
    public float time = 0;
    // Start is called before the first frame update
    void Start()
    {

    }
    IEnumerator idou()
    {

        for (int i = 0; i < 20; i = i + 1)
        {
            about.transform.position = new Vector3(0, 30, 0);
            time = time + 1;
            yield return new WaitForSeconds(0.1f);
        }
        if (time == 20)
        {
            for (int i = 0; i < 100; i = i + 1)
            {
                toumei3.alpha = toumei3.alpha - 0.1f;
                yield return new WaitForSeconds(0.01f);
            }
            toumei.SetActive(false);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {
        time = 0;
        Debug.Log("b");
        about.transform.position = new Vector3(0, -1100, 0);
        StartCoroutine("idou");
        
    }
}
