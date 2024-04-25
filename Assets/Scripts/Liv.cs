using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Liv : MonoBehaviour
{

    [SerializeField] public Image livSpriteRenderer;
    public float maxFillLevel = 3f;
    public float fillLevel;
    PlayerVFX playerVFX;

    void Start()
    {
        fillLevel = maxFillLevel;
        UpdateLivSprite();
        playerVFX = FindObjectOfType<PlayerVFX>();
    }

    public void DecreaseFillLevel(float amount)
    {
        fillLevel -= amount;
        if (fillLevel <= 0)
        {
            fillLevel = 0;
            playerVFX.Die();
        }
        UpdateLivSprite();
    }

    void UpdateLivSprite()
    {
        float fillAmount = fillLevel / maxFillLevel;
        livSpriteRenderer.fillAmount = fillAmount;
    }

}
