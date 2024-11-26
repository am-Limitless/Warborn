using UnityEngine;

public class IdleState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {

    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.dir.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movement.SwicthState(movement.Run);
            }
            else
            {
                movement.SwicthState(movement.Walk);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            movement.SwicthState(movement.Crounch);
        }

    }
}

