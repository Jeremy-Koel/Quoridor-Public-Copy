//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//class InvalidMovePopup : MonoBehaviour
//{
//    // script for playing sound effects 
//    private SoundEffectController soundEffectController;

//    private float popupTime = 1f;
//    //how long should we turn for.

//    public bool isPoppedUp = false;
//    //are we currently popped up?
//    //we use this to prevent multiple instance of popping up starting.

//    private GameObject invalidMovePopup;
//    private bool initialInactive;
//    private void Start()
//    {
//        invalidMovePopup = GameObject.Find("InvalidMoveBox");
//        //invalidMovePopup.SetActive(false);
//        initialInactive = true;
//        soundEffectController = GameObject.Find("GameController").GetComponent<SoundEffectController>();
//    }

    
//    void Update()
//    {
//        if(initialInactive)
//        {
//            invalidMovePopup.SetActive(false);
//            initialInactive = false;
//        }
//        if (isPoppedUp)
//            StartCoroutine(PopUp());
//    }

//    IEnumerator PopUp()
//    {
//        invalidMovePopup.SetActive(true);
//        soundEffectController.PlayErrorSound();
//        float time = 0;
        
//        while (time < popupTime)
//        {
//            time += Time.deltaTime;
           
//            yield return null;
//        }
//        //Turn back to the starting position.
//        //while (time > 0)
//        //{
//        //    time -= Time.deltaTime;
//        //    transform.eulerAngles -= turnIncrement;
//        //    yield return null;
//        //}
//        isPoppedUp = false;
//        invalidMovePopup.SetActive(false);
//    }
//}