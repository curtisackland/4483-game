using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterCollider : MonoBehaviour
{
    public float speed = 1f;
    public float startDelay = 2f;
    public float maxDistance = 15f;
    public float damage = 10f;
    public float knockbackVelocity; 
    public bool damageEnabled = false;
    
    private float timeAlive;
    private Vector3 startingPos;
    private void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeAlive < startDelay) // Delay movement and damage
        {
            timeAlive += Time.deltaTime;
        }
        else // Start damaging and moving
        {
            damageEnabled = true;
            transform.position += transform.forward * (speed * Time.deltaTime);
            if ((startingPos - transform.position).magnitude >= maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform hitTransform = other.transform;
        
        if (damageEnabled && hitTransform.CompareTag("Player"))
        {
            Debug.Log("Shatter Hit");
            hitTransform.GetComponent<PlayerHealth>().TakeDamage(damage);
            hitTransform.GetComponent<PlayerMotor>().AddKnockback(transform.position, knockbackVelocity);
            Destroy(gameObject);
        }
    }
}
