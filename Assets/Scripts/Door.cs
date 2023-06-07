using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        GameManager.RegisterDoor(this);

    }
    public void Open()
    {
        anim.SetTrigger("Open");
        AudioManager.PlayDoorOpenAudio();
    }

    
}
