using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private float moveTimer;
    public override void Enter()
    {
        enemy.Agent().SetDestination(enemy.LastKnowPos);
    }

    public override void Perform()
    {
        enemy.DoSearchState();
    }

    public override void Exit()
    {
        
    }
}
