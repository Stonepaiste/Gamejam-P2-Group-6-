using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void GoToIntroScene()
    {
        SceneManager.LoadScene("IntroScene");
        Debug.Log("You have clicked");

       // SceneManager.LoadScene(2);        //Scene index i stedet
    }
}
