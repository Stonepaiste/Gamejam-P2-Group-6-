using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance = null;
    
    [SerializeField] float carterStopPositionY = 0f;
    [SerializeField] float carterStopDistance = 0f;
    [SerializeField] float waitForPlayerMovementTime = 3;
    
    private bool _hasEnded = false;
    private bool _hasStoppedAtCrater = false;
    
    private float PlayerMovementRemainingTime = 0;

    private MoonCrater _closestCrater;
    
    [SerializeField] private GameObject countdownTimer;
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject gameUI;
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
        StopPlayerActions();
        End();
    }
    public void GameWin()
    {
        Debug.Log("Game Win!");
        End();
    }

    private void Start()
    {
        Begin();
    }

    void Begin()
    {
        Debug.Log("Game Start!");
        _hasStoppedAtCrater = false;
        _hasEnded = false;
        Liv.Instance.Initiate();
        PlayerVFX.Instance.Initiate();
        PickupCheese.Instance.ResetCheese();
        PickupCheese.Instance.canPickupCheese = true;
        CelestialBodySpawner.Instance.StartSpawning();
        PlayerMovement.Instance.enabled = false;
        PlayerMovement.Instance.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        
        gameUI.SetActive(true);
        countdownTimer.SetActive(true);
        gameWinScreen.SetActive(false);
        
        ResumeMovement();
    }

    void End()
    {
        Debug.Log("Game Stop!");
        CelestialBodySpawner.Instance.StopSpawning();
        _hasEnded = true;
        gameUI.SetActive(false);
        gameWinScreen.SetActive(true);
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
            // Debug.Log("Player Position: " + playerPosition + " Closest Crater: " + _closestCrater.transform.position + " Distance: " + craterDistance);
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
        Debug.Log("Stop Game!");
        StopPlayerActions();
        StopPlayerMovement();
        // ONLY FOR TESTING
        StartCoroutine(WaitThenResumePatch(3));
    }

    private void StopPlayerActions()
    {
        Debug.Log("Stop Player Actions!");
        PlayerMovement.Instance.enabled = false;
        PickupCheese.Instance.canPickupCheese = false;
    }
    private void StopPlayerMovement()
    {
        Debug.Log("Stop Player Movement!");
        PlayerMovement.Instance.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        PlayerMovement.Instance.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    public void ResumeMovement()
    {
        MoonRotation.Instance.StartRotation();
        StartCoroutine(WaitForPlayerMovement(waitForPlayerMovementTime));
        Debug.Log("Resume Movement!");
    }
    private IEnumerator WaitForPlayerMovement(float seconds)
    {
        PlayerMovementRemainingTime = seconds;
        while (PlayerMovementRemainingTime > 0)
        {
            PlayerMovementRemainingTime -= Time.deltaTime;
            Debug.Log("Waiting for player movement: " + (int)PlayerMovementRemainingTime);
            countdownTimer.GetComponent<TMPro.TextMeshProUGUI>().text = ((int)PlayerMovementRemainingTime).ToString();
            yield return null;
        }
        countdownTimer.SetActive(false);
        // yield return new WaitForSeconds(seconds);
        PlayerMovement.Instance.enabled = true;
        PlayerMovement.Instance.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
    private IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
    private IEnumerator WaitThenResumePatch(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _closestCrater.Patch();
        Begin();
    }

    private void Update()
    {
        if (PlayerMovement.Instance != null && !_hasStoppedAtCrater && _hasEnded)
        {
            StopAtCrater();
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
    
}
