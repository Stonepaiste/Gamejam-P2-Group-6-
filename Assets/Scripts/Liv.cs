using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Liv : MonoBehaviour
{
    public static Liv Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] public Image livSpriteRenderer;
    public static float MaxFillLevel = 3f;
    public static float FillLevel;

    void Start()
    {
        Initiate();
    }

    public void Initiate()
    {
        FillLevel = MaxFillLevel;
        UpdateLivSprite();
    }
    

    public void DecreaseFillLevel(float amount)
    {
        FillLevel -= amount;
        if (FillLevel <= 0)
        {
            FillLevel = 0;
            GameFlow.Instance.GameOver();
        }
        UpdateLivSprite();
    }

    void UpdateLivSprite()
    {
        float fillAmount = FillLevel / MaxFillLevel;
        livSpriteRenderer.fillAmount = fillAmount;
    }
}
