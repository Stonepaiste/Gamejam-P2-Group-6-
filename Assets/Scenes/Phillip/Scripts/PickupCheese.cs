using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupCheese : MonoBehaviour
{
    public int cheeseCounter = 0;
    [SerializeField] float fallAmount = 0f;
    [SerializeField] float fallSpeed = 0f;
    [SerializeField] bool isStinkyCheese = false;

    private void Awake()
    {
        cheeseCounter = 0;

    }

    private void Update()
    {
        StopMovement();
    }

    private void StopMovement()
    {
        if (isStinkyCheese)
        {
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            this.gameObject.GetComponent<PlayerMovement>().enabled = false;
            fallSpeed = -2f;
            //transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed * Time.deltaTime, transform.position.z);
            Physics.IgnoreLayerCollision(0, 1);
            if (Input.acceleration.y < -0.5f)
            {
                isStinkyCheese = false;
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                this.gameObject.GetComponent<PlayerMovement>().enabled = true;
            }
        }
    }

    void Pickup()
    {
        cheeseCounter++;
        Debug.Log("Cheese Counter: " + cheeseCounter);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cheese" && !isStinkyCheese)
        {
            Pickup();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "StinkyCheese")
        {
            isStinkyCheese = true;
            Destroy(collision.gameObject);
        }
    }
}
