using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PickupCheese : MonoBehaviour
{
   
    
    public static PickupCheese Instance = null;
    public int cheeseCounter = 0;
    [SerializeField] float fallAmount = 0f;
    [SerializeField] float fallSpeed = 0f;
    [SerializeField] float shakeAmount = 0.6f;
    [SerializeField] bool isStinkyCheese = false;

    CheeseMeter _cheeseMeter;

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
        
        cheeseCounter = 0;
        _cheeseMeter = FindObjectOfType<CheeseMeter>();
    }

    private void Update()
    {
    
        
    }

    private void StopMovement()
    {  
        if (isStinkyCheese)
        {
            if (PlayerVFX.Instance.IsPlayerDead() != true)
            {
                AudioManager.instance.playOneShot(FmodEvents.instance.badCheese, this.transform.position);
            }
            
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            this.gameObject.GetComponent<PlayerMovement>().enabled = false;
            fallSpeed = -2f;
            transform.position = new Vector3(transform.position.x, transform.position.y + fallSpeed * Time.deltaTime, transform.position.z);
            Physics.IgnoreLayerCollision(0, 1);
            if (Input.acceleration.x < -shakeAmount || Input.acceleration.x > shakeAmount || Input.acceleration.y < -shakeAmount || Input.acceleration.y > shakeAmount)
            {
                isStinkyCheese = false;
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                this.gameObject.GetComponent<PlayerMovement>().enabled = true;
            }
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
        if (collision.gameObject.tag == "Cheese" && !isStinkyCheese)
        {
            Pickup();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "StinkyCheese")
        {
            isStinkyCheese = true;
            StopMovement();
            Destroy(collision.gameObject);
        }
    }
}
