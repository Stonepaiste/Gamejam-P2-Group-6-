using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Meteor : MonoBehaviour
{
    Liv _liv;
    PlayerVFX _playerVFX;
    private void Start()
    {
        _liv = FindObjectOfType<Liv>();
        _playerVFX = FindObjectOfType<PlayerVFX>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _liv.DecreaseFillLevel(1f);
            _playerVFX.AlarmLights();
            Debug.Log("oh no!");
            AudioManager.instance.playOneShot(FmodEvents.instance.astroidHit, this.transform.position);
        }
    }
}
