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

    public TextMeshProUGUI currentMagAmmo;

    public TextMeshProUGUI maxMagAmmo;

    public Slider reloadSlider;

    [SerializeField]
    private ParticleSystem impactParticleSystem;

    [SerializeField]
    private TrailRenderer bulletTrail;

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

    private void OnDisable()
    {
        if (gunData != null)
        {
            gunData.reloading = false;
        }
    }

    public void StartReload()
    {
        if (!gunData.reloading && gameObject.activeSelf)
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
                    TrailRenderer trail = Instantiate(bulletTrail, transform.position, Quaternion.identity);

                    StartCoroutine(SpawnTrail(trail, hitInfo));
                    
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

                gunData.currentAmmo--;
                currentMagAmmo.text = gunData.currentAmmo.ToString();
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;

        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time = Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
        
        Destroy(trail.gameObject, trail.time);
    }

    private void OnGunShot()
    {
        
    }
}
