
using UnityEngine;

public class EnemyController 
{
    private EnemyView enemyView;
    private EnemyModel enemyModel;
    public EnemyController(EnemyView enemyView, EnemyModel enemyModel) 
    {
        this.enemyView = enemyView;
        this.enemyModel = enemyModel;
    }

    public void ReduceHealth()
    {
        enemyModel.Health -= 10;
        if(enemyModel.Health <= 0 )
        {
            enemyView.GetAnimator.SetTrigger("Died");
        }
        else
        {
            enemyView.GetAnimator.SetTrigger("Attacked");
        }
    }
}
