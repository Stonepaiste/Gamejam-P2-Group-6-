using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMeteorite : MonoBehaviour
{

    [SerializeField] GameObject[] meteorites; 

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(meteorites[Random.Range(0, meteorites.Length)], transform.position, Quaternion.identity, this.transform);
    }
}
