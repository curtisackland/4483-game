using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrateInteractable : Interactable
{

    private InventoryController inventory;

    public int refillAmount;

    public string ammoType;

    public AudioSource refillSound;

    public void Awake()
    {
        inventory = FindFirstObjectByType<InventoryController>();
    }

    protected override void Interact()
    {
        inventory.UpdateAmmo(ammoType, refillAmount);
        refillSound.Play();
    }
}
