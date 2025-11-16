using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class sakuteki : MonoBehaviour
{
    
    public Transform player;
    // Start is called before the first frame update
   
    void OnTriggerStay(Collider col)
    {
        Debug.Log("突破");
        if(col.gameObject.tag == "takeshi")
        {
            transform.LookAt(player);
            transform.Translate(0,0,0.1f);
             Debug.Log("みつけた");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}