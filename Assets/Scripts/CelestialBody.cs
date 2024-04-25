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
    private Renderer _childRenderer;
    private Camera _camera;

    private float _randomPhase;
    [SerializeField]
    private float moveRate;
    private float _rotateSpeed;
    private float _localTime = 0f;

    private void Start()
    {
        _randomPhase = Random.Range(0, 2 * Mathf.PI);
        if (!(moveRate >= 0))
        {
            moveRate = Random.Range(0.5f, 2f);
        }
        _localTime = 0f;
        
        _rotateSpeed = Random.Range(-maxRotateSpeed, maxRotateSpeed);
        
        _camera = Camera.main;
        _childRenderer = transform.GetComponentInChildren<SpriteRenderer>();
    }
    
    private Vector3 _targetPosition;
    private Vector3 _currentJitter;
    private float _lerpSpeed = 0.1f;
    private float _jitterStrength = 1f;
    private void FixedUpdate()
    {
        _localTime += Time.deltaTime;
        Vector3 movement = new Vector3(
            shouldMoveX ? Mathf.Cos(_localTime * moveRate + _randomPhase) * Time.deltaTime * moveScale : 0,
            shouldMoveY ? Mathf.Sin(_localTime * moveRate + _randomPhase) * Time.deltaTime * moveScale : 0,
            0
        );
        // transform.position += movement;
        
        _targetPosition = transform.position + movement;
        _jitterStrength = moveScale / 10f;
        _lerpSpeed = moveRate;

        if (shouldJitter)
        {
            Vector3 jitter = new Vector3(
                (Mathf.PerlinNoise(_localTime, 0) - 0.5f) * _jitterStrength,
                (Mathf.PerlinNoise(0, _localTime) - 0.5f) * _jitterStrength,
                0
            );
            _currentJitter = Vector3.Lerp(_currentJitter, jitter, _lerpSpeed);
            _targetPosition += _currentJitter;
        }

        transform.position = Vector3.Lerp(transform.position, _targetPosition, _lerpSpeed);

        
        if (shouldRotate)
        {
            // Rotate object on z-axis with random speed
            transform.Rotate(Vector3.forward * Time.deltaTime * _rotateSpeed);
        }
        
        // Check if object is to the left of the camera
        if (!_childRenderer.isVisible && transform.position.x < _camera.transform.position.x)
        {
            // ebug.Log("Destroy object");
            Destroy(gameObject);
        }
    }
}
