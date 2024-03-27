using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;

    private float xRotation = 0f;

    public float xSensitivity = 30f;

    public float ySensitivty = 30f;

    private InventoryController inventoryController;

    private void Start()
    {
        inventoryController = GetComponent<InventoryController>();
    }

    public void ProcessLook(Vector2 input)
    {
        
        if (!inventoryController.IsInventoryOpen())
        {
            float mouseX = input.x;
            float mouseY = input.y;
        
            // camera rotation for looking up and down
            xRotation -= (mouseY * Time.deltaTime) * ySensitivty;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        
            // apply rotation to camera
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        
            // camera rotation for left and right
            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }
    }
    
}
