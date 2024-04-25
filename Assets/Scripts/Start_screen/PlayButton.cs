using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void GoToIntroScene()
    {
        //SceneManager.LoadScene("IntroScene");
        SceneManager.LoadScene(1);        //Scene index i stedet
        Debug.Log("You have clicked");

 
    }
}
