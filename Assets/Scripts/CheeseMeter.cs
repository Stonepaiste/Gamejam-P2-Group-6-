using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheeseMeter : MonoBehaviour
{
    public Image cheeseBar;
    float _cheeseFill;

    public void GetCheese()
    {
        Debug.Log(cheeseBar.fillAmount + "got cheese");
        _cheeseFill = 1f / 9f;
        cheeseBar.fillAmount += _cheeseFill;
    }

    public void ResetCheese()
    {
        Debug.Log(cheeseBar.fillAmount + "lost cheese");
        cheeseBar.fillAmount = 0;
    }
}
