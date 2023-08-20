
using System;
using UnityEngine;

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

    public float HorizontalInput;

    public float VerticalInput;

    public Transform AttackPoint;

    private float nextSwingTime;

    public float AttackPointInitialYOffset;

    
    private void Awake()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        AttackPointInitialYOffset = AttackPoint.transform.position.y;
    }
    private void Start()
    {
        PlayerController = new(PlayerModel, this);
        PlayerController.ChangeLookDirection(LookDirection.Down);
        ChangeState(PlayerIdelState);
        nextSwingTime = Time.time;

        GameManager.Instance.Player = this;
    }

    private void Update()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
        changeLookDirectionBasedOnInput();
        playerAttackCheck();
    }
    private void playerAttackCheck()
    {
        if (Time.time >= nextSwingTime)
        {
            if (Input.GetMouseButton(0))
            {
                PlayerController.PlayerAttack();
                nextSwingTime = Time.time + PlayerModel.SwingRate;
            }
            else
            {
                PlayerAnimator.SetBool("IsAttacking", false);
            }
        }
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
        PlayerController.ReduceHealth(attackPower);
    }
}
