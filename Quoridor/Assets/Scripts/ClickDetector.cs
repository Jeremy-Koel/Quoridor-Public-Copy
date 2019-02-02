using UnityEngine;
using System.Collections;

public class ClickDetector : MonoBehaviour
{
    public bool HandleLeftClick = true;
    public bool HandleRightClick = true;
    public bool HandleMiddleClick = false;
    public string OnLeftClickMethodName = "OnLeftClick";
    public string OnRightClickMethodName = "OnRightClick";
    public string OnMiddleClickMethodName = "OnMiddleClick";
    public LayerMask layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Walls");
    }
    void Update()
    {
        GameObject clickedGmObj = null;
        bool clickedGmObjAcquired = false;

        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            /*if (!clickedGmObjAcquired)
            {*/
            clickedGmObj = GetClickedGameObject();
            clickedGmObjAcquired = true;
            /*}*/

            if (clickedGmObj != null)
                clickedGmObj.SendMessage(OnLeftClickMethodName, null, SendMessageOptions.DontRequireReceiver);
        }

        // Right click
        if (Input.GetMouseButtonDown(1))
        {
            if (!clickedGmObjAcquired)
            {
                clickedGmObj = GetClickedGameObject();
                clickedGmObjAcquired = true;
            }

            if (clickedGmObj != null)
                clickedGmObj.SendMessage(OnRightClickMethodName, null, SendMessageOptions.DontRequireReceiver);
        }

        // Middle click
        if (Input.GetMouseButtonDown(2))
        {
            if (!clickedGmObjAcquired)
            {
                clickedGmObj = GetClickedGameObject();
                clickedGmObjAcquired = true;
            }

            if (clickedGmObj != null)
                clickedGmObj.SendMessage(OnMiddleClickMethodName, null, SendMessageOptions.DontRequireReceiver);
        }
    }

    GameObject GetClickedGameObject()
    {
        // Builds a ray from camera point of view to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Casts the ray and get the first game object hit
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            return hit.transform.gameObject;
        else
            return null;

        
    }
}