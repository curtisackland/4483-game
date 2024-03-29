using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    public float damage = 10;
    public bool damageEnabled = true;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        Transform hitTransform = other.transform;
        
        if (damageEnabled && hitTransform.CompareTag("Player"))
        {
            hitTransform.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
        Debug.Log("Melee Hit");
    }
}
