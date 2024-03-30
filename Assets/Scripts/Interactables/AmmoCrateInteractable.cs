using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrateInteractable : Interactable
{

    public InventoryController inventory;

    public int refillAmount;

    public string ammoType;

    protected override void Interact()
    {
        inventory.UpdateAmmo(ammoType, refillAmount);
    }
}
