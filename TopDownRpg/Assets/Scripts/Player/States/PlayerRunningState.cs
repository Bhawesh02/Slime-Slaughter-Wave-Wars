

using UnityEngine;

public class PlayerRunningState : PlayerState
{
    

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        playerView.PlayerAnimator.SetBool("IsMoving",true);
    }

    public void FixedUpdate()
    {
        playerController.MovePlayer();
    }

    public override void OnStateExit()
    {
        playerView.PlayerAnimator.SetBool("IsMoving", false);

        base.OnStateExit();
    }
}
