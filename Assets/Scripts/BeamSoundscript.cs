using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BeamSoundscript : MonoBehaviour
{


    public void PlayBeamSound()
    {
        AudioManager.instance.playOneShot(FmodEvents.instance.beam, this.transform.position);
    }
}
