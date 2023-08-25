
using UnityEngine;

public class PlayerController
{
    private PlayerModel playerModel;
    private PlayerView playerView;
    private float newXPosForAttackPoint;
    private float newYPosForAttackPoint;
    private Vector2 movementDirection;
    public PlayerController(PlayerModel playerModel, PlayerView playerView)
    {
        this.playerModel = playerModel;
        this.playerView = playerView;
    }

    public void MovePlayer()
    {
        movementDirection = playerView.MoveVector;
        movementDirection.Normalize();
        playerView.PlayerRigidBody.velocity = playerModel.MovementSpeed * Time.deltaTime * movementDirection;
    }
    public void SetLookAndAttackPointDirection(LookDirection direction)
    {
        if (direction == playerModel.CurrentLookDirection)
            return;
        switch (direction)
        {
            case LookDirection.Down:
                setAnimatorDirectionBoolTrue("IsLookingDown");
                newXPosForAttackPoint = playerView.transform.position.x + playerView.PlayerModel.AttackPointDownXOffset;
                newYPosForAttackPoint = playerView.transform.position.y + playerView.PlayerModel.AttackPointDownYOffset + playerView.AttackPointInitialYOffset;
                break;
            case LookDirection.Up:
                setAnimatorDirectionBoolTrue("IsLookingUp");
                newXPosForAttackPoint = playerView.transform.position.x + playerView.PlayerModel.AttackPointUpXOffset;
                newYPosForAttackPoint = playerView.transform.position.y + playerView.PlayerModel.AttackPointUpYOffset + playerView.AttackPointInitialYOffset;
                

                break;
            case LookDirection.Left:
                setAnimatorDirectionBoolTrue("IsLookingHorizontal");
                newXPosForAttackPoint = playerView.transform.position.x + playerView.PlayerModel.AttackPointLeftXOffset;
                newYPosForAttackPoint = playerView.transform.position.y + playerView.PlayerModel.AttackPointLeftYOffset + playerView.AttackPointInitialYOffset;
                

                playerView.PlayerSpriteRenderer.flipX = true;
                break;
            case LookDirection.Right:
                setAnimatorDirectionBoolTrue("IsLookingHorizontal");
                newXPosForAttackPoint = playerView.transform.position.x + playerView.PlayerModel.AttackPointRightXOffset;
                newYPosForAttackPoint = playerView.transform.position.y + playerView.PlayerModel.AttackPointRightYOffset + playerView.AttackPointInitialYOffset;
               
                playerView.PlayerSpriteRenderer.flipX = false;
                break;
        }
        playerView.AttackPoint.position = new(newXPosForAttackPoint,newYPosForAttackPoint);
        playerModel.CurrentLookDirection = direction;
    }
    private void setAnimatorDirectionBoolTrue(string direction)
    {
        for (int i = 0; i < playerModel.AnimationDirectionBools.Length; i++)
        {

            if (playerModel.AnimationDirectionBools[i] == direction)
                playerView.PlayerAnimator.SetBool(playerModel.AnimationDirectionBools[i], true);
            else
                playerView.PlayerAnimator.SetBool(playerModel.AnimationDirectionBools[i], false);
        }
    }

    public void PlayerAttack()
    {
        playerView.PlayerAnimator.SetTrigger("Attacking");
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerView.AttackPoint.position,playerModel.AttackRadius);
        for(int i = 0; i < hits.Length; i++)
        {
            hits[i].GetComponent<IDamageable>()?.TakeDamage();
        }
    }

    public void ReduceHealth(int attackPower)
    {
        playerModel.CurrentHealth -= attackPower;
        if(playerModel.CurrentHealth <= 0)
        {
            PlayerDead();
            return;
        }

    }

    private void PlayerDead()
    {
        playerView.PlayerAnimator.SetTrigger("Dead");
        playerView.enabled = false;
        GameManager.Instance.PlayedDied();
    }
}
