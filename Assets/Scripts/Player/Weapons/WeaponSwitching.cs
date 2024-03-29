using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitching : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform[] weapons;

    [SerializeField] private Image[] backgroundImages;
    
    [Header("Keys")]
    [SerializeField] private KeyCode[] keys;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedWeapon;
    private float timeSinceLastSwitch;

    private void Start() {
        SetWeapons();
        Select(selectedWeapon);

        timeSinceLastSwitch = 0f;
    }

    private void SetWeapons() {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            weapons[i] = transform.GetChild(i);

        if (keys == null) keys = new KeyCode[weapons.Length];
    }

    private void Update() {
        int previousSelectedWeapon = selectedWeapon;

        for (int i = 0; i < keys.Length; i++)
            if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime)
            {
                selectedWeapon = i;
            }

        if (previousSelectedWeapon != selectedWeapon)
        {
            Select(selectedWeapon);
        }

        timeSinceLastSwitch += Time.deltaTime;
    }

    private void Select(int weaponIndex) {

        for (int i = 0; i < backgroundImages.Length; i++)
        {
            if (i == weaponIndex)
            {
                backgroundImages[i].color = new Color(255, 198, 0, 2);
            }
            else
            {
                backgroundImages[i].color = new Color(175, 175, 175, 0.7f);
            }
        }
        
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].gameObject.SetActive(i == weaponIndex);
            }
        }

        timeSinceLastSwitch = 0f;

        OnWeaponSelected();
    }

    private void OnWeaponSelected() {  }

    public void SwapWeapons(List<GunData> currentGuns)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                Destroy(weapons[i].gameObject);
            }
        }
        weapons = new Transform[currentGuns.Count];

        GameObject gunPrefab;
        for (int i = 0; i < currentGuns.Count; i++)
        {
            if (currentGuns[i] != null)
            {
                gunPrefab = Resources.Load<GameObject>("Guns/" + currentGuns[i].name);
                weapons[i] = Instantiate(gunPrefab, transform).transform;
                weapons[i].gameObject.SetActive(i == selectedWeapon);
            }
        }
    }
}
