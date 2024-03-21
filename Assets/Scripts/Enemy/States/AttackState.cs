using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackState : BaseState
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Enter()
    {
        
    }

    public override void Perform()
    {
        enemy.DoAttackState();
    }

    public override void Exit()
    {
        
    }
}
