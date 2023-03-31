using UnityEngine;
using System.Collections;

public class StateAttack : State
{

	public override void Execute(AIController character)
	{
        if (character.IsDead)
        {
            character.ChangeState(new StateDead());
        }
        //If see and in range, attack
        else if (character.EnemySeen() && character.EnemyInRange())
        {
            character.Attack();
        }
        //If seen and out of range, approach
        else if (character.EnemySeen() && !character.EnemyInRange())
        {
            if (character.ShouldRun())
            {
                character.ChangeState(new StateRun());
            }
            else
            {
                character.ChangeState(new StateWalk());
            }
        }
        //Otherwise, idle away
        else
        {
            character.ChangeState(new StateIdle());
        }
    }
}
