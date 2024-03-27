using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;
    
    private InventoryController inventoryController;

    private void Start()
    {
        inventoryController = GetComponent<InventoryController>();
    }

    private void Update()
    {
        // TODO use input actions for this
        
        if (Input.GetMouseButton(0) && !inventoryController.IsInventoryOpen())
        {
            // if there are no subscribers to this event, check for null first
            shootInput?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            reloadInput?.Invoke();
        }
    }
}
