using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    private EventInstance musicEventInstance;
    private List<EventInstance> eventInstances;
    

    private void Awake()
    {
        if (instance !=null)
        {
            Debug.LogError("Found more than one AudioManager in the scene");
        }
        

        instance = this;
        
        eventInstances = new List<EventInstance>();
    }

    private void Start()
    {
        InitilazeMusic(FmodEvents.instance.music);
        
    }
    private void InitilazeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreatInstance(musicEventReference);
        musicEventInstance.start();
    }
    
    
    public void playOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
        
    }
    
    public EventInstance CreatInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
        
    }

    private void Cleanup()
    {
        foreach (EventInstance eventInstance in eventInstances)
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
