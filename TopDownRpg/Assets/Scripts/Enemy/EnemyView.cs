
using UnityEngine;

public class EnemyView : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyModel model;
    public EnemyModel Model { get { return model; } }
    public EnemyController Controller;

    public Animator GetAnimator { get; private set; }

    private float nextDetectionTime;
    private void Awake()
    {
        Controller = new(this, Model);
        GetAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        nextDetectionTime = Time.time;
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

    public void EnemyDied()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Model.ObstacelDetectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Model.TargetDetectionRadius);
        Controller?.DrawDetectionGizmos();
    }
}
