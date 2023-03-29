using UnityEngine;
using System.Collections;

public class StateRun : State
{

	public override void Execute(AIController character)
    {
        if (character.IsDead)
        {
            character.ChangeState(new StateDead());
        }
        else if (character.InDanger)
        {
            character.ChangeState(new StateRunAway());
        }
        //If see and in range, attack
        else if (character.EnemySeen() && character.EnemyInRange())
        {
            character.ChangeState(new StateAttack());
        }
        //If seen and out of range, approach
        else if (character.EnemySeen() && !character.EnemyInRange())
        {
            if (character.ShouldRun())
            {
                character.Run();
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
