
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private PlayerModel playerModel;

    private PlayerController playerController;
    public Rigidbody2D RigidBody { get; private set; }

    void Start()
    {
        playerController = new(playerModel,this);
        RigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        playerMovement();
    }

    private void playerMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput > 0)
        {
            playerController.MoveRight();
        }
        else if (horizontalInput < 0)
        {
            playerController.MoveLeft();
        }
        float verticalInput = Input.GetAxisRaw("Vertical");
        if (verticalInput > 0)
        {
            playerController.MoveUp();
        }
        else if (verticalInput < 0)
        {
            playerController.MoveDown();
        }
    }
}
