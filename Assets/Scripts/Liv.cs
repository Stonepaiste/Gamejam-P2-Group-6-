using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Liv : MonoBehaviour
{

    [SerializeField] public Image livSpriteRenderer;
    public int maxFillLevel = 3;
    private float fillLevel;

    void Start()
    {
        fillLevel = maxFillLevel;
        UpdateLivSprite();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DecreaseFillLevel(1f);
        }
    }

    void DecreaseFillLevel(float amount)
    {
        fillLevel -= amount;
        if (fillLevel < 0)
        {
            fillLevel = 0;
        }
        UpdateLivSprite();
    }

    void UpdateLivSprite()
    {
        float fillAmount = fillLevel / maxFillLevel;
        livSpriteRenderer.fillAmount = fillAmount;

        //livSpriteRenderer.transform.localScale = new Vector3(fillPercentage, 1, 1);
    }
}
