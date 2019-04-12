using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WholeButtonFontTMP : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    List<TMPro.TextMeshProUGUI> texts = new List<TextMeshProUGUI>();

    // Start is called before the first frame update
    void Start()
    {
        texts.AddRange(GetComponentsInChildren<TextMeshProUGUI>());
        // Debug.Log(text.text);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData data)
    {
        foreach (var text in texts)
        {
            text.fontStyle = TMPro.FontStyles.Bold | TMPro.FontStyles.Underline;
        }        
        // Debug.Log("mouseover");
    }

    public void OnPointerExit(PointerEventData data)
    {
        foreach (var text in texts)
        {
            text.fontStyle = TMPro.FontStyles.Normal;
        }        
        // Debug.Log("mouseExit");
    }
    public void OnPointerClick(PointerEventData data)
    {
        foreach (var text in texts)
        {
            text.fontStyle = TMPro.FontStyles.Normal;
        }
    }

    public void SetFontToNormal()
    {
        foreach (var text in texts)
        {
            text.fontStyle = TMPro.FontStyles.Normal;
        }
    }
}