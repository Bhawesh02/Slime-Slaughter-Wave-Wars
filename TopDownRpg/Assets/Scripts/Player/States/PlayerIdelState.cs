
using UnityEngine;

public class PlayerIdelState : PlayerState
{
   public override void OnStateEnter()
    {
        base.OnStateEnter();
        playerView.PlayerAnimator.SetBool("IsMoving", false);
    }
    public override void Update()
    {
        base.Update();
        if (playerView.HorizontalInput != 0 || playerView.VerticalInput != 0)
            playerView.ChangeState(playerView.PlayerRunningState);
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
