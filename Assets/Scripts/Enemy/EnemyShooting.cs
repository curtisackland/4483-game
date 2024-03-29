using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyShooting : Enemy
{
    [Header("Weapon Values")]
    public Transform gunBarrel;

    [Range(0.1f, 10)]
    public float fireRate;
    
    protected float searchTimer;
    protected float moveTimer;
    
    protected float shotTimer;
    
   
}
