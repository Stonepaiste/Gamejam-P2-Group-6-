using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    
    //[Header("Volume")]
    //[Range(0, 1)]
    //public float masterVolume = 1;
    //[Range(0,1)]
    //public float musicVolume = 1;
    //[Range(0,1)]
    //public float SFXVolume = 1;
    //[Range(0,1)]
    //public float voiceVolume = 1;

    //private Bus masterBus;
    //private Bus musicBus;
    //private Bus SFXBus;
   // private Bus voiceBus;
    
    
    public static AudioManager instance { get; private set; }
    private EventInstance _musicEventInstance;
    private List<EventInstance> _eventInstances;
    // Store the EventInstance for the spaceship lift sound
    //private EventInstance spaceshipLiftInstance;
    

    private void Awake()
    {
        if (instance !=null)
        {
            Debug.LogError("Found more than one AudioManager in the scene");
        }
        

        instance = this;
        
        _eventInstances = new List<EventInstance>();
        
        //masterBus = RuntimeManager.GetBus("bus:/");
        //musicBus = RuntimeManager.GetBus("bus:/Music");
        //SFXBus = RuntimeManager.GetBus("bus:/SFX");
       // voiceBus = RuntimeManager.GetBus("bus:/Voice");   
    }

    private void Start()
    {
        InitilazeMusic(FmodEvents.instance.music);
        
    }

    private void Update()
    {
        
        //masterBus.setVolume(masterVolume);
        //musicBus.setVolume(musicVolume);
        //SFXBus.setVolume(SFXVolume);
        //voiceBus.setVolume(voiceVolume);
    }
    
    
    // Creating Music EventInstance
    private void InitilazeMusic(EventReference musicEventReference)
    {
        _musicEventInstance = CreatInstance(musicEventReference);
        _musicEventInstance.start();
    }
    
    
    public void playOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
        // Create an EventInstance and start it
        //spaceshipLiftInstance = RuntimeManager.CreateInstance(sound);
        //spaceshipLiftInstance.set3DAttributes(RuntimeUtils.To3DAttributes(worldPos));
        //spaceshipLiftInstance.start();
        
    }
    
   // public void stopSpaceshipLiftSound()
    //{
        //  check if the Spaceshipsound is playimg before trying to stop it. 
      //  if (spaceshipLiftInstance.isValid())
        //{
          //  spaceshipLiftInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //}

        //{
          //  spaceshipLiftInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //spaceshipLiftInstance.release();
            
        //}
       
    //}
    
    public EventInstance CreatInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(eventInstance);
        return eventInstance;
        
    }
    
    

    private void Cleanup()
    {
        foreach (EventInstance eventInstance in _eventInstances)
        {
         
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        
    }

    private void OnDestroy()
    {
        Cleanup();
    }
}
