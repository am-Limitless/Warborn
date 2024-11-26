using UnityEngine;

public class RunState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.SetBool("Running", true);
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ExistState(movement, movement.Walk);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            ExistState(movement, movement.Crounch);
        }
        else if (movement.dir.magnitude > 0.1f)
        {
            ExistState(movement, movement.Idle);
        }
    }

    void ExistState(MovementStateManager movement, MovementBaseState state)
    {
        movement.animator.SetBool("Running", false);
        movement.SwicthState(state);
    }
}
