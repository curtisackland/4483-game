using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponData : MonoBehaviour
{
    public Camera mainCamera;

    public Slider reloadSlider;

    public TextMeshProUGUI currentMagAmmo;

    public TextMeshProUGUI maxMagAmmo;

    public TextMeshProUGUI totalAmmo;

    public Image ammoTypeImage;

    public InventoryController inventoryController;
}
