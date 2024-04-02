using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseDoor : Interactable
{
    [SerializeField]
    private GameObject door;
    public AudioSource doorOpenSound;
    public AudioSource doorCloseSound;

    private bool doorOpen;
    
    protected override void Interact()
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);

        if (doorOpen)
        {
            promptMessage = "Close Door";
            doorOpenSound.Play();
        }
        else
        {
            doorCloseSound.Play();
            promptMessage = "Open Door";
        }
    }
}
