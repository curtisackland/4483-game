using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInteractable : Interactable
{
    public GameObject store;

    public bool isStoreActive = false;

    [SerializeField]
    private GameObject inventory;

    private float timer;
    
    void Start()
    {
        store.SetActive(false);
        timer = 0;
    }

    protected override void Interact()
    {
        if (!isStoreActive && timer > 0.2 && !inventory.activeSelf)
        {
            isStoreActive = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            promptMessage = "";
            store.SetActive(true);
        }
    }
    
    void Update()
    {
        if (isStoreActive && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            isStoreActive = false;
            timer = 0;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            store.SetActive(false);
        }

        if (!isStoreActive)
        {
            timer += Time.deltaTime;
        }
    }
}
