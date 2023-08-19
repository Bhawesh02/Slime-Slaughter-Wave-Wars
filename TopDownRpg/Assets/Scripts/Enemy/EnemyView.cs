
using UnityEngine;

public class EnemyView : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyModel model;
    public EnemyModel Model {  get { return model; } }
    public EnemyController Controller;

    public Animator GetAnimator { get;private set; }
    private void Awake()
    {
        Controller = new(this,Model);
        GetAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
       Controller.DetectObstacelsAndPlayer();
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
