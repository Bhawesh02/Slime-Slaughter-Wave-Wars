

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
        if(playerView.HorizontalInput == 0)
        {
            if(playerView.VerticalInput == 1)
            {
                playerController.ChangeLookDirection(LookDirection.Up);
            }
            else if(playerView.VerticalInput == -1)
            {
                playerController.ChangeLookDirection(LookDirection.Down);
            }
        }
        else if(playerView.HorizontalInput == 1)
            playerController.ChangeLookDirection(LookDirection.Right);
        else
            playerController.ChangeLookDirection(LookDirection.Left);
        playerController.MovePlayer(playerView.HorizontalInput, playerView.VerticalInput);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
