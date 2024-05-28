using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void GoToIntroScene()        // Code from https://www.youtube.com/watch?v=3SdMFPdSi7M&t=656s
    {
        //SceneManager.LoadScene("IntroScene");
        SceneManager.LoadScene(1);        //Scene index i stedet
        Debug.Log("Play button is clicked");

 
    }
}
