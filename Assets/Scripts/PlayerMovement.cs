using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;

    void Start()
    {
        body= GetComponent<Rigidbody2D>();
    }

    
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            body.AddForce(new Vector3(0, 50, 0),ForceMode2D.Force);
            // Debug.Log("Mouse Clicked");
        }
        else if (Input.GetMouseButtonUp(0))
        {
           body.velocity*=0.25f;
        }
    }
}
