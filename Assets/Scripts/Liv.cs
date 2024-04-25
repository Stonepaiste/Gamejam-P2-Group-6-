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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Meteor"))
        {
            DecreaseFillLevel(1f);
            Debug.Log("oh no!");
        }
    }

    public void DecreaseFillLevel(float amount)
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
