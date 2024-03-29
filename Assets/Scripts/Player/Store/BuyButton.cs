using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyButton : MonoBehaviour
{
    [SerializeField]
    private StoreController store;

    public void OnPointerClick(BaseEventData pointerClickEvent)
    {
        store.BuyWeapon();
    }
}
