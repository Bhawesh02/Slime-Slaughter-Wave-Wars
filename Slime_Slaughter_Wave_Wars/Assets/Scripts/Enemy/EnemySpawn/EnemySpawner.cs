
using UnityEngine;

public class EnemySpawner : MonoSingletonGeneric<EnemySpawner>
{
    [SerializeField]
    private EnemyScriptableObject enemyScriptableObject;
    [SerializeField]
    private EnemyView enemyView;
    [SerializeField]
    private Collider2D levelConfiner;
    [SerializeField]
    private LayerMask layerToNotSpawnEnemy;

    private int maxTries = 500;
    [SerializeField]
    private float spawnOffset = 0.5f;
    


    private EnemyPoolService enemyPoolService;
    protected override void Awake()
    {
        base.Awake();
        enemyPoolService = EnemyPoolService.Instance;
        enemyPoolService.MakeEnemyPool(enemyView,this);
    }
    public void SpawnEnemy()
    {
        
        EnemyView newEnemy = enemyPoolService.GetEnemy();
        newEnemy.transform.position = GetRandomSpawnPoint(newEnemy);
        newEnemy.gameObject.SetActive(true);
        
        EventService.Instance.InvokeEnemySpawned(newEnemy);
    }

    private Vector2 GetRandomSpawnPoint(EnemyView enemy)
    {
        Vector2 randomPoint = Vector2.zero;
        Collider2D collider2D;
        for (int i = 0; i < maxTries; i++)
        {
            randomPoint = GetRandomPositionInLevel();
            collider2D = Physics2D.OverlapCircle(randomPoint, enemy.Controller.Model.ColliderSize, layerToNotSpawnEnemy);
            if(collider2D == null){
                break;
            }
        }
        return randomPoint;
    }

    private Vector2 GetRandomPositionInLevel()
    {
        Vector2 randomPoint;

        Bounds colliderBounds = levelConfiner.bounds;
        float minX = colliderBounds.min.x;
        float minY = colliderBounds.min.y;

        while (true)
        {
            randomPoint = new Vector2(
                Random.Range(minX + spawnOffset, colliderBounds.max.x - spawnOffset) ,
                Random.Range(minY + spawnOffset, colliderBounds.max.y - spawnOffset)
            );

            if (levelConfiner.OverlapPoint(randomPoint))
            {
                return randomPoint;
            }
        }

    }

    

}
