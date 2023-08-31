

using UnityEngine;

public class EnemyFightState : EnemyState
{
    private PlayerView playerView;
    private float nextPlayerCheckTime;
    private float nextAttackTime;

    protected override void Start()
    {
        base.Start();
        nextPlayerCheckTime = Time.time;
        nextAttackTime = Time.time;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        if (Model == null || Controller == null)
            SetModelController();
        playerView = Model.PlayerTransform.GetComponent<PlayerView>();
    }
    
    private void Update()
    {
        if(Time.time >= nextPlayerCheckTime)
        {
            PlayerCheck();
            nextPlayerCheckTime = Time.time + Model.DetectionDelay;
        }
        if (Model.CurrentState != this)
            return;

        if(Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + Model.AttackDelay;
        }
    }
    private void PlayerCheck()
    {
        if (Model.PlayerTransform == null || Vector2.Distance(transform.position, Model.PlayerTransform.position) > Model.FightRadius)
        {

            Controller.ChangeState(View.IdelState);
            
        }
    }

    private void AttackPlayer()
    {
        playerView.TakeDamage(Model.AttackPower);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
