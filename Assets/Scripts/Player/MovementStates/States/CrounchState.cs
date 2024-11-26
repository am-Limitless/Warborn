using UnityEngine;

public class CrounchState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.SetBool("Crouching", true);
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ExistState(movement, movement.Run);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (movement.dir.magnitude > 0.1f)
            {
                ExistState(movement, movement.Idle);
            }
            else
            {
                ExistState(movement, movement.Walk);
            }
        }
    }

    void ExistState(MovementStateManager movement, MovementBaseState state)
    {
        movement.animator.SetBool("Crouching", false);
        movement.SwicthState(state);
    }
}
