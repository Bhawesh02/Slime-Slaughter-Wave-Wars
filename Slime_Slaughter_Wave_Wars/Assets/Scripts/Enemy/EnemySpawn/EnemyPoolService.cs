
using UnityEngine;

public class EnemyPoolService : SingletonGeneric<EnemyPoolService>
{
    private EnemyPool enemyPool = null;

    public void MakeEnemyPool(EnemyView enemyView, EnemySpawner parent)
    {
        enemyPool = new EnemyPool(enemyView, parent);
    }
    public EnemyView GetEnemy()
    {

        return enemyPool.GetItem();
    }
    public void ReturnEnemy(EnemyView enemyView)
    {
        enemyView.gameObject.SetActive(false);
        enemyPool.ReturnItem(enemyView);
    }

}

