using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveArms : MonoBehaviour
{
    public bool moveArm;
    public Vector3 target;
    private float speed = 35.0f;
    public float chatSpeed = 2000.0f;
    private Vector3 origPos;
    public Vector2 chatTarget;
    public Vector2 chatOrigPos;
    public bool isChatArm = false;

    public RectTransform chatArmRectTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            isChatArm = true;
            chatArmRectTransform = GetComponent<RectTransform>();
        }
        else
        {
            origPos = transform.position;
            target = transform.position;
        }
        moveArm = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveArm)
        {
            if (isChatArm)
            {
                //float step = chatSpeed * Time.deltaTime; // calculate distance to move
                float step = FastAnimations.CalculateSpeed(chatSpeed);
                Vector2 transformPosition = new Vector2(chatArmRectTransform.localPosition.x,
                                                chatArmRectTransform.localPosition.y);

                chatArmRectTransform.localPosition = Vector2.MoveTowards(transformPosition, chatTarget, step);
                chatArmRectTransform.localPosition = new Vector3(chatArmRectTransform.localPosition.x,
                                                        chatArmRectTransform.localPosition.y, -3);

                //if ((chatArmRectTransform.localPosition.x == chatTarget.x &&
                //        chatArmRectTransform.localPosition.y == chatTarget.y) && chatTarget != chatOrigPos)
                //{
                //    chatTarget = chatOrigPos;
                //}
                if (chatArmRectTransform.localPosition.x == chatOrigPos.x &&
                            chatArmRectTransform.localPosition.y == chatOrigPos.y)
                {
                    moveArm = false;
                }
            }
            else
            {
                //float step = speed * Time.deltaTime; // calculate distance to move
                float step = FastAnimations.CalculateSpeed(speed);
                transform.position = Vector3.MoveTowards(transform.position, target, step);

                if (transform.position == target && target != origPos)
                {
                    target = origPos;
                }
                else if (transform.position == target)
                {
                    moveArm = false;
                }
            }
        }
    }

    public void SetTarget(Vector3 pos)
    {
        if (name == "ScientistArmOne")
        {
            Vector3 newTarget = new Vector3(pos.x, pos.y - 18, -2);
            target = newTarget;
        }
        else if(name == "ScientistArmTwo")
        {
            Vector3 newTarget = new Vector3(pos.x, pos.y + 18, -2);
            target = newTarget;
        }
    }

    public void SetTargetChat(Vector2 pos)
    {
        //Vector2 newTarget = new Vector2(pos.x, pos.y - 23);
        chatTarget = pos;
    }
}
