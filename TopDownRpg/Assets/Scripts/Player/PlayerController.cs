
using UnityEngine;

public class PlayerController 
{
    private PlayerModel playerModel;
    private PlayerView playerView;
   public PlayerController(PlayerModel playerModel,PlayerView playerView)
    {
        this.playerModel = playerModel;
        this.playerView = playerView;
    }

    public void MoveUp()
    {
        playerView.transform.Translate(playerModel.MovementSpeed * Time.deltaTime * Vector2.up);
    }
    public void MoveDown() {
        playerView.transform.Translate(playerModel.MovementSpeed * Time.deltaTime * Vector2.down);

    }
    public void MoveLeft() {
        playerView.transform.Translate(playerModel.MovementSpeed * Time.deltaTime * Vector2.left);
    }
    public void MoveRight() {
        playerView.transform.Translate(playerModel.MovementSpeed * Time.deltaTime * Vector2.right);
    }

}
