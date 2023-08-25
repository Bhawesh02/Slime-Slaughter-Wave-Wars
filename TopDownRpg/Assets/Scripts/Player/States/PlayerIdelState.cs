
using UnityEngine;

public class PlayerIdelState : PlayerState
{
   public override void OnStateEnter()
    {
        base.OnStateEnter();
        playerView.PlayerAnimator.SetBool("IsMoving", false);
        playerView.PlayerRigidBody.velocity = Vector2.zero;
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
