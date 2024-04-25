using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomMeteorite : MonoBehaviour
{

    [SerializeField] GameObject[] meteorites; 

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
            DestroyImmediate(child);
        }
        
        Instantiate(meteorites[Random.Range(0, meteorites.Length)], transform.position, Quaternion.identity, this.transform);
    }
}
