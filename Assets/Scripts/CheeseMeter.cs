using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheeseMeter : MonoBehaviour
{
    public Image cheeseBar;
    int cheeseCounter;

    void Start()
    {
        cheeseCounter = FindObjectOfType<PickupCheese>().cheeseCounter;
    }

    public void getCheese(int Cheese)
    {
        Debug.Log(cheeseBar.fillAmount + "got cheese");
        cheeseCounter += Cheese;
        cheeseBar.fillAmount = cheeseCounter/100f;
    }

    public void loseCheese(int Cheese)
    {
        Debug.Log(cheeseBar.fillAmount + "lost cheese");
        cheeseCounter -= Cheese;
        cheeseBar.fillAmount = cheeseCounter/100f;
    }
}
