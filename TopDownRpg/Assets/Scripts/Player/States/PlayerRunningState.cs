

using UnityEngine;

public class PlayerRunningState : PlayerState
{
    

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        playerView.PlayerAnimator.SetBool("IsMoving",true);
    }

    public override void Update()
    {
        base.Update();
        if (playerView.HorizontalInput == 0 && playerView.VerticalInput == 0)
            playerView.ChangeState(playerView.PlayerIdelState);
        
        playerController.MovePlayer(playerView.HorizontalInput, playerView.VerticalInput);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
