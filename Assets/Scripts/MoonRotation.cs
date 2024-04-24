using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonRotation : MonoBehaviour
{
    [SerializeField] float moonSpeed = 3f;

    // Update is called once per frame
    void Update()
    {
        RotateMoon();
    }


    void RotateMoon()
    {
        transform.Rotate(Vector3.forward * moonSpeed * Time.deltaTime);
    }
}
