using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public static PlayerMovement Instance = null;
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
    
    Rigidbody2D _body;
    public bool canPlaySound = false;
    private EventInstance _spaceShipLift;
    private float _previousYPosition; 
    
    void Start()
    {
        _body= GetComponent<Rigidbody2D>();
        _spaceShipLift = AudioManager.Instance.CreatInstance(FmodEvents.Instance.SpaceShipLift);
        _previousYPosition = transform.position.y;
    }

    
    void FixedUpdate()
    {
        AddForce();
       // UpdateSound();
        UpdateSoundBasedOnYPosition();
    }

    private void AddForce()
    {
        if (Input.GetMouseButton(0))
        {
            canPlaySound = true;
            _body.AddForce(new Vector3(0, 50, 0), ForceMode2D.Force);
            //Debug.Log("Mouse Clicked" + canPlaySound);

        }
        else if (Input.GetMouseButtonUp(0))
        {
            _body.velocity *= 0.25f;

        }
        else
        {
            canPlaySound = false;
        }
    }

    private void UpdateSoundBasedOnYPosition()
    {
        float currentYPosition = transform.position.y;

        if (currentYPosition > _previousYPosition)
        {
            // Player is rising, play the sound
            PLAYBACK_STATE playbackState;
            _spaceShipLift.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                _spaceShipLift.start();
            }
        }
        else if (currentYPosition < _previousYPosition)
        {
            // Player is falling, stop the sound
            _spaceShipLift.stop(STOP_MODE.ALLOWFADEOUT);
        }

        _previousYPosition = currentYPosition;
    }
    
    //private void UpdateSound()
    
    
   // {
     //   if (canPlaySound == true)
     //   {
            //get playback state
        //    PLAYBACK_STATE playbackState;
         //   spaceShipLift.getPlaybackState(out playbackState);
          //  if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
          // {
            //    spaceShipLift.start();

            //}

        //}

        //otherwise stop the spaceship sound
       // else
        //{
         //   spaceShipLift.stop(STOP_MODE.ALLOWFADEOUT);

        //}

   // }
    
   // void waittoStopSound()
    //{
      //  spaceShipLift.stop(STOP_MODE.ALLOWFADEOUT);
      //  Debug.log("Sound Stopped");
   // }




}
    

