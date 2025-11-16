using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toumeika : MonoBehaviour
{
    MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        StartCoroutine("Transparent");
    }
   
        IEnumerator Transparent()
        {
            for (int i = 0; i < 255; i = i - 8)
            {
                mesh.material.color = mesh.material.color - new Color32(0, 0, 0, 8);
                yield return new WaitForSeconds(0.01f);
            }
        }
    
    public void OnClick()
        {
        StartCoroutine("Transparent");
        }
    

}
