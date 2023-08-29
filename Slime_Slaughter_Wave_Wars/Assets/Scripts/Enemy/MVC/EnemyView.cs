
using System.Collections;
using UnityEngine;

public class EnemyView : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyScriptableObject enemyScriptableObject;
   
    public EnemyController Controller;

    public EnemyChaseState ChaseState;
    public EnemyIdelState IdelState;
    public EnemyFightState FightState;

    public Animator GetAnimator { get; private set; }

    public Rigidbody2D GetRigidbody { get; private set; }

    public SpriteRenderer GetSpriteRenderer { get; private set; }


    public bool ShowGizmos;

    public Coroutine PlayerDetectCoroutine { get; private set; }


    private void Awake()
    {
        Controller = new(this,enemyScriptableObject);
        GetAnimator = GetComponent<Animator>();
        GetRigidbody = GetComponent<Rigidbody2D>();
        GetSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        if(Controller.Model!= null && Controller.Model.CurrHealth!= Controller.Model.MaxHealth) 
            Controller.ResetHealth();
        Controller.ChangeState(IdelState);
        PlayerDetectCoroutine = StartCoroutine(playerDetect());

    }
    
    private IEnumerator playerDetect()
    {
        Controller.PlayerDetect();
        if (Controller.Model.PlayerTransform != null)
            Controller.CheckIfPlayerIsInSight();
        
        yield return new WaitForSeconds(Controller.Model.DetectionDelay);
        PlayerDetectCoroutine = StartCoroutine(playerDetect());

    }

    public void TakeDamage()
    {
        Controller.ReduceHealth();
    }
    public void Died()
    {
        Controller.EnemyDied();
    }
    
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (!ShowGizmos)
            return;
        if(Controller.Model == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Controller.Model.ObstacelDetectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Controller.Model.ChaseRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Controller.Model.FightRadius);
        Controller?.DrawDetectionGizmos();

    }
}
