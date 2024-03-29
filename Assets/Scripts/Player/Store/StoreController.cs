using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour
{
    public PlayerXP playerStats;
    
    public Image previewGunImage;

    public TextMeshProUGUI previewGunDescription;

    public TextMeshProUGUI previewMonsterPointsCost;

    public TextMeshProUGUI previewUnlockLevel;

    public TextMeshProUGUI previewPowerStat;

    public TextMeshProUGUI previewFirerateStat;

    public TextMeshProUGUI previewMagSizeStat;

    public Image previewAmmoTypeImage;

    public TextMeshProUGUI previewBuyButtonText;
    
    public Image previewBuyButtonBackground;

    [SerializeField]
    private InventoryController inventory;
    
    private GunData previewGunData;
    
    void Start()
    {

        StoreItem[] storeItems = GetComponentsInChildren<StoreItem>();
        ChangePreviewWeapon(storeItems[0].gunData, storeItems[0].GetWeaponSprite());
        
        // make sure none of the guns are owned on restart
        for (int i = 0; i < storeItems.Length; i++)
        {
            storeItems[i].gunData.owned = false;
        }
    }

    private void Update()
    {
        if (previewGunData.owned)
        {
            previewBuyButtonBackground.color = Color.grey;
            previewBuyButtonText.text = "OWNED";
        } 
        else if (playerStats.GetXP() >= previewGunData.requiredXPLevel && playerStats.GetMonsterPoints() >= previewGunData.monsterPointsCost)
        {
            previewBuyButtonBackground.color = Color.green;
            previewBuyButtonText.text = "BUY";
        }
        else
        {
            previewBuyButtonBackground.color = Color.red;
            previewBuyButtonText.text = "BUY";
        }
    }

    public void ChangePreviewWeapon(GunData gun, Sprite gunSprite)
    {
        previewGunData = gun;
        previewGunImage.sprite = gunSprite;
        previewGunDescription.text = gun.description;
        previewMonsterPointsCost.text = gun.monsterPointsCost.ToString();
        previewUnlockLevel.text = gun.requiredXPLevel.ToString();
        previewPowerStat.text = gun.damage.ToString();
        previewFirerateStat.text = gun.fireRate.ToString();
        previewMagSizeStat.text = gun.magSize.ToString();
        previewAmmoTypeImage.sprite = Resources.Load<Sprite>("Icons/" + gun.ammoType + " Ammo");
    }

    public void BuyWeapon()
    {
        if (!previewGunData.owned && playerStats.GetXP() >= previewGunData.requiredXPLevel && playerStats.GetMonsterPoints() >= previewGunData.monsterPointsCost)
        {
            previewGunData.owned = true;
            playerStats.AddMonsterPoints(-previewGunData.monsterPointsCost);
            inventory.AddWeapon(previewGunData);
        } 
    }
}
