using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kakunin : MonoBehaviour
{
   [SerializeField] GameObject prefab;
   [SerializeField] Transform contentParent;
      void Start()
      {
         // foreach (Sprite s in Resources.LoadAll<Sprite>("ScreenShots"))
         // {
         //    GameObject obj = Instantiate(prefab);
         //    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
         //    sr.sprite = s;
         // }
      }
   public void showAllPicture()
   {
      foreach (Transform child in contentParent) {
         Destroy(child.gameObject);
      }
      foreach (Texture2D t in Resources.LoadAll<Texture2D>("ScreenShots"))
      {
         Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
         GameObject obj = Instantiate(prefab, contentParent);
         Image im = obj.GetComponent<Image>();
         im.sprite = s;
      }
   }
}