
using UnityEngine;

public class EnemySpawner : MonoSingletonGeneric<EnemySpawner>
{
    [SerializeField]
    private EnemyView enemyView;

    private EnemyPoolService enemyPoolService;
    protected override void Awake()
    {
        base.Awake();
        enemyPoolService = EnemyPoolService.Instance;
        enemyPoolService.MakeEnemyPool(enemyView,this);
    }

}
