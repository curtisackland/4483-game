using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GunData gunData;

    public TextMeshProUGUI currentMagAmmo;

    public TextMeshProUGUI maxMagAmmo;

    public Slider reloadSlider;

    private float timeSinceLastShot;

    private float reloadTimer = 0f;

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
        maxMagAmmo.text = gunData.magSize.ToString();
        reloadSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    public void StartReload()
    {
        if (!gunData.reloading)
        {
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

        //yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;
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
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
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
                    //hitInfo.transform.IsChildOf(Damageable);
                    //Damageable damageable = hitInfo.transform.GetComponent<Damageable>();
                    //damageable?.Damage(gunData.damage);
                }

                gunData.currentAmmo--;
                currentMagAmmo.text = gunData.currentAmmo.ToString();
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }

    private void OnGunShot()
    {
        
    }
}
