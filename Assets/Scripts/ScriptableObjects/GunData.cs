using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName  = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    public int bulletsPerShot;
    public float bulletSpread;
    public float bulletSpeed = 200;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    
    [Header("Recoil")]
    public float hipFireRecoilX = -2.0f;
    public float hipFireRecoilY = 2.0f;
    public float hipFireRecoilZ = 0.35f;
    public float adsRecoilX = -2.0f;
    public float adsRecoilY = 2.0f;
    public float adsRecoilZ = 0.35f;
    public float snappiness = 6.0f;
    public float returnSpeed = 2.0f;
    
    public float kickBackAmount = -0.5f;
    public float kickBackSpeed = 10;
    public float gunReturnSpeed = 20;
    
    [HideInInspector]
    public bool reloading;
}
