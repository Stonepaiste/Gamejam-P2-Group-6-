using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    public bool canPlaySound = false;
    private EventInstance spaceShipLift;
    private float previousYPosition; 
    
    void Start()
    {
        body= GetComponent<Rigidbody2D>();
        spaceShipLift = AudioManager.instance.CreatInstance(FmodEvents.instance.spaceShipLift);
        previousYPosition = transform.position.y;
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
            body.AddForce(new Vector3(0, 50, 0), ForceMode2D.Force);
            //Debug.Log("Mouse Clicked" + canPlaySound);

        }
        else if (Input.GetMouseButtonUp(0))
        {
            body.velocity *= 0.25f;

        }
        else
        {
            canPlaySound = false;
        }
    }

    private void UpdateSoundBasedOnYPosition()
    {
        float currentYPosition = transform.position.y;

        if (currentYPosition > previousYPosition)
        {
            // Player is rising, play the sound
            PLAYBACK_STATE playbackState;
            spaceShipLift.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                spaceShipLift.start();
            }
        }
        else if (currentYPosition < previousYPosition)
        {
            // Player is falling, stop the sound
            spaceShipLift.stop(STOP_MODE.ALLOWFADEOUT);
        }

        previousYPosition = currentYPosition;
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
    

