
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

    private float nextDetectionTime;

    public bool ShowGizmos;

    private void Awake()
    {
        Controller = new(this, Model);
        GetAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        nextDetectionTime = Time.time;
        ChangeState(ChaseState);
    }
    private void Update()
    {
        if(Time.time >= nextDetectionTime) 
        { 
            Controller.DetectObstacelsAndPlayer();
            nextDetectionTime = Time.time + Model.DetectionDelay;
        }
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
