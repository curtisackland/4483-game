using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : Interactable
{

    [SerializeField]
    private InventoryController inventory;

    [SerializeField]
    private GunData gunData;
    
    protected override void Interact()
    {
        inventory.AddWeapon(gunData);
        Destroy(gameObject);
    }
}
