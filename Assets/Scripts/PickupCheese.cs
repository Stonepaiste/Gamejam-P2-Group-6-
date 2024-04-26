using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PickupCheese : MonoBehaviour
{
   
    
    public static PickupCheese Instance = null;
    public int cheeseCounter = 0;
    [SerializeField] float fallSpeed = 0f;
    [SerializeField] float shakeAmount = 0.3f;
    [SerializeField] bool isStinkyCheese = false;
    
    public bool canPickupCheese = true; 

    CheeseMeter _cheeseMeter;

    private float acclAmount;

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
        
        cheeseCounter = 0;
        _cheeseMeter = FindObjectOfType<CheeseMeter>();
    }

    private void Update()
    {
        acclAmount = Mathf.Abs(Input.acceleration.y);
        // Debug.Log("accelerometer: " + acclAmount);
        if (isStinkyCheese)
        {
            if (acclAmount > shakeAmount)
            {
                Debug.Log("Stinky cheese accelerometer");
                isStinkyCheese = false;
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                this.gameObject.GetComponent<PlayerMovement>().enabled = true;
            }
        }
    }


    private void StopMovement()
    {  
        if (isStinkyCheese)
        {
            if (PlayerVFX.Instance.IsPlayerDead() != true)
            {
                AudioManager.instance.playOneShot(FmodEvents.instance.badCheese, this.transform.position);
            }
            Debug.Log("Stinky cheese running");
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            this.gameObject.GetComponent<PlayerMovement>().enabled = false;
            fallSpeed = -2f;
            transform.position = new Vector3(transform.position.x, transform.position.y + fallSpeed * Time.deltaTime, transform.position.z);
            Physics.IgnoreLayerCollision(0, 1);
            Debug.Log("Stinky Accl: " + Input.acceleration.y);
        }
    }

    void Pickup()
    {
        cheeseCounter++;
        AudioManager.instance.playOneShot(FmodEvents.instance.cheesePickupSFX, this.transform.position);
        Debug.Log("Cheese Counter: " + cheeseCounter);
        _cheeseMeter.GetCheese();
        if (cheeseCounter >= 9)
        {
            GameFlow.Instance.GameWin();
        }
    }
    
    public void ResetCheese()
    {
        cheeseCounter = 0;
        _cheeseMeter.ResetCheese();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canPickupCheese)
        {
            if (collision.gameObject.tag == "Cheese" && !isStinkyCheese)
            {
                Pickup();
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.tag == "StinkyCheese")
            {
                isStinkyCheese = true;
                StopMovement();
                //PlayerMovement.Instance.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Destroy(collision.gameObject);
            }
        }
    }
}
