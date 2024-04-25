using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance = null;
    
    private bool _hasEnded = false;
    private bool _hasStoppedAtCrater = false;

    private MoonCrater _closestCrater;
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
    }
    public void GameOver()
    {
        Debug.Log("Game Over!");
        PlayerVFX.Instance.Die();
        End();
    }
    public void GameWin()
    {
        Debug.Log("Game Win!");
        End();
    }
    
    void Start()
    {
        Debug.Log("Game Start!");
        _hasStoppedAtCrater = false;
        _hasEnded = false;
        PickupCheese.Instance.ResetCheese();
        Liv.Instance.Initiate();
        CelestialBodySpawner.Instance.StartSpawning();
        ResumeMovement();
    }

    void End()
    {
        Debug.Log("Game Stop!");
        CelestialBodySpawner.Instance.StopSpawning();
        _hasEnded = true;
    }
    
    void StopAtCrater()
    {
        // .Log("Game Stop at Crater!");
        // Get current player position
        Vector3 playerPosition = PlayerMovement.Instance.transform.position;
        float craterDistance;
        (_closestCrater, craterDistance) = CraterGenerator.Instance.GetClosestCraterRight(playerPosition);
        if (_closestCrater != null)
        {
            Debug.Log("Player Position: " + playerPosition + " Closest Crater: " + _closestCrater.transform.position + " Distance: " + craterDistance);
        }
        if (craterDistance < 0.1f)
        {
            Debug.Log("Game Stop at Crater!");
            _hasStoppedAtCrater = true;
            
            // MoonRotation.instance.isRotating = false;
            MoonRotation.Instance.StopRotation();
            StopGame();
        }
    }

    private void StopGame()
    {
        Debug.Log("Stop Movement!");
        PlayerMovement.Instance.enabled = false;
        PickupCheese.Instance.enabled = false;
        PlayerMovement.Instance.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        PlayerMovement.Instance.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        // Wait for 3 seconds before resuming movement
        StartCoroutine(WaitThenResume(3));
    }
    
    private IEnumerator WaitThenResume(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _closestCrater.Patch();
        Start();
    }

    public void ResumeMovement()
    {
        // MoonRotation.instance.isRotating = true;
        MoonRotation.Instance.StartRotation();
        PlayerMovement.Instance.enabled = true;
        PickupCheese.Instance.enabled = true;
        PlayerMovement.Instance.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
    
    private void Update()
    {
        if (PlayerMovement.Instance != null && !_hasStoppedAtCrater && _hasEnded)
        {
            StopAtCrater();
        }
    }
}
