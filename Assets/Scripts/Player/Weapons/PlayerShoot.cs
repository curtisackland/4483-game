using UnityEngine;
using System;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;
    
    private InventoryController inventoryController;
    
    [SerializeField]
    private GameObject store;

    private void Start()
    {
        inventoryController = GetComponent<InventoryController>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !inventoryController.IsInventoryOpen() && !store.activeSelf)
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
