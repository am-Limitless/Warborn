using UnityEngine;

public class WalkState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.SetBool("Walking", true);
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ExistState(movement, movement.Run);
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
        movement.animator.SetBool("Walking", false);
        movement.SwicthState(state);
    }
}
