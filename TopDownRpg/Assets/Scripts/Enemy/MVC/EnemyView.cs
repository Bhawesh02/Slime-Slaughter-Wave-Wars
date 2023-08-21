
using System.Collections;
using UnityEngine;

public class EnemyView : MonoBehaviour, IDamageable
{

    [SerializeField]
    private EnemyScriptableObject enemyScriptableObject;
    public EnemyModel Model;
    public EnemyController Controller;

    public EnemyChaseState ChaseState;
    public EnemyIdelState IdelState;
    public EnemyFightState FightState;
    public EnemyState CurrentState { get; private set; }

    public Animator GetAnimator { get; private set; }

    public Rigidbody2D GetRigidbody { get; private set; }

    public SpriteRenderer GetSpriteRenderer { get; private set; }


    public bool ShowGizmos;

    private Coroutine playerDetectCoroutine = null;


    private void Awake()
    {
        Model = new(enemyScriptableObject);
        Controller = new(this, Model);
        GetAnimator = GetComponent<Animator>();
        GetRigidbody = GetComponent<Rigidbody2D>();
        GetSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        if (Model == null)
            return;
        Controller.ResetHealth(enemyScriptableObject);
        Start();
    }
    private void Start()
    {
        ChangeState(IdelState);
        playerDetectCoroutine = StartCoroutine(playerDetect());

    }
    private IEnumerator playerDetect()
    {
        Controller.PlayerDetect();
        if (Model.PlayerTransform != null)
            Controller.CheckIfPlayerIsInSight();
        
        yield return new WaitForSeconds(Model.DetectionDelay);
        playerDetectCoroutine = StartCoroutine(playerDetect());

    }

    public void TakeDamage()
    {
        Controller.ReduceHealth();
    }
    public void ChangeState(EnemyState state)
    {
        CurrentState?.OnStateExit();
        CurrentState = state;
        CurrentState.OnStateEnter();
    }
    public void EnemyDied()
    {
        EnemyPoolService.Instance.ReturnEnemy(this);
        EventService.Instance.InvokeEnemyDied();
        StopCoroutine(playerDetectCoroutine);
    }
    private void OnDisable()
    {
        CurrentState?.OnStateExit();
    }
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (!ShowGizmos)
            return;
        if(Model == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Model.ObstacelDetectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Model.ChaseRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Model.FightRadius);
        Controller?.DrawDetectionGizmos();

    }
}
