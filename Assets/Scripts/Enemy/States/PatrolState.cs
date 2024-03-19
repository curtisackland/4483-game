using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{

    public int waypointIndex;
    public float waitTimer;
    
    private float moveTimer;
    public override void Enter()
    {
        
    }
    
    public override void Perform()
    {
        enemy.DoPatrolState();
        
    }
    
    public override void Exit()
    {
        
    }

    
}
