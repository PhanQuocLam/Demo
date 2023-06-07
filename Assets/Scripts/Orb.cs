using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public GameObject explosionVFXprefab;
    int player;
    
    void Start()
    {
        player = LayerMask.NameToLayer("Player");
        GameManager.RegisterOrb(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(explosionVFXprefab, transform.position, transform.rotation);
        gameObject.SetActive(false);
        AudioManager.PlayOrbAudio();
        GameManager.PlayerGrabbedOrb(this);
    }

}
