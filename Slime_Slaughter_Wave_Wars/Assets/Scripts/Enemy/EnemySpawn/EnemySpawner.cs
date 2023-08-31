
using System.Collections;
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

    private GameManager gameManager;
    private UIService uIService;

    public WaveSystemScriptableObject WaveSystem;

    public int CurrWave { get; private set; }

    private int numOfEnemyToSpawn;


    private EnemyPoolService enemyPoolService;
    protected override void Awake()
    {
        base.Awake();
        CurrWave = 0;
        enemyPoolService = EnemyPoolService.Instance;
        enemyPoolService.MakeEnemyPool(enemyView,this);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        uIService = UIService.Instance;
        EventService.Instance.EnemySpawned += IncreaseEnemyInScene;
        EventService.Instance.EnemyDied += DecreaseEnemyInScene;
        StartCoroutine(SpawnNewWave());

    }
    #region Instantiate Enemy at scene
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

    #endregion

    #region Spwan Wave
    private IEnumerator SpawnNewWave()
    {
        CurrWave++;
        if (CurrWave > WaveSystem.NumOfWaves)
        {
            gameManager.PlayerWon();
            yield break;
        }
        uIService.WaveNotification.text = "Wave: " + CurrWave;
        yield return StartCoroutine(uIService.ShowWaveNotification());
        CalculateNumOfEnemyToSpawn();
        for (int i = 0; i < numOfEnemyToSpawn; i++)
        {
            SpawnEnemy();
        }
    }


    private void CalculateNumOfEnemyToSpawn()
    {
        if (CurrWave == 1)
        {
            numOfEnemyToSpawn = WaveSystem.NumOfEneimesInFirstWave;
            return;
        }
        int additionEnemy = Random.Range(WaveSystem.MinNumOfEneimesIncraseEachWave, WaveSystem.MinNumOfEneimesIncraseEachWave + 1);
        numOfEnemyToSpawn += additionEnemy;
    }
    #endregion

    #region EnemyCount Update
    private void IncreaseEnemyInScene(EnemyView enemyView)
    {
        gameManager.EnemyInScene.Add(enemyView);
        uIService.UpdateEnemyCountUI();
    }

    private void DecreaseEnemyInScene(EnemyView enemyView)
    {
        gameManager.EnemyInScene.Remove(enemyView);
        uIService.UpdateEnemyCountUI();

        if (gameManager.EnemyInScene.Count == 0)
            StartCoroutine(SpawnNewWave());

    }



    #endregion

}
