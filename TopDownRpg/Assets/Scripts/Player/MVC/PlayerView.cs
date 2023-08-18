
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private PlayerModel playerModel;

    public PlayerController PlayerController { get; private set; }
    public Rigidbody2D PlayerRigidBody { get; private set; }

    public Animator PlayerAnimator { get; private set; }

    public PlayerIdelState PlayerIdelState;
    public PlayerRunningState PlayerRunningState;
    [SerializeField]
    private PlayerState currentPlayerState;

    public float HorizontalInput;

    public float VerticalInput;

    private void Awake()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        PlayerController = new(playerModel, this);
        PlayerAnimator.SetBool("IsLookingDown",true);
        ChangeState(PlayerIdelState);
    }

   
    public void ChangeState(PlayerState playerState)
    {
        currentPlayerState?.OnStateExit();
        currentPlayerState = playerState;
        currentPlayerState.OnStateEnter();
    }
}
