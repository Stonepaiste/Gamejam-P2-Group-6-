using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonRotation : MonoBehaviour
{
    public static MoonRotation Instance = null;
    
    public bool isRotating = true;

    private float _currentSpeed;
    [SerializeField] float moonSpeed = 3f;
    [SerializeField] float changeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
        _currentSpeed = moonSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            RotateMoon();
        }
    }


    void RotateMoon()
    {
        transform.Rotate(Vector3.forward * _currentSpeed * Time.deltaTime);
    }
    
    public void StopRotation()
    {
        StartCoroutine(ChangeRotationGradually(0));
        isRotating = false;
    }
    
    public void StartRotation()
    {
        StartCoroutine(ChangeRotationGradually(moonSpeed));
        isRotating = true;
    }
    
    private IEnumerator ChangeRotationGradually(float targetSpeed)
    {
        float initialSpeed = _currentSpeed;
        float timeElapsed = 0;
        while (timeElapsed < changeDuration)
        {
            _currentSpeed = Mathf.Lerp(initialSpeed, targetSpeed, timeElapsed / changeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _currentSpeed = targetSpeed;
    }
}
