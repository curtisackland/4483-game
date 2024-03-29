using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GunData gunData;

    private TextMeshProUGUI currentMagAmmo;

    private TextMeshProUGUI maxMagAmmo;
    
    private TextMeshProUGUI totalAmmo;
    
    private Image ammoTypeImage;

    private Slider reloadSlider;

    public Transform barrelPosition;
    
    public Transform ADSbarrelPosition;

    public Light muzzleFlashLight;

    public ParticleSystem muzzleFlashParticles;

    private float lightIntensity;

    [SerializeField] private float lightReturnSpeed = 10f;

    [SerializeField]
    private TrailRenderer bulletTrail;

    [SerializeField]
    private GameObject impactParticles;

    private float timeSinceLastShot;

    private float reloadTimer = 0f;
    
    private LayerMask layerMask;

    private ScopeIn gunScopeIn;

    private GunRecoil gunRecoil;

    private AudioSource gunshotAudio;

    private AudioSource reloadAudio;

    private InventoryController inventory;

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
        
        UIWeaponData temp = GetComponentInParent<UIWeaponData>();
        reloadSlider = temp.reloadSlider;
        reloadSlider.value = 0f;
        reloadSlider.gameObject.SetActive(false);
        currentMagAmmo = temp.currentMagAmmo;
        maxMagAmmo = temp.maxMagAmmo;
        totalAmmo = temp.totalAmmo;
        ammoTypeImage = temp.ammoTypeImage;
        inventory = temp.inventoryController;
        
        layerMask = LayerMask.GetMask("Default", "Water", "Spawnable", "Enemy");
        gunScopeIn = GetComponent<ScopeIn>();
        gunRecoil = GetComponent<GunRecoil>();
        gunshotAudio = GetComponents<AudioSource>()[0];
        reloadAudio = GetComponents<AudioSource>()[1];

        // muzzle flash
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
    }

    private void Update()
    {
        currentMagAmmo.text = gunData.currentAmmo.ToString();
        maxMagAmmo.text = gunData.magSize.ToString();
        totalAmmo.text = inventory.GetAmmoCount(gunData.ammoType).ToString();
        ammoTypeImage.sprite = Resources.Load<Sprite>("Icons/" + gunData.ammoType + " Ammo");
        timeSinceLastShot += Time.deltaTime;
        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        if (gunData != null)
        {
            gunData.reloading = false;
        }
    }

    public void StartReload()
    {
        if (gameObject.activeSelf && !gunData.reloading && inventory.GetAmmoCount(gunData.ammoType) > 0 && gunData.currentAmmo != gunData.magSize)
        {
            reloadAudio.Play();
            reloadTimer = 0;
            reloadSlider.gameObject.SetActive(true);
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;

        while (reloadTimer < gunData.reloadTime)
        {
            reloadTimer += Time.deltaTime;
            float progress = reloadTimer / gunData.reloadTime;
            reloadSlider.value = Mathf.Lerp(0, gunData.reloadTime, progress);
            yield return null;
        }

        // decrease ammo on reload
        int availableAmmo = Math.Min(gunData.magSize - gunData.currentAmmo, inventory.GetAmmoCount(gunData.ammoType));
        gunData.currentAmmo += availableAmmo;
        inventory.UpdateAmmo(gunData.ammoType, -availableAmmo);
        currentMagAmmo.text = gunData.currentAmmo.ToString();
        reloadSlider.gameObject.SetActive(false);

        gunData.reloading = false;
    }

    public void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                
                Transform camTransform = transform.parent.parent.transform;
                for (int i = 0; i < gunData.bulletsPerShot; i++) {
                    Vector3 direction = Quaternion.Euler(UnityEngine.Random.Range(-gunData.bulletSpread, gunData.bulletSpread), UnityEngine.Random.Range(-gunData.bulletSpread, gunData.bulletSpread), 0) * camTransform.forward;

                    // If the bullet hit something
                    if (Physics.Raycast(camTransform.position, direction, out RaycastHit hitInfo,
                            gunData.maxDistance,
                            layerMask))
                    {
                        TrailRenderer trail = Instantiate(bulletTrail,
                            gunScopeIn.isADS() ? ADSbarrelPosition.position : barrelPosition.position, Quaternion.identity);

                        StartCoroutine(SpawnTrail(trail, hitInfo.point, hitInfo.normal, true, hitInfo.collider.gameObject));

                        Transform tf = hitInfo.transform;
                        while (tf != null)
                        {
                            Damageable damageable = tf.GetComponent<Damageable>();
                            if (damageable != null)
                            {
                                damageable.Damage(gunData.damage);
                                break;
                            }

                            tf = tf.parent;

                        } 
                    }
                    // If the bullet did not hit something
                    else
                    {
                        TrailRenderer trail = Instantiate(bulletTrail,
                            gunScopeIn.isADS() ? ADSbarrelPosition.position : barrelPosition.position, Quaternion.identity);

                        StartCoroutine(SpawnTrail(trail, (gunScopeIn.isADS() ? ADSbarrelPosition.position : barrelPosition.position) + camTransform.forward * 200, Vector3.zero, false, null));
                    }
                }
                
                
                gunData.currentAmmo--;
                currentMagAmmo.text = gunData.currentAmmo.ToString();
                timeSinceLastShot = 0;
                gunRecoil.RecoilFire();
                OnGunShot();
            }
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hit, Vector3 hitNormal, bool madeImpact, GameObject rayCollider)
    {
        // Bullet tracer travelling to target
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(trail.transform.position, hit);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit, 1 - (remainingDistance / distance));

            remainingDistance -= gunData.bulletSpeed * Time.deltaTime;

            yield return null;
        }

        trail.transform.position = hit;

        // if impact was made create an impact and make it attached to what it hit
        if (madeImpact)
        {
            if (rayCollider != null)
            {
                GameObject impact = Instantiate(impactParticles, hit, Quaternion.LookRotation(hitNormal));
                impact.transform.parent = rayCollider.transform;
            }
        }
        
        Destroy(trail.gameObject, trail.time);
    }

    private void OnGunShot()
    {
        // Muzzle flash
        if (gunScopeIn.isADS())
        {
            muzzleFlashLight.transform.position = ADSbarrelPosition.transform.position;
            muzzleFlashParticles.transform.position = ADSbarrelPosition.transform.position;
        }
        else
        {
            muzzleFlashLight.transform.position = barrelPosition.transform.position;
            muzzleFlashParticles.transform.position = barrelPosition.transform.position;
        }
        muzzleFlashParticles.Play();
        muzzleFlashLight.intensity = lightIntensity;
        
        // Gunshot audio
        gunshotAudio.Play();
    }

    private void OnDestroy()
    {
        PlayerShoot.shootInput -= Shoot;
        PlayerShoot.reloadInput -= StartReload;
    }

    private void OnEnable()
    {
        if (reloadSlider != null)
        {
            reloadSlider.gameObject.SetActive(false);
        }
    }
}
