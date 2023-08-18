
using UnityEngine;

[RequireComponent(typeof(PlayerView))]
public class PlayerState : MonoBehaviour
{
    protected PlayerView playerView;
    protected PlayerController playerController;
    private void Awake()
    {
        playerView = GetComponent<PlayerView>();
    }
    private void Start()
    {
        playerController = playerView.PlayerController;
    }
    public virtual void Update()
    {
        playerView.HorizontalInput = Input.GetAxisRaw("Horizontal");
        playerView.VerticalInput = Input.GetAxisRaw("Vertical");
        changeLookDirectionBasedOnInput();
    }

    private void changeLookDirectionBasedOnInput()
    {
        if (playerView.HorizontalInput != 0)
        {
            if (playerView.HorizontalInput == 1)
                playerController.ChangeLookDirection(LookDirection.Right);
            else
                playerController.ChangeLookDirection(LookDirection.Left);
        }
        else
        {
            if (playerView.VerticalInput == 1)
            {
                playerController.ChangeLookDirection(LookDirection.Up);
            }
            else if (playerView.VerticalInput == -1)
            {
                playerController.ChangeLookDirection(LookDirection.Down);
            }
        }
    }

    public virtual void OnStateEnter()
    {
        this.enabled = true;
    }
    public virtual void OnStateExit()
    {
        this.enabled = false;
    }
}

public enum LookDirection
{
    Down,
    Up,
    Left,
    Right
}