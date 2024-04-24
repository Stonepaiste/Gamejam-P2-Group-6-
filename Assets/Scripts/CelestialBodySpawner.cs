using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CelestialBodySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject meteoritePrefab, cheesePrefab, stinkyCheesePrefab;
    
    [SerializeField]
    private float movementSpeed = 0.01f;
    [SerializeField]
    private static int defaultCheeseAmount = 3;
    [SerializeField]
    private static int defaultMeteoriteAmount = 2;
    [SerializeField]
    private static int defaultStinkyCheeseInterval= 9;
    
    [SerializeField]
    private static float waveOffsetMax = 4f;
    [SerializeField]
    private static float waveOffsetMin = -3f;
    
    [SerializeField]
    private float meteoriteOffsetMax = 6f;
    [SerializeField]
    private float meteoriteOffsetMin = -3f;
    
    private List<float> wavePhases = new List<float>();
    private List<float> waveOffsets = new List<float>();
    private List<Vector3> cheesePositions = new List<Vector3>();
    
    
    [Serializable]
    public class CelestialBodyRange
    {
        [Range(0, 10)]
        public int min = 0;
        [Range(0, 10)]
        public int max = 10;
    }
    
    
    [Serializable]
    public class MapSection
    {
        [Tooltip("Moves the sine wave along the x-axis")]
        public float wavePhase;
        [Tooltip("Moves the sine wave along the y-axis")]
        public float waveOffset;
        [Tooltip("Amplitude of the sine wave")]
        public float waveAmplitude;
        [Tooltip("Denisity of cheeses")]
        public int cheeseAmount = 10;
        [Tooltip("Density of meteorites")]
        public int meteoriteAmount = 10;
        [Tooltip("Interval of stinky cheeses")]
        public int stinkyCheeseOccurrence = 1;
        public CelestialBodyRange meteoriteRange = new();
        public CelestialBodyRange cheeseRange = new();

        public void Initialize()
        {
            if (wavePhase == 0.0f)
            {
                wavePhase = Random.Range(-20f, 20f);
            }
            if (waveOffset == 0.0f)
            {
                waveOffset = Random.Range(waveOffsetMin, waveOffsetMax);
            }
            if (waveAmplitude == 0.0f)
            {
                waveAmplitude = Random.Range(.5f, 2f);
            }
            if (cheeseAmount == 0)
            {
                cheeseAmount = defaultCheeseAmount;
            }
            if (meteoriteAmount == 0)
            {
                meteoriteAmount = defaultMeteoriteAmount;
            }
            if (stinkyCheeseOccurrence == 0)
            {
                stinkyCheeseOccurrence = defaultStinkyCheeseInterval;
            }
        }
        public override string ToString()
        {
            return $"Wave Phase: {wavePhase}, Wave Offset: {waveOffset}, Wave Amplitude: {waveAmplitude}, Cheese Amount: {cheeseAmount}, Meteorite Amount: {meteoriteAmount}, Stinky Cheese Occurrence: {stinkyCheeseOccurrence}, Meteorite Range: {meteoriteRange.min} - {meteoriteRange.max}, Cheese Range: {cheeseRange.min} - {cheeseRange.max}";
        }
    }
    
    [SerializeField]
    List<MapSection> mapSections = new();

    private void Awake()
    {
        ClearAndSpawn();
    }

    public void ClearAndSpawn()
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
        
        Spawn();
    }
    
    void Spawn()
    {
        float currentX = 0;
        foreach (MapSection mapSection in mapSections)
        {
            mapSection.Initialize();
            Debug.Log(mapSection.ToString());   
            
            int phaseCounter = 0, stinkyCheeseCounter = 0;
            float wavePhase = mapSection.wavePhase;
            float waveOffset = mapSection.waveOffset;
            int phaseShiftInteval = Random.Range(5, 10);
            
            int stinkyCheeseInterval = Random.Range((int)(mapSection.stinkyCheeseOccurrence * .5), mapSection.stinkyCheeseOccurrence * 2);
            
            int numberOfCheeses = (int)Mathf.Round(Random.Range((float)(mapSection.cheeseAmount * .5), mapSection.cheeseAmount * 2)); // Number of cheeses to spawn
            int numberOfMeteorites = mapSection.meteoriteAmount;
            float cheeseDensity = numberOfCheeses / 10f; // Cheeses per x unit
            float meteoriteDensity = numberOfMeteorites / 10f; // Meteorites per x unit
            
            
            // TODO: Extract the sine wave generation to the upper loop to have the same data available for both cheese and meteorite generation.
            //       Currently, the meteorites are generated based on the cheeses, which is not ideal.
            // BUG: Meteorites are not created in sections without cheeses.
            
            
            // Spawn cheeses alone the x-axis following a sine wave
            for (float x = mapSection.cheeseRange.min + currentX; x < mapSection.cheeseRange.max + currentX; x += 1/cheeseDensity)
            {
                if (phaseCounter >= phaseShiftInteval)
                {
                    wavePhase = Random.Range(-50f, 50f);
                    phaseCounter = 0;
                    phaseShiftInteval = Random.Range(5, 10);
                    waveOffset = Random.Range(waveOffsetMin, waveOffsetMax);

                }
                
                float y = Mathf.Sin(x + wavePhase) + waveOffset;
                
                Vector3 position = new Vector3(x, y, 0);
                Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-45, 45));

                GameObject cheese;
                if (stinkyCheeseCounter >= stinkyCheeseInterval)
                {
                    cheese = Instantiate(stinkyCheesePrefab, transform.position + position, rotation);
                    stinkyCheeseCounter = 0;
                    stinkyCheeseInterval = Random.Range((int)(mapSection.stinkyCheeseOccurrence * .5), mapSection.stinkyCheeseOccurrence * 2);
                }
                else
                {
                    cheese = Instantiate(cheesePrefab, transform.position + position, rotation);
                }
                cheese.transform.parent = transform;
                
                wavePhases.Add(wavePhase);
                waveOffsets.Add(waveOffset);
                cheesePositions.Add(position);
                
                phaseCounter++;
                stinkyCheeseCounter++;
            }
            
            // Spawn meteorites around the cheeses randomly on the y-axis
            for (float x = mapSection.meteoriteRange.min + currentX; x < mapSection.meteoriteRange.max + currentX; x += 1/meteoriteDensity)
            {
                Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                float meteoriteOffset = Random.Range(meteoriteOffsetMin, meteoriteOffsetMax);
                
                List<float> cheesePositionsX = cheesePositions.ConvertAll(vector => vector.x);
                int index = cheesePositionsX.BinarySearch(x);
                if (index < 0)
                {
                    index = ~index;
                }
                int lowerIndex = Math.Max(0, index -1);
                int upperIndex = Math.Min(cheesePositions.Count - 1, index + 1);
                
                float t = (x - cheesePositions[lowerIndex].x) / (cheesePositions[upperIndex].x - cheesePositions[lowerIndex].x);
                wavePhase = Mathf.Lerp(wavePhases[lowerIndex], wavePhases[upperIndex], t);
                waveOffset = Mathf.Lerp(waveOffsets[lowerIndex], waveOffsets[upperIndex], t);
                float y = Mathf.Sin(x + wavePhase) + waveOffset;
                
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
                
                GameObject meteorite = Instantiate(meteoritePrefab, transform.position + position, rotation);
                meteorite.transform.parent = transform;
                
                
                // DEBUG - Display distances between meteorites and cheeses
                List<float> distances = new List<float>();

                foreach (Vector3 cheesePosition in cheesePositions)
                {
                    if (Math.Abs(cheesePosition.x - x) <= 1)
                    {
                        float distance = Vector3.Distance(cheesePosition, position);
                        distances.Add(distance);
                    }
                }
                
                string distancesStr = "";

                foreach (float distance in distances)
                {
                    distancesStr += Math.Round(distance, 2).ToString() + "\n";
                }
                /*GameObject textObject = new GameObject("Text");
                textObject.transform.parent = meteorite.transform;
                TextMesh textMesh = textObject.AddComponent<TextMesh>();
                textMesh.text = distancesStr;
                textMesh.fontSize = 25;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.characterSize = 0.1f;
                textMesh.offsetZ = -1f;
                textMesh.color = Color.white;
                textObject.transform.position = meteorite.transform.position + new Vector3(0, 0, 0);*/
            }

            currentX += Math.Max(mapSection.cheeseRange.max, mapSection.meteoriteRange.max);
        }
    }

    private void FixedUpdate()
    {
        this.transform.position += new Vector3(-movementSpeed, 0, 0);
    }
}
