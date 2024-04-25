using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FmodEvents : MonoBehaviour
{
    
    [field: Header("Cheesep pickup SFX")]
    [field: SerializeField] public EventReference cheesePickupSFX { get; private set; }
    
    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }
    
    [field: Header ("Space Ship Lift")]
    [field: SerializeField] public EventReference spaceShipLift { get; private set; }
    
    [field: Header ("Badcheese")]
    [field: SerializeField] public EventReference badCheese { get; private set; }
    
    [field: Header ("AstroidHit")]
    [field:SerializeField] public EventReference astroidHit { get; private set; }
   public static FmodEvents instance { get; private set; }
   
   private void Awake()
   {
       if (instance != null)
       {
           Debug.LogError("Found more than one FMOD Events scripts in the scene");
       }
       
       instance = this;
   }
}
