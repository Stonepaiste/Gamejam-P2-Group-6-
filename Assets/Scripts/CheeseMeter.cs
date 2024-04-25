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

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            getCheese(10);
        }

        else if (Input.GetKeyDown(KeyCode.Return))
        {
            loseCheese(100);
        }
    }

    public void getCheese(int Cheese)
    {
        cheeseCounter += Cheese;
        cheeseBar.fillAmount = cheeseCounter/100f;
    }

    public void loseCheese(int Cheese)
    {
        cheeseCounter -= Cheese;
        cheeseBar.fillAmount = cheeseCounter/100f;
    }
}
