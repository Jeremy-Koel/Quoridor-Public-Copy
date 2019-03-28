//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GameCore;
//using UnityEngine.EventSystems;

//public class ClickMouse : MonoBehaviour
//{

//    public bool mouseSelected;
//    private bool cursorOnMouse;
//    private SpriteOutline spriteOutline;
//    public InterfaceController interfaceController;
//    private Material highlightMat;
//    private Material gameSquareMat;
//    private List<string> possibleMoves;

//    // Start is called before the first frame update
//    void Start()
//    {
//        mouseSelected = false;
//        cursorOnMouse = false;
//        spriteOutline = GetComponent<SpriteOutline>();
//        spriteOutline.enabled = false;
//        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
//        highlightMat = Resources.Load("highlightColor", typeof(Material)) as Material;
//        possibleMoves = new List<string>();
//        gameSquareMat = Resources.Load("cubeColor", typeof(Material)) as Material;
//    }

//    //https://answers.unity.com/questions/587637/replacing-onmouseenterexitdownetc-with-raycasting.html
//    // Update is called once per frame
//    public GameObject hoveredGO;
//    public enum HoverState { HOVER, NONE };
//    public HoverState hover_state = HoverState.NONE;

//    void Update()
//    {
//        RaycastHit hitInfo = new RaycastHit();
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//        if (Physics.Raycast(ray, out hitInfo))
//        {
//            if (hover_state == HoverState.NONE)
//            {
//                hitInfo.collider.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
//                hoveredGO = hitInfo.collider.gameObject;
//            }
//            hover_state = HoverState.HOVER;
//        }
//        else
//        {
//            if (hover_state == HoverState.HOVER)
//            {
//                hoveredGO.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
//            }
//            hover_state = HoverState.NONE;
//        }

//        if (hover_state == HoverState.HOVER)
//        {
//            hitInfo.collider.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver); //Mouse is hovering
//            if (Input.GetMouseButtonDown(0))
//            {
//                hitInfo.collider.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver); //Mouse down
//            }
//            if (Input.GetMouseButtonUp(0))
//            {
//                hitInfo.collider.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver); //Mouse up
//            }

//        }
        
//        if(Input.GetMouseButtonUp(0) && !cursorOnMouse)
//        {
//            mouseSelected = false;
//            spriteOutline.enabled = false;
//            foreach (string move in possibleMoves)
//            {
//                GameObject.Find(move).GetComponent<Renderer>().material = gameSquareMat;
//            }
//            //Debug.Log("Update Mouse");
//        }
//    }

//    private void OnMouseEnter()
//    {
//        if (!EventSystem.current.IsPointerOverGameObject())
//        {
//            cursorOnMouse = true;
//        }
//    }

//    private void OnMouseExit()
//    {
//        cursorOnMouse = false;
//    }
//    private void OnMouseUpAsButton()
//    {
//        if (!EventSystem.current.IsPointerOverGameObject())
//        {
//            GameBoard.PlayerEnum player = interfaceController.GetWhoseTurn();

//            if ((player == GameBoard.PlayerEnum.ONE && name == "playerMouse") || (player == GameBoard.PlayerEnum.TWO && name == "opponentMouse"))
//            {
//                mouseSelected = true;
//                spriteOutline.enabled = true;
//                possibleMoves = interfaceController.GetPossibleMoves(); 
//                foreach(string move in possibleMoves)
//                {
//                    GameObject square = GameObject.Find(move);
//                    square.GetComponent<Renderer>().material = highlightMat;
//                }

//                //Debug.Log("ButtonMouse");
//            }
//        }
//        //gameObject.SendMessage("OnClickToggleMouse", SendMessageOptions.DontRequireReceiver);
//    }
//}
