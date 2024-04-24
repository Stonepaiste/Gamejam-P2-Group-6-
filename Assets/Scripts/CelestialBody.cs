using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    private Renderer childRenderer;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        childRenderer = transform.GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        // Check if object is to the left of the camera
            Debug.Log("Camera position: " + transform.position.x);
        if (!childRenderer.isVisible && transform.position.x < _camera.transform.position.x)
        {
            Debug.Log("Destroy object");
            Destroy(gameObject);
        }
    }
}
