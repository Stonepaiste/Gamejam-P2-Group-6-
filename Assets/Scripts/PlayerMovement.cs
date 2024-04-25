using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    bool canPlaySound = false;
    void Start()
    {
        body= GetComponent<Rigidbody2D>();
    }

    
    void FixedUpdate()
    {
        AddForce();
    }

    private void AddForce()
    {
        if (Input.GetMouseButton(0))
        {
            canPlaySound = Input.GetMouseButtonDown(0);
            body.AddForce(new Vector3(0, 50, 0), ForceMode2D.Force);
            //Debug.Log("Mouse Clicked" + canPlaySound);

        }
        else if (Input.GetMouseButtonUp(0))
        {
            body.velocity *= 0.25f;


        }
    }
}
