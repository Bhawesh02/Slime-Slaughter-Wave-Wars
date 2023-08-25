
using System;
using System.Collections;
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
    public PlayerState CurrentState;

    public Vector2 MoveVector = Vector2.zero;


    public Transform AttackPoint;




    public float AttackPointInitialYOffset { get; private set; }

    private InputMaster inputs;

    private bool canAttack = false;

    private float nextSwingTime;
    private void Awake()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        AttackPointInitialYOffset = AttackPoint.transform.position.y;
        inputs = new();
    }
    private void Start()
    {
        PlayerController = new(PlayerModel, this);
        PlayerController.SetLookAndAttackPointDirection(LookDirection.Down);
        ChangeState(PlayerIdelState);
        nextSwingTime = Time.time;
        GameManager.Instance.Player = this;
    }
    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Movement.performed += OnMovementPerformed;
        inputs.Player.Movement.canceled += OnMovementCancelled;
        inputs.Player.Shoot.performed += _ => canAttack = true;
        inputs.Player.Shoot.canceled += _ => canAttack = false;
    }

    private void Update()
    {
        PerformAttackCheck();
    }
    private void PerformAttackCheck()
    {
        if (!canAttack)
            return;
        if (Time.time >= nextSwingTime)
        {
            PlayerController.PlayerAttack();
            nextSwingTime = Time.time + PlayerModel.AttackRate;
        }

    }


    #region Player Direction Set
    private void OnMovementPerformed(InputAction.CallbackContext value)
    {

        MoveVector = value.ReadValue<Vector2>();
        changeLookDirectionBasedOnInput();
        ChangeState(PlayerRunningState);
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        MoveVector = Vector2.zero;
        ChangeState(PlayerIdelState);
    }
    private void changeLookDirectionBasedOnInput()
    {

        if (MoveVector.x != 0)
        {
            if (MoveVector.x > 0)
                PlayerController.SetLookAndAttackPointDirection(LookDirection.Right);
            else
                PlayerController.SetLookAndAttackPointDirection(LookDirection.Left);
        }
        else
        {
            if (MoveVector.y > 0)
            {
                PlayerController.SetLookAndAttackPointDirection(LookDirection.Up);
            }
            else if (MoveVector.y < 0)
            {
                PlayerController.SetLookAndAttackPointDirection(LookDirection.Down);
            }
        }
    }

    #endregion
    public void ChangeState(PlayerState playerState)
    {
        CurrentState?.OnStateExit();
        CurrentState = playerState;
        CurrentState.OnStateEnter();
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.position, PlayerModel.AttackRadius);
    }

    public void TakeDamage(int attackPower)
    {
        PlayerController.ReduceHealth(attackPower);
        GameManager.Instance.SetPlayerHealthInSlider();
    }
    private void OnDisable()
    {
        inputs.Disable();
        inputs.Player.Movement.performed -= OnMovementPerformed;
        inputs.Player.Movement.performed -= OnMovementCancelled;
        inputs.Player.Shoot.performed -= _ => canAttack = true;
        inputs.Player.Shoot.canceled -= _ => canAttack = false;



    }
}
