using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCheese : MonoBehaviour
{
    int cheeseCounter = 0;

    private void Awake()
    {
        cheeseCounter = 0;
    }

    void Pickup()
    {
        cheeseCounter++;
        Debug.Log("Cheese Counter: " + cheeseCounter);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cheese")
        {
            Pickup();
            Destroy(collision.gameObject);
        }
    }
}
