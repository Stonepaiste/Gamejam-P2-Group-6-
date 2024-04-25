using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Liv : MonoBehaviour
{

    [SerializeField] public Image livSpriteRenderer;
    public int maxFillLevel = 3;
    public float fillLevel;
    death death;

    void Start()
    {
        fillLevel = maxFillLevel;
        UpdateLivSprite();
        death = FindObjectOfType<death>();
    }

    public void DecreaseFillLevel(float amount)
    {
        fillLevel -= amount;
        if (fillLevel < 0)
        {
            fillLevel = 0;
            death.Die();
        }
        UpdateLivSprite();
    }

    void UpdateLivSprite()
    {
        float fillAmount = fillLevel / maxFillLevel;
        livSpriteRenderer.fillAmount = fillAmount;
    }

}
