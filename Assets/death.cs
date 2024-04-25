using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour
{
    SpriteRenderer sprite;
    [SerializeField] Sprite deadMouse;

    public void Die()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = deadMouse;
       
    }
}
