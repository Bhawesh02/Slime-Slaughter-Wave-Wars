


public class PlayerRunningState : PlayerState
{
    

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        playerController ??= playerView.Controller;
        playerController.PlayPlayerRunningAnimation();
    }

    public void FixedUpdate()
    {
        playerController.MovePlayer();
    }

    public override void OnStateExit()
    {

        base.OnStateExit();
    }
}
