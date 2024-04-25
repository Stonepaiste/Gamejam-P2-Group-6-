using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonRotation : MonoBehaviour
{
    public static MoonRotation instance = null;
    
    public bool isRotating = true;

    private float currentSpeed;
    [SerializeField] float moonSpeed = 3f;
    [SerializeField] float stopDuration = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
        currentSpeed = moonSpeed;
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
        transform.Rotate(Vector3.forward * currentSpeed * Time.deltaTime);
    }
    
    public void StopRotation()
    {
        isRotating = false;
        StartCoroutine(StopRotationGradually());
    }
    
    private IEnumerator StopRotationGradually()
    {
        float timeElapsed = 0;
        while (timeElapsed < stopDuration)
        {
            currentSpeed = Mathf.Lerp(moonSpeed, 0, timeElapsed / stopDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        currentSpeed = 0;
        isRotating = false;
    }
}
