
using System.Collections;
using UnityEngine;

public class EnemyView : MonoBehaviour, IDamageable
{

    [SerializeField]
    private EnemyScriptableObject enemyScriptableObject;
    public EnemyModel Model { get; private set; }
    public EnemyController Controller;

    public EnemyChaseState ChaseState;
    public EnemyIdelState IdelState;
    public EnemyFightState FightState;
    public EnemyState CurrentState { get;private set; }

    public Animator GetAnimator { get; private set; }

    public Rigidbody2D GetRigidbody {  get; private set; }

    public SpriteRenderer GetSpriteRenderer { get; private set; }


    public bool ShowGizmos;

    Coroutine playerDetectCoroutine = null;

    private void Awake()
    {
        Model = new(enemyScriptableObject);
        Controller = new(this, Model);
        GetAnimator = GetComponent<Animator>();
        GetRigidbody = GetComponent<Rigidbody2D>();
        GetSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        ChangeState(IdelState);
        playerDetectCoroutine = StartCoroutine(playerDetect());
    }
    
    IEnumerator playerDetect()
    {
        Controller.PlayerDetect();
        yield return new WaitForSeconds(Model.DetectionDelay);
        playerDetectCoroutine = StartCoroutine(playerDetect());

    }
    public void TakeDamage()
    {
        Controller.ReduceHealth();
    }
    public void ChangeState(EnemyState state)
    {
        /*if(CurrentState!=null)
        Debug.Log("New State: " + state + "  Current State" + CurrentState);*/
        CurrentState?.OnStateExit();
        CurrentState = state;
        CurrentState.OnStateEnter();
    }
    public void EnemyDied()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        CurrentState?.OnStateExit();
        StopCoroutine(playerDetectCoroutine);
    }
    private void OnDrawGizmos()
    {
        if (!ShowGizmos)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Model.ObstacelDetectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Model.ChaseRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Model.FightRadius);
        Controller?.DrawDetectionGizmos();

    }
}
