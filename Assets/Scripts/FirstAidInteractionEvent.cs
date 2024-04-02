using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public void Awake()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        //playerHealth =  GetComponentInChildren<PlayerHealth>();
        OnInteract.AddListener(() =>
        {
            playerHealth.RestoreHealth(100);
        });
    }

    public UnityEvent OnInteract;
}
