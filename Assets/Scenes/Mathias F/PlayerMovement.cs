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
        if (Input.GetMouseButtonDown(0))
        {
            body.AddForce(new Vector3(0, 500, 0),ForceMode.Force);
            Debug.Log("Mouse Clicked");
        }
    }
}
