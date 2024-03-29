using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMelee : Enemy
{
    protected float searchTimer;
    protected float moveTimer;

    public float attackDistance = 2;
    public float followDistance = 1;
    public float attackRate = 1;
}
