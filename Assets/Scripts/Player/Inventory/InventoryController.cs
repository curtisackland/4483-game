
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{

    public List<Image> weaponSlots;

    private List<GunData> weaponSlotsGunData = new List<GunData>();

    private List<GunData> inventoryGunData = new List<GunData>();

    public List<Image> inventoryGunImages;

    public List<Image> inventoryWeaponSlotImages;

    public Image previewWeaponImage;

    public TextMeshProUGUI previewWeaponDescription;

    public TextMeshProUGUI previewWeaponPower;
    
    public TextMeshProUGUI previewWeaponFirerate;
    
    public TextMeshProUGUI previewWeaponMagSize;

    private GunData previewWeaponGunData;

    public GameObject inventoryUI;

    public WeaponSwitching weaponSwitching;

    [SerializeField]
    private Dictionary<string, int> ammoCounts = new Dictionary<string, int>();

    private bool inventoryOpen = false;
    
    void Start()
    {
        weaponSlots[0].sprite = Resources.Load<Sprite>("WeaponOutlines/AK Blue");
        weaponSlots[1].sprite = Resources.Load<Sprite>("WeaponOutlines/Silenced Pistol Blue");
        weaponSlots[2].sprite = Resources.Load<Sprite>("WeaponOutlines/Blank Gun");
        weaponSlots[3].sprite = Resources.Load<Sprite>("WeaponOutlines/Blank Gun");
        
        inventoryGunImages[0].sprite = Resources.Load<Sprite>("WeaponOutlines/AK Blue");
        inventoryGunImages[1].sprite = Resources.Load<Sprite>("WeaponOutlines/Silenced Pistol Blue");
        inventoryGunImages[2].sprite = Resources.Load<Sprite>("WeaponOutlines/Blank Gun");
        inventoryGunImages[3].sprite = Resources.Load<Sprite>("WeaponOutlines/Blank Gun");
        inventoryGunImages[4].sprite = Resources.Load<Sprite>("WeaponOutlines/Blank Gun");
        inventoryGunImages[5].sprite = Resources.Load<Sprite>("WeaponOutlines/Blank Gun");
        inventoryGunImages[6].sprite = Resources.Load<Sprite>("WeaponOutlines/Blank Gun");
        inventoryGunImages[7].sprite = Resources.Load<Sprite>("WeaponOutlines/Blank Gun");
        
        inventoryWeaponSlotImages[0].sprite = Resources.Load<Sprite>("WeaponOutlines/AK Blue");
        inventoryWeaponSlotImages[1].sprite = Resources.Load<Sprite>("WeaponOutlines/Silenced Pistol Blue");
        inventoryWeaponSlotImages[2].sprite = Resources.Load<Sprite>("WeaponOutlines/Blank Gun");
        inventoryWeaponSlotImages[3].sprite = Resources.Load<Sprite>("WeaponOutlines/Blank Gun");

        previewWeaponImage.sprite = Resources.Load<Sprite>("WeaponOutlines/AK Blue");
        previewWeaponGunData = AssetDatabase.LoadAssetAtPath<GunData>("Assets/Scripts/ScriptableObjects/Gun Objects/Assault Rifle/AK47.asset");
        
        // Start with these two guns
        GunData ak47 = AssetDatabase.LoadAssetAtPath<GunData>("Assets/Scripts/ScriptableObjects/Gun Objects/Assault Rifle/AK47.asset");
        GunData silencedPistol = AssetDatabase.LoadAssetAtPath<GunData>("Assets/Scripts/ScriptableObjects/Gun Objects/Pistol/Silenced Pistol.asset");
        inventoryGunData.Add(ak47);
        inventoryGunData.Add(silencedPistol);
        weaponSlotsGunData.Add(ak47);
        weaponSlotsGunData.Add(silencedPistol);
        weaponSlotsGunData.Add(null);
        weaponSlotsGunData.Add(null);

        ammoCounts["AR"] = 300;
        ammoCounts["Pistol"] = 300;
        ammoCounts["Shotgun"] = 300;
        ammoCounts["Sniper"] = 300;
        
        inventoryUI.GameObject().SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryOpen = !inventoryOpen;
            if (!inventoryOpen)
            {
                inventoryUI.SetActive(false);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            }
            else
            {
                inventoryUI.SetActive(true);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
            }
        }

        if (inventoryOpen)
        {
            
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            AddWeapon(AssetDatabase.LoadAssetAtPath<GunData>("Assets/Scripts/ScriptableObjects/Gun Objects/Assault Rifle/M4A1.asset"));
        }
    }

    public bool IsInventoryOpen()
    {
        return inventoryOpen;
    }

    public void SwitchGunSlotWeapon(Image gunSwitchImage)
    {
        
        // We don't want to swap blanks with anything
        if (gunSwitchImage.sprite.name == "Blank Gun")
        {
            return;
        }
        
        bool fromInventory = inventoryGunImages.Contains(gunSwitchImage);
        bool toInventory = false;

        // Setting up preview weapon area on the click of a weapon
        previewWeaponImage.sprite = gunSwitchImage.sprite;
        if (fromInventory)
        {
            previewWeaponGunData = inventoryGunData[inventoryGunImages.IndexOf(gunSwitchImage)];
        }
        else
        {
            previewWeaponGunData = weaponSlotsGunData[inventoryWeaponSlotImages.IndexOf(gunSwitchImage)];
        }
        previewWeaponDescription.text = previewWeaponGunData.description;
        previewWeaponPower.text = previewWeaponGunData.damage.ToString();
        previewWeaponFirerate.text = previewWeaponGunData.fireRate.ToString();
        previewWeaponMagSize.text = previewWeaponGunData.magSize.ToString();

        for (int i = 0; i < inventoryWeaponSlotImages.Count; i++)
        {
            Drag slot = inventoryGunImages[i].GetComponent<Drag>();
            if (slot != null && slot.IsMouseOver() && inventoryGunImages[i] != gunSwitchImage)
            {
                toInventory = true;
            } else if (slot == null)
            {
                Debug.Log("Drag component not assigned to weapon UI element");
            }
        }

        // swap from inventory to gun slots
        if (fromInventory && !toInventory)
        {
            int indexInInventory = inventoryGunImages.IndexOf(gunSwitchImage);

            // if the weapon is already in the slots, don't do anything
            if (weaponSlotsGunData.Contains(inventoryGunData[indexInInventory]))
            {
                return;
            }
            
            for (int i = 0; i < inventoryWeaponSlotImages.Count; i++)
            {
                GameObject parent = inventoryWeaponSlotImages[i].gameObject;
                Drag slot = parent.GetComponent<Drag>();
                if (slot != null && slot.IsMouseOver())
                {
                    weaponSlotsGunData[i] = inventoryGunData[indexInInventory];
                    weaponSlots[i].sprite = inventoryGunImages[indexInInventory].sprite;
                    inventoryWeaponSlotImages[i].sprite = inventoryGunImages[indexInInventory].sprite;
                    weaponSwitching.SwapWeapons(weaponSlotsGunData);
                    break;
                } else if (slot == null)
                {
                    Debug.Log("Drag component not assigned to weapon UI element");
                }
            }
            
        } 
        // swap from inventory to inventory
        else if (fromInventory && toInventory)
        {
            int indexInInventory = inventoryGunImages.IndexOf(gunSwitchImage);
            
            for (int i = 0; i < inventoryGunImages.Count; i++)
            {
                Drag slot = inventoryGunImages[i].gameObject.GetComponent<Drag>();
                if (slot != null && slot.IsMouseOver() && inventoryGunImages[i] != gunSwitchImage)
                {
                    (inventoryGunImages[i].sprite, inventoryGunImages[indexInInventory].sprite) = (inventoryGunImages[indexInInventory].sprite, inventoryGunImages[i].sprite);
                    (inventoryGunData[i], inventoryGunData[indexInInventory]) = (inventoryGunData[indexInInventory], inventoryGunData[i]);
                    break;
                }
            }
        } 
        // swap from gun slots to gun slots
        else if (!fromInventory && !toInventory)
        {
            int indexInSlots = inventoryWeaponSlotImages.IndexOf(gunSwitchImage);
            for (int i = 0; i < inventoryWeaponSlotImages.Count; i++)
            {
                Drag slot = inventoryWeaponSlotImages[i].gameObject.GetComponent<Drag>();
                if (slot != null && slot.IsMouseOver() && inventoryWeaponSlotImages[i] != gunSwitchImage)
                {
                    (inventoryWeaponSlotImages[i].sprite, inventoryWeaponSlotImages[indexInSlots].sprite) = (inventoryWeaponSlotImages[indexInSlots].sprite, inventoryWeaponSlotImages[i].sprite);
                    (weaponSlotsGunData[i], weaponSlotsGunData[indexInSlots]) = (weaponSlotsGunData[indexInSlots], weaponSlotsGunData[i]);
                    (weaponSlots[i].sprite, weaponSlots[indexInSlots].sprite) = (weaponSlots[indexInSlots].sprite, weaponSlots[i].sprite);
                    weaponSwitching.SwapWeapons(weaponSlotsGunData);
                    break;
                }
            }
        } 
        // swap from gun slots to inventory
        else if (!fromInventory && toInventory)
        {
            // not implementing from slots to inventory. Might just add an "x" to the slots so users can remove them that way 
        }
    }

    public void AddWeapon(GunData newGun)
    {
        if (!inventoryGunData.Contains(newGun))
        {
            inventoryGunData.Add(newGun);
            inventoryGunImages[inventoryGunData.IndexOf(newGun)].sprite = Resources.Load<Sprite>("WeaponOutlines/" + newGun.outlineAssetName);
        }
    }

    public void UpdateAmmo(string ammoType, int amount)
    {
        switch (ammoType)
        {
            case "AR":
                ammoCounts["AR"] += amount;
                ammoCounts["AR"] = Math.Clamp(ammoCounts["AR"], 0, 300);
                break;
            case "Pistol":
                ammoCounts["Pistol"] += amount;
                ammoCounts["Pistol"] = Math.Clamp(ammoCounts["Pistol"], 0, 60);
                break;
            case "Shotgun":
                ammoCounts["Shotgun"] += amount;
                ammoCounts["Shotgun"] = Math.Clamp(ammoCounts["Shotgun"], 0, 180);
                break;
            case "Sniper":
                ammoCounts["Sniper"] += amount;
                ammoCounts["Sniper"] = Math.Clamp(ammoCounts["Sniper"], 0, 60);
                break;
            case "All":
                ammoCounts["AR"] += amount;
                ammoCounts["AR"] = Math.Clamp(ammoCounts["AR"], 0, 300);
                ammoCounts["Pistol"] += amount;
                ammoCounts["Pistol"] = Math.Clamp(ammoCounts["Pistol"], 0, 60);
                ammoCounts["Shotgun"] += amount;
                ammoCounts["Shotgun"] = Math.Clamp(ammoCounts["Shotgun"], 0, 180);
                ammoCounts["Sniper"] += amount;
                ammoCounts["Sniper"] = Math.Clamp(ammoCounts["Sniper"], 0, 60);
                break;
        }
    }

    public int GetAmmoCount(string type)
    {
        return ammoCounts[type];
    }
}
