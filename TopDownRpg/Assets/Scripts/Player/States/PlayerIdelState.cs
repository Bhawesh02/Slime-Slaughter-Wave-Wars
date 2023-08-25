
using UnityEngine;

public class PlayerIdelState : PlayerState
{
   public override void OnStateEnter()
    {
        base.OnStateEnter();
        playerController ??= playerView.PlayerController;
        playerController.PlayPlayerIdelAnimation();
        playerView.PlayerRigidBody.velocity = Vector2.zero;
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
