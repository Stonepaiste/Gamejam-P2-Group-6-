using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance = null;
    
    [SerializeField] float carterStopPositionY = 0f;
    [SerializeField] float carterStopDistance = 0.1f;
    [SerializeField] float waitForPlayerMovementTime = 3;
    
    private bool _hasEnded = false;
    private bool _hasStoppedAtCrater = false;
    
    private float PlayerMovementRemainingTime = 0;

    private MoonCrater _closestCrater;
    
    private int patchedCraters = 0;
    private int totalCraters = 0;
    
    [SerializeField] private GameObject countdownTimer;
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject looseGameScreen;
    
    Vector3 playerPosition;

    private PlayableDirector playable;
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
        totalCraters = CraterGenerator.Instance.numberOfCraters;
        playable = PlayerVFX.Instance.gameObject.GetComponent<PlayableDirector>();
        
        playerPosition = PlayerMovement.Instance.transform.position;
        Begin();
    }
    
    public void Intro()
    {
        Debug.Log("Intro!");
    }

    void Begin()
    {
        Debug.Log("Game Start!");
        _hasStoppedAtCrater = false;
        _hasEnded = false;
        var vector3 = playerPosition;
        vector3.y = 0;
        PlayerMovement.Instance.gameObject.transform.position = vector3;
        // Liv.Instance.Initiate();
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
        PickupCheese.Instance.canPickupCheese = false;
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
            // Debug.Log("Player Position: " + playerPosition + " Closest Crater: " + _closestCrater.transform.position + " Distance: " + craterDistance);
        }
        if (craterDistance < 0.1f)
        {
            Debug.Log("Game Stop at Crater!");
            _hasStoppedAtCrater = true;
            playable.Play();
            // When the playable is done, stop the playable and resume the game
            StartCoroutine(WaitThenResumeGame((float)playable.duration));
            // MoonRotation.instance.isRotating = false;
            MoonRotation.Instance.StopRotation();
            StopGame();
        }
    }
    
    private IEnumerator WaitThenResumeGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        playable.Stop();
        // TODO: Replace with timeline event.
        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.orthographicSize = 5;
        Begin();
    }
    
    public void PatchCrater()
    {
        Debug.Log("Crater Patched!");
        patchedCraters++;
        _closestCrater.Patch();
    }

    private void StopGame()
    {
        Debug.Log("Stop Game!");
        StopPlayerActions();
        StopPlayerMovement();
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
            countdownTimer.GetComponent<TMPro.TextMeshProUGUI>().text = ((int)PlayerMovementRemainingTime + 1).ToString();
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
    private void Update()
    {
        if (PlayerMovement.Instance is not null && !_hasStoppedAtCrater)
        {
            if (_hasEnded)
            {
                StopAtCrater();
            }
        }

        if (patchedCraters == totalCraters)
        {
            EndGameScreen(true);
        }
    }
    
    private void EndGameScreen(bool hasWon)
    {
        gameWinScreen.SetActive(true);
        restartButton.SetActive(true);
    }
    
    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
    
}
