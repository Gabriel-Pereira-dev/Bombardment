using UnityEngine;

public class Idle : State
{
    private PlayerController controller;

    public Idle(PlayerController controller) : base("Idle")
    {
        this.controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        Debug.Log("Saiu no Idle");
    }

    public override void Update()
    {
        base.Update();
        // Change to Walking
        if (!controller.movementVector.IsZero())
        {
            controller.stateMachine.ChangeState(controller.walkingState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

}
