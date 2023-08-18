
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private PlayerModel playerModel;

    private PlayerController playerController;
    public Rigidbody2D PlayerRigidBody { get; private set; }

    void Start()
    {
        playerController = new(playerModel,this);
        PlayerRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        playerMovement();
    }

    private void playerMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        playerController.MovePlayer(horizontalInput, verticalInput);
        
        
    }
}
