using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonFont : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInParent<Text>();
       // Debug.Log(text.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData data)
    {
        text.fontStyle = FontStyle.Bold;
       // Debug.Log("mouseover");
    }

    public void OnPointerExit(PointerEventData data)
    {
        text.fontStyle = FontStyle.Normal;
       // Debug.Log("mouseExit");
    }

    //public void OnPointerClick(PointerEventData data)
    //{
    //    text.fontStyle = FontStyle.Normal;
    //}

}
