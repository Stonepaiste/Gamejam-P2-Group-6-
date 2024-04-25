using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CelestialBodySpawner : MonoBehaviour
{
    public static CelestialBodySpawner instance = null;
    
    public bool isSpawning = true;
    
    [SerializeField]
    private GameObject meteoritePrefab, cheesePrefab, stinkyCheesePrefab;
    
    [SerializeField]
    private float cheeseSpeed = 2f;
    [SerializeField]
    private float meteoriteSpeed = 2f;

    [SerializeField][Tooltip("Cheeses to spawn per second.")]
    private float defaultCheeseAmount = 3;
    [SerializeField][Tooltip("Meteorites to spawn per second.")]
    private float defaultMeteoriteAmount = 2;
    [SerializeField][Tooltip("Interval between stinky cheeses.")]
    private float defaultStinkyCheeseInterval = 9;
    
    [SerializeField]
    private float cheesePaddingY = 2f;
    [SerializeField]
    private float meteoritePaddingY = 1f;
    [SerializeField]
    private float moonPaddingY = 1.5f;

    private float _waveOffsetMax;
    private float _waveOffsetMin;
    private float _meteoriteOffsetMax;
    private float _meteoriteOffsetMin;
    
    [SerializeField]
    private bool randomizeMeteorites = true;
    [SerializeField]
    private bool randomizeCheeses = true;
    
    private readonly List<float> _wavePhases = new List<float>();
    private readonly List<float> _waveOffsets = new List<float>();
    private readonly List<Vector3> _cheesePositions = new List<Vector3>();
    
    
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
        public float cheeseAmount = 10;
        [Tooltip("Density of meteorites")]
        public float meteoriteAmount = 10;
        [Tooltip("Interval of stinky cheeses")]
        public float stinkyCheeseOccurrence = 1;
        public CelestialBodyRange meteoriteRange = new();
        public CelestialBodyRange cheeseRange = new();

        public void Initialize()
        {
            if (wavePhase == 0.0f)
            {
                wavePhase = Random.Range(-20f, 20f);
            }
            if (waveAmplitude == 0.0f)
            {
                waveAmplitude = Random.Range(.5f, 2f);
            }
        }
        public override string ToString()
        {
            return $"Wave Phase: {wavePhase}, Wave Offset: {waveOffset}, Wave Amplitude: {waveAmplitude}, Cheese Amount: {cheeseAmount}, Meteorite Amount: {meteoriteAmount}, Stinky Cheese Occurrence: {stinkyCheeseOccurrence}, Meteorite Range: {meteoriteRange.min} - {meteoriteRange.max}, Cheese Range: {cheeseRange.min} - {cheeseRange.max}";
        }
    }
    
    [SerializeField]
    List<MapSection> mapSections = new();

    private GameObject _cheeseRoot;
    private GameObject _meteoriteRoot;

    private Quaternion _randRotation = Quaternion.identity;
    private Quaternion _randRotation45 = Quaternion.identity;
    
    private int _stinkyCheeseCounter = 0;

    private int _stinkyCheeseInterval = 0;
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
        
        // Get viewport top and bottom in world space
        float topEdgeY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        float bottomEdgeY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        
        _meteoriteOffsetMax = topEdgeY - meteoritePaddingY;
        _meteoriteOffsetMin = bottomEdgeY + meteoritePaddingY + moonPaddingY;

        _waveOffsetMax = topEdgeY - cheesePaddingY;
        _waveOffsetMin = bottomEdgeY + cheesePaddingY + moonPaddingY;
        
        _stinkyCheeseInterval = (int)Random.Range((defaultStinkyCheeseInterval* .5f), defaultStinkyCheeseInterval * 2f);
        ClearAndSpawn();
    }

    public void ClearAndSpawn()
    {
        _randRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        _randRotation45 = Quaternion.Euler(0, 0, Random.Range(-45, 45));
        _cheeseRoot = new GameObject("Cheeses");
        _meteoriteRoot = new GameObject("Meteorites");
        
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

        if (Application.isPlaying)
        {
            _cheeseRoot.transform.parent = transform;
            _cheeseRoot.transform.position = transform.position;
            
            _meteoriteRoot.transform.parent = transform;
            _meteoriteRoot.transform.position = transform.position;
        }
        
        foreach (MapSection mapSection in mapSections)
        {
            mapSection.Initialize();
            // Debug.Log(mapSection.ToString());   

            int phaseCounter = 0;
            float wavePhase = mapSection.wavePhase;
            float waveOffset = mapSection.waveOffset;
            int phaseShiftInteval = Random.Range(5, 10);
            
            int numberOfCheeses = (int)Mathf.Round(Random.Range((float)(mapSection.cheeseAmount * .5), mapSection.cheeseAmount * 2)); // Number of cheeses to 
            
            float numberOfMeteorites = mapSection.meteoriteAmount;
            float cheeseDensity = numberOfCheeses / 10f; // Cheeses per x unit
            float meteoriteDensity = numberOfMeteorites / 10f; // Meteorites per x unit
            
            
            // TODO: Extract the sine wave generation to the upper loop to have the same data available for both cheese and meteorite generation.
            //       Currently, the meteorites are generated based on the cheeses, which is not ideal.
            // BUG: Meteorites are not created in sections without cheeses.
            
            if (waveOffset == 0.0f)
            {
                waveOffset = Random.Range(_waveOffsetMin, _waveOffsetMax);
            }
            if (mapSection.cheeseAmount == 0)
            {
                mapSection.cheeseAmount = defaultCheeseAmount;
            }
            if (mapSection.meteoriteAmount == 0)
            {
                mapSection.meteoriteAmount = defaultMeteoriteAmount;
            }
            if (mapSection.stinkyCheeseOccurrence == 0)
            {
                mapSection.stinkyCheeseOccurrence = defaultStinkyCheeseInterval;
            }
            
            if(!randomizeCheeses)
            // Spawn cheeses alone the x-axis following a sine wave
            for (float x = mapSection.cheeseRange.min + currentX; x < mapSection.cheeseRange.max + currentX; x += 1/cheeseDensity)
            {
                if (phaseCounter >= phaseShiftInteval)
                {
                    wavePhase = Random.Range(-50f, 50f);
                    phaseCounter = 0;
                    phaseShiftInteval = Random.Range(5, 10);
                    waveOffset = Random.Range(_waveOffsetMin, _waveOffsetMax);

                }
                
                float y = Mathf.Sin(x + wavePhase) + waveOffset;
                
                Vector3 position = new Vector3(x, y, 0);

                GameObject cheese = Instantiate(CheeseOrStinkyCheese(_stinkyCheeseInterval), _cheeseRoot.transform.position + position, _randRotation45);
                cheese.transform.parent = _cheeseRoot.transform;
                
                _wavePhases.Add(wavePhase); 
                _waveOffsets.Add(waveOffset);
                _cheesePositions.Add(position);
                
                phaseCounter++;
                _stinkyCheeseCounter++;
            }

            if (!randomizeMeteorites)
            {
                // Spawn meteorites around the cheeses randomly on the y-axis
                for (float x = mapSection.meteoriteRange.min + currentX; x < mapSection.meteoriteRange.max + currentX; x += 1/meteoriteDensity)
                {
                    float meteoriteOffset = Random.Range(_meteoriteOffsetMin, _meteoriteOffsetMax);
                    
                    List<float> cheesePositionsX = _cheesePositions.ConvertAll(vector => vector.x);
                    int index = cheesePositionsX.BinarySearch(x);
                    if (index < 0)
                    {
                        index = ~index;
                    }
                    int lowerIndex = Math.Max(0, index -1);
                    int upperIndex = Math.Min(_cheesePositions.Count - 1, index + 1);
                    
                    float t = (x - _cheesePositions[lowerIndex].x) / (_cheesePositions[upperIndex].x - _cheesePositions[lowerIndex].x);
                    wavePhase = Mathf.Lerp(_wavePhases[lowerIndex], _wavePhases[upperIndex], t);
                    waveOffset = Mathf.Lerp(_waveOffsets[lowerIndex], _waveOffsets[upperIndex], t);
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
                    
                    GameObject meteorite = Instantiate(meteoritePrefab, _meteoriteRoot.transform.position + position, _randRotation);
                    meteorite.transform.parent = _meteoriteRoot.transform;
                }
            }

            currentX += Math.Max(mapSection.cheeseRange.max, mapSection.meteoriteRange.max);
        }
    }
    
    private GameObject CheeseOrStinkyCheese(float interval)
    {
       GameObject cheese;
        if (_stinkyCheeseCounter >= _stinkyCheeseInterval)
        {
            _stinkyCheeseCounter = 0;
            cheese = stinkyCheesePrefab;
            _stinkyCheeseInterval = (int)Random.Range(Math.Max((int)(interval * .5), 4), interval * 2);
        }
        else
        {
            cheese = cheesePrefab;
        }
        _stinkyCheeseCounter++;
        return cheese;
    }
    
    float _currentMeteoriteY = 0;
    private void SpawnRandomCelestialBody(GameObject celestialBodyPrefab)
    {
        float rightEdgeX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float y = celestialBodyPrefab == cheesePrefab ? Random.Range(_waveOffsetMin, _waveOffsetMax) : Random.Range(_meteoriteOffsetMin, _meteoriteOffsetMax);
        if (celestialBodyPrefab == meteoritePrefab)
        {
            _currentMeteoriteY = y;
        }
        // Move cheese out of the way of the meteorite
        else if (Math.Abs(_currentMeteoriteY - y) < 1f)
        {
            // Debug.Log("Moved cheese: " + Math.Abs(_currentMeteoriteY - y));

            if (_currentMeteoriteY - y < 0)
            {
                y -= 1;
            }
            else
            {
                y += 1;
            }
        }
        Vector3 position = new Vector3(rightEdgeX, y, 0);
        GameObject celestialObject = Instantiate(celestialBodyPrefab, position, celestialBodyPrefab == cheesePrefab ? _randRotation45 : _randRotation);

        celestialObject.transform.parent = celestialBodyPrefab == meteoritePrefab ? _meteoriteRoot.transform : _cheeseRoot.transform;
        CelestialBody celestialBody = celestialObject.GetComponent<CelestialBody>();
    }

    private float _meteoriteTimer = 0;
    private float _cheeseTimer = 0;
    private void Update()
    {
        // Move celestial bodies
        _meteoriteTimer += Time.deltaTime;
        _cheeseTimer += Time.deltaTime;
        
        Vector3 moveVector = new Vector3(-Time.deltaTime, 0, 0);
        _cheeseRoot.transform.position += cheeseSpeed * moveVector;
        _meteoriteRoot.transform.position += meteoriteSpeed * moveVector;
        
        if (!isSpawning)
        {
            return;
        }
        
        // Spawn celestial bodies
        var flippedMeteoriteAmount = 1f / defaultMeteoriteAmount * 10;
        var flippedCheeseAmount = 1f / defaultCheeseAmount * 10;

        if (_meteoriteTimer > Random.Range(flippedMeteoriteAmount * .5f, flippedMeteoriteAmount * 2f))
        {
            _meteoriteTimer = 0;
            if (randomizeMeteorites) SpawnRandomCelestialBody(meteoritePrefab);
        }

        if (_cheeseTimer > Random.Range(flippedCheeseAmount * .5f, flippedCheeseAmount * 2f))
        {
            _cheeseTimer = 0;
            if (randomizeCheeses) SpawnRandomCelestialBody(CheeseOrStinkyCheese(_stinkyCheeseInterval));
        }
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
    }
    public void StartSpawning()
    {
        isSpawning = true;
    }
}
