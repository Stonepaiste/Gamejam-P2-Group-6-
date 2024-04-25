using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    public bool IsPlayerDead()
    {
        return _isDead;

    }
    public static PlayerVFX Instance = null;
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
    
    [SerializeField] Sprite[] playerLightsSprite;
    [SerializeField] Sprite[] playerAlarmLightsSprites;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite deadMouse;
    
    bool _mustDie = false;
    bool _isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        Initiate();
    }
    
    public void Initiate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(LoopLights());
    }

    IEnumerator LoopLights()
    {
        int index = 0;
        while (true)
        {
            spriteRenderer.sprite = playerLightsSprite[index];
            index = (index + 1) % playerLightsSprite.Length;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public void AlarmLights()
    {
        if (_isDead) return;
        StopAllCoroutines();
        StartCoroutine(AlarmLightsCoroutine());
    }
    
    IEnumerator AlarmLightsCoroutine()
    {

        int index = 0;
        for (int i = 0; i < 5 * playerAlarmLightsSprites.Length; i++)
        {
            spriteRenderer.sprite = playerAlarmLightsSprites[index];
            index = (index + 1) % playerAlarmLightsSprites.Length;
            yield return new WaitForSeconds(0.2f);
        }
        Debug.Log("AlarmLightsCoroutine completed" + _mustDie);
        StartCoroutine(LoopLights());
    }
    
    public void Die()
    {
        _isDead = true;
        AudioManager.instance.playOneShot(FmodEvents.instance.crash, this.transform.position);
        StopAllCoroutines();

        spriteRenderer.sprite = deadMouse;
        
    }
}
