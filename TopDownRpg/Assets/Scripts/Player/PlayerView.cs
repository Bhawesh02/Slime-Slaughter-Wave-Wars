
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private PlayerModel playerModel;

    private PlayerController playerController;
    public Rigidbody2D PlayerRigidBody { get; private set; }

    public Animator PlayerAnimator { get; private set; }

    private float horizontalInput;

    private float verticalInput;

    void Start()
    {
        playerController = new(playerModel,this);
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        playerController.MovePlayer(horizontalInput, verticalInput);
    }

}
