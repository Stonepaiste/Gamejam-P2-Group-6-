using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Meteor : MonoBehaviour
{
    Liv Liv;
    PlayerVFX playerVFX;
    private void Start()
    {
        Liv = FindObjectOfType<Liv>();
        playerVFX = FindObjectOfType<PlayerVFX>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Liv.DecreaseFillLevel(1f);
            playerVFX.AlarmLights();
            Debug.Log("oh no!");
        }
    }
}
