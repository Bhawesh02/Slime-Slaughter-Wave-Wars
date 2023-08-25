
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerView : MonoBehaviour
{
    public PlayerModel PlayerModel;

    public PlayerController PlayerController { get; private set; }
    public Rigidbody2D PlayerRigidBody { get; private set; }

    public SpriteRenderer PlayerSpriteRenderer;

    public Animator PlayerAnimator;

    public PlayerIdelState PlayerIdelState;
    public PlayerRunningState PlayerRunningState;
    [SerializeField]
    private PlayerState currentPlayerState;

    public Vector2 MoveVector = Vector2.zero;

    public float HorizontalInput;

    public float VerticalInput;

    public Transform AttackPoint;

    private float nextSwingTime;

    public float AttackPointInitialYOffset;

    public InputMaster Inputs { get; private set; }
    
    private void Awake()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        AttackPointInitialYOffset = AttackPoint.transform.position.y;
        Inputs = new();
    }
    private void OnEnable()
    {
        Inputs.Enable();
        Inputs.Player.Movement.performed += OnMovementPerformed;
        Inputs.Player.Movement.performed += OnMovementCancelled;
    }
    private void Start()
    {
        PlayerController = new(PlayerModel, this);
        PlayerController.ChangeLookDirection(LookDirection.Down);
        ChangeState(PlayerIdelState);
        nextSwingTime = Time.time;

        GameManager.Instance.Player = this;
    }

    #region Player Control
    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        MoveVector = value.ReadValue<Vector2>();
        HorizontalInput = MoveVector.x;
        VerticalInput = MoveVector.y;
        changeLookDirectionBasedOnInput();
        playerAttackCheck();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        MoveVector = Vector2.zero;
    }
    private void changeLookDirectionBasedOnInput()
    {

        if (HorizontalInput != 0)
        {
            if (HorizontalInput == 1)
                PlayerController.ChangeLookDirection(LookDirection.Right);
            else
                PlayerController.ChangeLookDirection(LookDirection.Left);
        }
        else
        {
            if (VerticalInput == 1)
            {
                PlayerController.ChangeLookDirection(LookDirection.Up);
            }
            else if (VerticalInput == -1)
            {
                PlayerController.ChangeLookDirection(LookDirection.Down);
            }
        }
    }
    private void playerAttackCheck()
    {
        if (Time.time >= nextSwingTime && Input.GetMouseButton(0))
        {
            PlayerController.PlayerAttack();
            nextSwingTime = Time.time + PlayerModel.SwingRate;
        }
    }
    #endregion
    public void ChangeState(PlayerState playerState)
    {
        currentPlayerState?.OnStateExit();
        currentPlayerState = playerState;
        currentPlayerState.OnStateEnter();
    }

    private void OnDrawGizmos()
    {
       Gizmos.DrawWireSphere(AttackPoint.position, PlayerModel.AttackRadius);
    }

    public void TakeDamage(int attackPower)
    {
        PlayerAnimator.SetTrigger("Attacked");
        PlayerController.ReduceHealth(attackPower);
        GameManager.Instance.SetPlayerHealthInSlider();
    }
    private void OnDisable()
    {
        Inputs.Disable();
        Inputs.Player.Movement.performed -= OnMovementPerformed;
        Inputs.Player.Movement.performed -= OnMovementCancelled;

    }
}
