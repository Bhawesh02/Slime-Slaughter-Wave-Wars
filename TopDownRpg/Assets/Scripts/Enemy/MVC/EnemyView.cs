
using System.Collections;
using UnityEngine;

public class EnemyView : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyModel model;
    public EnemyModel Model { get { return model; } }
    public EnemyController Controller;

    public EnemyChaseState ChaseState;
    public EnemyIdelState IdelState;
    public EnemyFightState FightState;
    public EnemyState CurrentState { get;private set; }

    public Animator GetAnimator { get; private set; }

    public Rigidbody2D GetRigidbody {  get; private set; }



    public bool ShowGizmos;

    Coroutine playerDetectCoroutine = null;

    private void Awake()
    {
        Controller = new(this, Model);
        GetAnimator = GetComponent<Animator>();
        GetRigidbody = GetComponent<Rigidbody2D>();
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
        StopCoroutine(playerDetectCoroutine);
    }
    private void OnDrawGizmos()
    {
        if (!ShowGizmos)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Model.ObstacelDetectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Model.TargetDetectionRadius);
        Controller?.DrawDetectionGizmos();
    }
}
