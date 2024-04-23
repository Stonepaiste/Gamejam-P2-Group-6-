using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image cheeseBar;
    public float cheeseAmount=0f;
    void Start()
    {
        
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

    public void getCheese(float Cheese)
    {
        cheeseAmount+=Cheese;
        cheeseBar.fillAmount = cheeseAmount/100f;
    }

    public void loseCheese(float Cheese)
    {
        cheeseAmount-=Cheese;
        cheeseBar.fillAmount = cheeseAmount/100f;
    }
}
