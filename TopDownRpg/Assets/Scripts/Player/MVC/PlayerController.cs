
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
}
