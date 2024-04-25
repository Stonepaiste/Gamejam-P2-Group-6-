using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FmodEvents : MonoBehaviour
{
    
    [field: Header("Cheesep pickup SFX")]
    [field: SerializeField] public EventReference CheesePickupSfx { get; private set; }
    
    [field: Header("Music")]
    [field: SerializeField] public EventReference Music { get; private set; }
    
    [field: Header ("Space Ship Lift")]
    [field: SerializeField] public EventReference SpaceShipLift { get; private set; }
    
    [field: Header ("Badcheese")]
    [field: SerializeField] public EventReference BadCheese { get; private set; }
   public static FmodEvents Instance { get; private set; }
   
   private void Awake()
   {
       if (Instance != null)
       {
           Debug.LogError("Found more than one FMOD Events scripts in the scene");
       }
       
       Instance = this;
   }
}
