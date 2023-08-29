using UnityEngine;

public class EnemyPool : PoolService<EnemyView>
{
    private EnemyView enemyView;
    private EnemySpawner parent;

    public EnemyPool(EnemyView enemyView, EnemySpawner parent)
    {
        this.enemyView = enemyView;
        this.parent = parent;
    }
    protected override EnemyView CreateItem()
    {
        EnemyView item = GameObject.Instantiate(enemyView, parent.transform);
        return item;
    }
}
