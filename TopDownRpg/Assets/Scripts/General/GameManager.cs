
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingletonGeneric<GameManager>
{
    public PlayerView Player;
    [SerializeField]
    private Slider playerHealthSlider;
    [SerializeField]
    private WaveSystemScriptableObject waveSystem;



    private int numOfEnemyInScene;

    private int currWave;

    private int numOfEnemyToSpawn;
    
    protected override void Awake()
    {
        base.Awake();
        numOfEnemyInScene = 0;
        currWave = 0;
    }
    private void Start()
    {
        
        StartCoroutine(setPlayerMaxHealthInSlider());
        EventService.Instance.EnemySpawned += increaseEnemyInScene;
        EventService.Instance.EnemyDied += decreaseEnemyInScene;
        spawnWave();
    }

    private void spawnWave()
    {
        currWave++;
        if(currWave > waveSystem.NumOfWaves)
        {
            Debug.Log("Player Won");
            return;
        }
        calculateNumOfEnemyToSpawn();
        for(int i = 0;i<numOfEnemyToSpawn;i++)
        {
            EnemySpawner.Instance.SpawnEnemy();
        }
    }

    private void calculateNumOfEnemyToSpawn()
    {
        if (currWave == 1)
        {
            numOfEnemyToSpawn = waveSystem.NumOfEneimesInFirstWave;
            return;
        }
        int additionEnemy = Random.Range(waveSystem.MinNumOfEneimesIncraseEachWave,waveSystem.MinNumOfEneimesIncraseEachWave+1);
        numOfEnemyToSpawn+= additionEnemy;
    }

    private void increaseEnemyInScene()
    {
        numOfEnemyInScene++;
    }

    private void decreaseEnemyInScene()
    {
        numOfEnemyInScene--;
        if (numOfEnemyInScene == 0)
            spawnWave();
    }
    private IEnumerator setPlayerMaxHealthInSlider()
    {
        yield return null;
        playerHealthSlider.maxValue = Player.PlayerModel.MaxHealth;
    }
    public void SetPlayerHealthInSlider()
    {
        playerHealthSlider.value = Player.PlayerModel.CurrentHealth;
    }
}
