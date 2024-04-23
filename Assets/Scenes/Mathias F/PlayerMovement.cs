using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody body;

    void Start()
    {
        body= GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            body.AddForce(new Vector3(0, 50, 0),ForceMode.Acceleration);
            Debug.Log("Mouse Clicked");
        }
        else if (Input.GetMouseButtonUp(0))
        {
           body.velocity*=0.25f;
        }
    }
}
