using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

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
    
    [SerializeField] private TimelineAsset sceneEnterTimeline;
    [SerializeField] private TimelineAsset sceneExitTimeline;
    
    [SerializeField] private GameObject countdownTimer;
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject looseGameScreen;
    
    Vector3 playerPosition;
    
    private bool gameIsOver = false;

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
        // DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        totalCraters = CraterGenerator.Instance.numberOfCraters;
        playable = PlayerVFX.Instance.gameObject.GetComponent<PlayableDirector>();
        playerPosition = PlayerMovement.Instance.transform.position;
        
        /*// Get object named "GameUI" in the scene
        gameUI = GameObject.Find("GameUI");
        // Get object named "CountdownTimer" in the scene
        countdownTimer = GameObject.Find("CountdownTimer");
        // Get object named "GameWinScreen" in the scene
        gameWinScreen = gameWinScreen ? gameWinScreen : GameObject.Find("EndGame");
        // Get object named "RestartButton" in the scene
        restartButton = GameObject.Find("RestartButton");
        // Get object named "LooseGameScreen" in the scene
        looseGameScreen = GameObject.Find("EndGame");*/
        Begin();
    }
    public void GameOver()
    {
        Debug.Log("Game Over!");
        PlayerVFX.Instance.Die();
        StopPlayerActions();
        End();
        EndGameScreen(false);
    }
    public void GameWin()
    {
        Debug.Log("Game Win!");
        _hasEnded = true;
        End();
    }
    
    
    public void Intro()
    {
        Debug.Log("Intro!");
        playable.Play(sceneEnterTimeline);
    }

    void Begin()
    {
        Debug.Log("Game Start!");
        // Intro();
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
    }
    
    void StopAtCrater()
    {
        // .Log("Game Stop at Crater!");
        // Get current player position
        Vector3 playerPosition = PlayerMovement.Instance.transform.position;
        float craterDistance;
        (_closestCrater, craterDistance) = CraterGenerator.Instance.GetClosestCraterRight(new Vector3(0,0,0));
        /*if (_closestCrater != null)
        {
            // Debug.Log("Player Position: " + playerPosition + " Closest Crater: " + _closestCrater.transform.position + " Distance: " + craterDistance);
        }*/
        if (craterDistance < carterStopDistance)
        {
            Debug.Log("Game Stop at Crater!");
            _hasStoppedAtCrater = true;
            MoonRotation.Instance.StopRotation();
            playable.Play(sceneExitTimeline);
            // When the playable is done, stop the playable and resume the game
            StartCoroutine(WaitThenPatch(12));
            StopGame();
            if (!gameIsOver)
            {
                // Intro();
                StartCoroutine(WaitThenResumeGame((float)playable.duration));
            }
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
    
    private IEnumerator WaitThenPatch(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PatchCrater();
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
        if (PlayerMovement.Instance is not null)
        {
            Debug.Log("playermovement is not null");
            if (_hasEnded && !_hasStoppedAtCrater)
            {
                Debug.Log("hasEnded and not stopped at crater");
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
        gameIsOver = true;
        gameWinScreen.SetActive(true);

        if (playable.state != PlayState.Playing)
        {
            Debug.Log("Not Playing!");

            // StartCoroutine(LoadSceneAfterDelay(0, 3));
            SceneManager.LoadScene(0);
        }
        // TODO: Reset singletons upon scene load.
        // TODO: Show end game screen earlier.
    }
    
    private IEnumerator LoadSceneAfterDelay(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
}
