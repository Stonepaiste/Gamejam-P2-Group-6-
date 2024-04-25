using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meteor : MonoBehaviour
{
    Liv Liv;

    private void Start()
    {
        Liv = FindObjectOfType<Liv>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Liv.DecreaseFillLevel(1f);
            Debug.Log("oh no!");
        }
    }
}
