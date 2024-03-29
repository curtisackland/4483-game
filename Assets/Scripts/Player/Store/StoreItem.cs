using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    private StoreController store;

    private Sprite gunImage;
    
    public GunData gunData;

    void Start()
    {
        store = GetComponentInParent<StoreController>();
        gunImage = GetComponent<Image>().sprite;
    }

    public void OnPointerClick(BaseEventData pointerEvent)
    {
        store.ChangePreviewWeapon(gunData, gunImage);
    }

    public Sprite GetWeaponSprite()
    {
        return gunImage;
    }
    
}
