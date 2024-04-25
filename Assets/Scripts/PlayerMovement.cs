using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    public bool canPlaySound = false;
    private EventInstance spaceShipLift;
    void Start()
    {
        body= GetComponent<Rigidbody2D>();
        spaceShipLift = AudioManager.instance.CreatInstance(FmodEvents.instance.spaceShipLift);
    }

    
    void FixedUpdate()
    {
        AddForce();
        UpdateSound();
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

    
    
    private void UpdateSound()
    
    
    {
        if (canPlaySound == true)
        {
            //get playback state
            PLAYBACK_STATE playbackState;
            spaceShipLift.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
           {
                spaceShipLift.start();

            }

        }

        //otherwise stop the spaceship sound
        else
        {
            spaceShipLift.stop(STOP_MODE.ALLOWFADEOUT);

        }

    }
    
   // void waittoStopSound()
    //{
      //  spaceShipLift.stop(STOP_MODE.ALLOWFADEOUT);
      //  Debug.log("Sound Stopped");
   // }




}
    

