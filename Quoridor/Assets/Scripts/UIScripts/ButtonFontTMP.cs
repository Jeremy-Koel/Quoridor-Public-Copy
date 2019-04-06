using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonFontTMP : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TMPro.TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInParent<TextMeshProUGUI>();
        // Debug.Log(text.text);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData data)
    {
        text.fontStyle = TMPro.FontStyles.Bold | TMPro.FontStyles.Underline;
        // Debug.Log("mouseover");
    }

    public void OnPointerExit(PointerEventData data)
    {
        text.fontStyle = TMPro.FontStyles.Normal;
        // Debug.Log("mouseExit");
    }

}