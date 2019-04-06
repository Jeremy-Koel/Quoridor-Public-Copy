
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    public string levelToLoad = "";


    // Update is called once per frame
    void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    FadeToLevel("GameBoard");
        //}
    }

    public void FadeToLevel(string level)
    {
        levelToLoad = level;
        animator.SetTrigger("Fade_out");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
