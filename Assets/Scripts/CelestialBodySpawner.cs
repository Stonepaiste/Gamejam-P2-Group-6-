using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CelestialBodySpawner : MonoBehaviour
{
    [FormerlySerializedAs("MeteoritePrefab")] [SerializeField]
    private GameObject meteoritePrefab;
    [FormerlySerializedAs("CheesePrefab")] [SerializeField]
    private GameObject cheesePrefab;
    [FormerlySerializedAs("StinkyCheesePrefab")] [SerializeField]
    private GameObject stinkyCheesePrefab;
    
    [SerializeField]
    private int cheeseAmount = 100;
    [SerializeField]
    private int meteoriteAmount = 500;
    [SerializeField]
    private int stinkyCheeseOccurrence= 10;
    
    [SerializeField]
    private static float waveOffsetMax = 4f;
    [SerializeField]
    private static float waveOffsetMin = -3f;
    
    [SerializeField]
    private float meteoriteOffsetMax = 6f;
    [SerializeField]
    private float meteoriteOffsetMin = -3f;
    
    private List<float> phaseShifts = new List<float>();
    private List<float> waveOffsets = new List<float>();
    private List<Vector3> cheesePositions = new List<Vector3>();

    /*private void Awake()
    {
        _phaseShift = Random.Range(-20f, 20f);
        _waveOffset = Random.Range(waveOffsetMin, waveOffsetMax);
    }*/

    // Start is called before the first frame update
    
    private TextMesh textMesh;

    void Start()
    {
        int phaseCounter = 0;
        float phaseShift = Random.Range(-20f, 20f);
        float waveOffset = Random.Range(waveOffsetMin, waveOffsetMax);
        int phaseShiftInteval = Random.Range(5, 10);
        
        int stinkyCheeseCounter = 0;
        int stinkyCheeseInterval = Random.Range((int)(stinkyCheeseOccurrence * .5), stinkyCheeseOccurrence * 2);
        
        
        int numberOfCheeses = (int)Mathf.Round(Random.Range((float)(cheeseAmount * .5), cheeseAmount * 2)); // Number of cheeses to spawn
        // int numberOfMeteorites = (int)Mathf.Round(Random.Range((float)(meteoriteAmount * .5), meteoriteAmount * 2));; // Number of meteorites to spawn
        int numberOfMeteorites = meteoriteAmount;
        float cheeseDensity = numberOfCheeses / 100f; // Cheeses per x unit
        float meteoriteDensity = numberOfMeteorites / 100f; // Meteorites per x unit
        
        
        
        // Spawn cheeses alone the x-axis following a sine wave
        for (float x = 0; x < numberOfCheeses / cheeseDensity; x += 1/cheeseDensity)
        {
            // float x = i * 1;
            if (phaseCounter >= phaseShiftInteval)
            {
                phaseShift = Random.Range(-50f, 50f);
                phaseCounter = 0;
                phaseShiftInteval = Random.Range(5, 10);
                waveOffset = Random.Range(waveOffsetMin, waveOffsetMax);
                Debug.Log("Phase shift: " + phaseShift);
            }
            
            float y = Mathf.Sin(x + phaseShift) + waveOffset;
            // float y = Mathf.Sin(x);
            Debug.Log("PhaseShift: " + phaseShift + " x: " + x + " y: " + y);
            Vector3 position = new Vector3(x, y, 0);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

            GameObject cheese;
            if (stinkyCheeseCounter >= stinkyCheeseInterval)
            {
                cheese = Instantiate(stinkyCheesePrefab, transform.position + position, rotation);
                stinkyCheeseCounter = 0;
                stinkyCheeseInterval = Random.Range((int)(stinkyCheeseOccurrence * .5), stinkyCheeseOccurrence * 2);
                Debug.Log("Stinky cheese spawned at " + position);
            }
            else
            {
                cheese = Instantiate(cheesePrefab, transform.position + position, rotation);
            }
            cheese.transform.parent = this.transform;
            
            phaseShifts.Add(phaseShift);
            waveOffsets.Add(waveOffset);
            cheesePositions.Add(position);
            
            phaseCounter++;
            stinkyCheeseCounter++;
        }
        
        // Spawn meteorites around the cheeses randomly on the y-axis
        for (float x = 0; x < numberOfMeteorites / meteoriteDensity; x += 1/meteoriteDensity)
        {
            Debug.Log("Meteorite spawn" + " _phaseShift: " + phaseShift + " _waveOffset: " + waveOffset);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            // float y = Mathf.Sin(x + _phaseShift) + _waveOffset;
            // float y = Mathf.Sin(x);
            float meteoriteOffset = Random.Range(meteoriteOffsetMin, meteoriteOffsetMax);
            // float meteoriteOffset = 0f;
            
            // DEBUG OBJECTs
            /*GameObject oriMeteorite = Instantiate(meteoritePrefab, transform.position + new Vector3(x, y, 1), rotation);
            oriMeteorite.transform.parent = this.transform;
            SpriteRenderer spriteRenderer = oriMeteorite.transform.GetChild(0).GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1, 0, 0, 1);
            
            
            float distanceFromCheese = Mathf.Abs(meteoriteOffset -y);
            // Add a text object to the Gameobject that shows the offset
            GameObject textObject = new GameObject("Text");
            textObject.transform.parent = oriMeteorite.transform;
            TextMesh textMesh = textObject.AddComponent<TextMesh>();
            textMesh.text = Math.Round(meteoriteOffset, 2).ToString() + "\n" + distanceFromCheese.ToString() + "\n" + (meteoriteOffset - y).ToString();
            textMesh.fontSize = 25;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.characterSize = 0.1f;
            textMesh.offsetZ = -10f;
            textMesh.color = Color.white;
            textObject.transform.position = oriMeteorite.transform.position + new Vector3(0, 0, 0);*/
            
            List<float> cheesePositionsX = cheesePositions.ConvertAll(vector => vector.x);
            
            int index = cheesePositionsX.BinarySearch(x);
            if (index < 0)
            {
                index = ~index;
            }
            int lowerIndex = Math.Max(0, index -1);
            int upperIndex = Math.Min(cheesePositions.Count - 1, index + 1);
            
            float t = (x - cheesePositions[lowerIndex].x) / (cheesePositions[upperIndex].x - cheesePositions[lowerIndex].x);
            phaseShift = Mathf.Lerp(phaseShifts[lowerIndex], phaseShifts[upperIndex], t);
            waveOffset = Mathf.Lerp(waveOffsets[lowerIndex], waveOffsets[upperIndex], t);
            float y = Mathf.Sin(x + phaseShift) + waveOffset;
            
            Vector3 position;
            
            if (Math.Abs(meteoriteOffset - y) < 1f)
            {
                if (meteoriteOffset -y < 0)
                {
                    position = new Vector3(x, meteoriteOffset - 1, 0);
                }
                else
                {
                    position = new Vector3(x, meteoriteOffset + 1, 0);
                }
            }
            else
            {
                position = new Vector3(x, meteoriteOffset, 0);
            }
            
            List<float> distances = new List<float>();

            foreach (Vector3 cheesePosition in cheesePositions)
            {
                if (Math.Abs(cheesePosition.x - x) <= 1)
                {
                    float distance = Vector3.Distance(cheesePosition, position);
                    distances.Add(distance);
                }
            }
            
            GameObject meteorite = Instantiate(meteoritePrefab, transform.position + position, rotation);
            meteorite.transform.parent = this.transform;
            
            string distancesStr = "";

            foreach (float distance in distances)
            {
                distancesStr += distance.ToString() + "\n";
            }
            GameObject textObject = new GameObject("Text");
            textObject.transform.parent = meteorite.transform;
            TextMesh textMesh = textObject.AddComponent<TextMesh>();
            textMesh.text = distancesStr;
            textMesh.fontSize = 25;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.characterSize = 0.1f;
            textMesh.offsetZ = -1f;
            textMesh.color = Color.white;
            textObject.transform.position = meteorite.transform.position + new Vector3(0, 0, 0);
        }
    }
}
