using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
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

    public Image previewWeaponAmmoType;

    private GunData previewWeaponGunData;

    public GameObject inventoryUI;

    public WeaponSwitching weaponSwitching;
    
    private Dictionary<string, int> ammoCounts = new Dictionary<string, int>();

    [SerializeField]
    private TextMeshProUGUI arAmmoText;
    
    [SerializeField]
    private TextMeshProUGUI pistolAmmoText;
    
    [SerializeField]
    private TextMeshProUGUI shotgunAmmoText;
    
    [SerializeField]
    private TextMeshProUGUI sniperAmmoText;

    [SerializeField]
    private GameObject store;

    private bool inventoryOpen = false;

    private AudioSource openInventoryAudio;
    
    private AudioSource closeInventoryAudio;
    
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
        previewWeaponGunData = Resources.Load<GunData>("Gun Objects/Assault Rifle/AK47");
        
        // Start with these two guns
        GunData ak47 = Resources.Load<GunData>("Gun Objects/Assault Rifle/AK47");
        GunData silencedPistol = Resources.Load<GunData>("Gun Objects/Pistol/Silenced Pistol");
        inventoryGunData.Add(ak47);
        inventoryGunData.Add(silencedPistol);
        inventoryGunData.Add(null);
        inventoryGunData.Add(null);
        inventoryGunData.Add(null);
        inventoryGunData.Add(null);
        inventoryGunData.Add(null);
        inventoryGunData.Add(null);
        
        weaponSlotsGunData.Add(ak47);
        weaponSlotsGunData.Add(silencedPistol);
        weaponSlotsGunData.Add(null);
        weaponSlotsGunData.Add(null);

        ammoCounts["AR"] = 300;
        ammoCounts["Pistol"] = 300;
        ammoCounts["Shotgun"] = 300;
        ammoCounts["Sniper"] = 300;

        openInventoryAudio = GetComponents<AudioSource>()[1];
        closeInventoryAudio = GetComponents<AudioSource>()[2];
        
        inventoryUI.GameObject().SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !store.activeSelf)
        {
            inventoryOpen = !inventoryOpen;
            if (!inventoryOpen)
            {
                closeInventoryAudio.Play();
                inventoryUI.SetActive(false);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            }
            else
            {
                openInventoryAudio.Play();
                inventoryUI.SetActive(true);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
            }
        }

        if (inventoryOpen)
        {
            arAmmoText.text = ammoCounts["AR"].ToString();
            pistolAmmoText.text = ammoCounts["Pistol"].ToString();
            shotgunAmmoText.text = ammoCounts["Shotgun"].ToString();
            sniperAmmoText.text = ammoCounts["Sniper"].ToString();
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
        previewWeaponAmmoType.sprite = Resources.Load<Sprite>("Icons/" + previewWeaponGunData.ammoType + " Ammo");

        for (int i = 0; i < inventoryGunImages.Count; i++)
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
            for (int i = 0; i < inventoryGunData.Count; i++)
            {
                if (inventoryGunData[i] == null)
                {
                    inventoryGunData[i] = newGun;
                    inventoryGunImages[inventoryGunData.IndexOf(newGun)].sprite = Resources.Load<Sprite>("WeaponOutlines/" + newGun.outlineAssetName);
                    break;
                }
            }
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
