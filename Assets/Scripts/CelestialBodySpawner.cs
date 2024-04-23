using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject MeteoritePrefab;
    [SerializeField]
    private GameObject CheesePrefab;
    [SerializeField]
    private GameObject StinkyCheesePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        int counter = 0;
        float phaseShift = 0;
        int phaseShiftInteval = 10;
        // Spawn cheeses alone the x-axis following a sine wave
        for (int i = 0; i < 100; i++)
        {
            float x = i * 1;
            if (counter >= phaseShiftInteval)
            {
                phaseShift = Random.Range(-5f, 5f);
                // phaseShiftInteval = Random.Range(10, 20);
                counter = 0;
            }
            float y = Mathf.Sin(x + phaseShift);
            Vector3 position = new Vector3(x, y, 0);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            GameObject cheese = Instantiate(CheesePrefab, transform.position + position, rotation);
            cheese.transform.parent = this.transform;
            counter++;
        }
        
        // Spawn meteorites around the cheeses randomly on the y-axis
        for (int i = 0; i < 100; i++)
        {
            float x = i * 1;
            // float y = 0;
            float y = Mathf.Sin(x) + 1;
            Vector3 position = new Vector3(x, y, 0);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            GameObject meteorite = Instantiate(MeteoritePrefab, transform.position + position + new Vector3(0, Random.Range(-3, 6), 0), rotation);
            meteorite.transform.parent = this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
