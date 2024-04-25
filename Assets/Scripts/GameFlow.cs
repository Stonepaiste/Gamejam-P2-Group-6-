using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameFlow : MonoBehaviour
{
    public static GameFlow instance = null;
    
    private bool hasEnded = false;
    private bool hasStoppedAtCrater = false;
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
    }
    public void GameOver()
    {
        Debug.Log("Game Over!");
        End();
    }
    public void GameWin()
    {
        Debug.Log("Game Win!");
        End();
    }
    
    public void Restart()
    {
        Debug.Log("Restart!");
    }
    
    void Start()
    {
        Debug.Log("Game Start!");
        CelestialBodySpawner.instance.StartSpawning();
    }

    void End()
    {
        Debug.Log("Game Stop!");
        CelestialBodySpawner.instance.StopSpawning();
        hasEnded = true;
    }
    
    void StopAtCrater()
    {
        // .Log("Game Stop at Crater!");
        // Get current player position
        Vector3 playerPosition = PlayerMovement.instance.transform.position;
        (MoonCrater closestCrater, float craterDistance) = CraterGenerator.instance.GetClosestCraterRight(playerPosition);
        Debug.Log("Player Position: " + playerPosition + " Closest Crater: " + closestCrater.transform.position + " Distance: " + craterDistance);
        if (craterDistance < 0.12f)
        {
            Debug.Log("Game Stop at Crater!");
            hasStoppedAtCrater = true;
            StopMovement();
        }
    }

    private void StopMovement()
    {
        Debug.Log("Stop Movement!");
        MoonRotation.instance.isRotating = false;
        PlayerMovement.instance.enabled = false;
        PickupCheese.instance.enabled = false;
        PlayerMovement.instance.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        PlayerMovement.instance.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void ResumeMovement()
    {
        MoonRotation.instance.isRotating = true;
        PlayerMovement.instance.enabled = true;
        PickupCheese.instance.enabled = true;
        PlayerMovement.instance.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
    
    private void Update()
    {
        if (!hasStoppedAtCrater && hasEnded)
        {
            StopAtCrater();
        }
    }
}
