
using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController
{
    private PlayerModel playerModel;
    private PlayerView playerView;
    public PlayerController(PlayerModel playerModel, PlayerView playerView)
    {
        this.playerModel = playerModel;
        this.playerView = playerView;
    }

    public void MovePlayer(float horizontalInput, float verticalInput)
    {
        Vector2 movement = new(horizontalInput, verticalInput);
        playerView.PlayerRigidBody.velocity = playerModel.MovementSpeed * Time.deltaTime * movement;
    }
    public void ChangeLookDirection(LookDirection direction)
    {
        if (direction == playerModel.CurrentLookDirection)
            return;
        switch (direction)
        {
            case LookDirection.Down:
                setAnimatorDirectionBoolTrue("IsLookingDown");
                break;
            case LookDirection.Up:
                setAnimatorDirectionBoolTrue("IsLookingUp");

                break;
            case LookDirection.Left:
                setAnimatorDirectionBoolTrue("IsLookingHrizontal");
                playerView.PlayerSpriteRenderer.flipX = true;
                break;
            case LookDirection.Right:
                setAnimatorDirectionBoolTrue("IsLookingHrizontal");
                playerView.PlayerSpriteRenderer.flipX = false;
                break;
        }
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
        playerView.PlayerAnimator.SetBool("IsAttacking", true);

    }
}
