
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private PlayerScriptableObject playerScriptableObject;

    public PlayerController Controller { get; private set; }
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
        Controller = new(playerScriptableObject, this);
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        AttackPointInitialYOffset = AttackPoint.transform.position.y;
        inputs = new();
    }
    private void Start()
    {
        Controller.SetLookAndAttackPointDirection(LookDirection.Down);
        Controller.ChangeState(PlayerIdelState);
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
            Controller.PlayerAttack();
            nextSwingTime = Time.time + Controller.Model.AttackRate;
        }

    }


    #region Player Direction Set
    private void OnMovementPerformed(InputAction.CallbackContext value)
    {

        MoveVector = value.ReadValue<Vector2>();
        changeLookDirectionBasedOnInput();
        Controller.ChangeState(PlayerRunningState);
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        MoveVector = Vector2.zero;
        Controller.ChangeState(PlayerIdelState);
    }
    private void changeLookDirectionBasedOnInput()
    {

        if (MoveVector.x != 0)
        {
            if (MoveVector.x > 0)
                Controller.SetLookAndAttackPointDirection(LookDirection.Right);
            else
                Controller.SetLookAndAttackPointDirection(LookDirection.Left);
        }
        else
        {
            if (MoveVector.y > 0)
            {
                Controller.SetLookAndAttackPointDirection(LookDirection.Up);
            }
            else if (MoveVector.y < 0)
            {
                Controller.SetLookAndAttackPointDirection(LookDirection.Down);
            }
        }
    }

    #endregion
    


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
        Gizmos.DrawWireSphere(AttackPoint.position, Controller.Model.AttackRadius);
    }

    public void TakeDamage(int attackPower)
    {
        Controller.ReduceHealth(attackPower);
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
