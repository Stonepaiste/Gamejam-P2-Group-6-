using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CelestialBody : MonoBehaviour
{
    public bool shouldRotate = true;
    public float maxRotateSpeed = 100f;
    public bool shouldMoveY = true;
    public bool shouldMoveX = false;
    [SerializeField]
    private bool shouldJitter = false;
    public float moveScale = .3f;
    private Renderer childRenderer;
    private Camera _camera;

    private float randomPhase;
    [SerializeField]
    private float moveRate;
    private float rotateSpeed;
    private float localTime = 0f;

    private void Start()
    {
        randomPhase = Random.Range(0, 2 * Mathf.PI);
        if (!(moveRate >= 0))
        {
            moveRate = Random.Range(0.5f, 2f);
        }
        localTime = 0f;
        
        rotateSpeed = Random.Range(-maxRotateSpeed, maxRotateSpeed);
        
        _camera = Camera.main;
        childRenderer = transform.GetComponentInChildren<SpriteRenderer>();
    }
    
    private Vector3 targetPosition;
    private Vector3 currentJitter;
    private float lerpSpeed = 0.1f;
    private float jitterStrength = 1f;
    private void FixedUpdate()
    {
        localTime += Time.deltaTime;
        Vector3 movement = new Vector3(
            shouldMoveX ? Mathf.Cos(localTime * moveRate + randomPhase) * Time.deltaTime * moveScale : 0,
            shouldMoveY ? Mathf.Sin(localTime * moveRate + randomPhase) * Time.deltaTime * moveScale : 0,
            0
        );
        // transform.position += movement;
        
        targetPosition = transform.position + movement;
        jitterStrength = moveScale / 10f;
        lerpSpeed = moveRate;

        if (shouldJitter)
        {
            Vector3 jitter = new Vector3(
                (Mathf.PerlinNoise(localTime, 0) - 0.5f) * jitterStrength,
                (Mathf.PerlinNoise(0, localTime) - 0.5f) * jitterStrength,
                0
            );
            currentJitter = Vector3.Lerp(currentJitter, jitter, lerpSpeed);
            targetPosition += currentJitter;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);

        
        if (shouldRotate)
        {
            // Rotate object on z-axis with random speed
            transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
        }
        
        // Check if object is to the left of the camera
        if (!childRenderer.isVisible && transform.position.x < _camera.transform.position.x)
        {
            // ebug.Log("Destroy object");
            Destroy(gameObject);
        }
    }
}
