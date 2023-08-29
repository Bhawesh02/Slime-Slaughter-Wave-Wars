
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingletonGeneric<GameManager>
{
    public WaveSystemScriptableObject WaveSystem;
    public List<EnemyView> EnemyInScene  { get; private set; }
    [HideInInspector]
    public PlayerView Player;
    


    public int CurrWave { get; private set; }

    private int numOfEnemyToSpawn;


    private Coroutine spawnWave;


    private InputMaster inputs;

    private UIService uIService;
    private bool isPaused;


    protected override void Awake()
    {
        base.Awake();
        CurrWave = 0;
        
        inputs = new();
        isPaused = false;
        EnemyInScene = new();
    }

    private void Start()
    {
        uIService = UIService.Instance;
        StartCoroutine(uIService.SetPlayerMaxHealthInSlider());
        EventService.Instance.EnemySpawned += IncreaseEnemyInScene;
        EventService.Instance.EnemyDied += DecreaseEnemyInScene;
        spawnWave = StartCoroutine(spawnNewWave());
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Menu.Pause.performed += _ =>
        OnPausePress();

    }

    
    


    #region Spwan Wave
    private IEnumerator spawnNewWave()
    {
        CurrWave++;
        if (CurrWave > WaveSystem.NumOfWaves)
        {
            PlayerWon();
            yield break;
        }
        uIService.WaveNotification.text = "Wave: " + CurrWave;
        yield return StartCoroutine(uIService.ShowWaveNotification());
        CalculateNumOfEnemyToSpawn();
        for (int i = 0; i < numOfEnemyToSpawn; i++)
        {
            EnemySpawner.Instance.SpawnEnemy();
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
        EnemyInScene.Add(enemyView);
        uIService.UpdateEnemyCountUI();
    }

    private void DecreaseEnemyInScene(EnemyView enemyView)
    {
        EnemyInScene.Remove(enemyView);
        uIService.UpdateEnemyCountUI();

        if (EnemyInScene.Count == 0)
            spawnWave = StartCoroutine(spawnNewWave());

    }
    


    #endregion

   

    #region Player Won And dead
    public void PlayedDied()
    {
        setAllEnemyIdel();
        uIService.PlayerDiedUI();
    }

    public void PlayerWon()
    {
        setAllEnemyIdel();
        uIService.PlayerWonUI();
    }
    private void setAllEnemyIdel()
    {
        for (int i = 0; i < EnemyInScene.Count; i++)
        {
            EnemyInScene[i].Controller.ChangeState(EnemyInScene[i].IdelState);
        }
    }
    #endregion


    #region Pause & Resume Game

    private void OnPausePress()
    {
        if (!isPaused)
        {
            isPaused = true;
            Player.enabled = false;
            uIService.PauseGameUI();
        }
        else if (uIService.SettingUi.activeSelf)
            uIService.SettingUi.SetActive(false);
        else
        {
            isPaused = false;
            Player.enabled = true;
            uIService.ResumeGameUI();
        }
    }

    
    #endregion

    private void OnDisable()
    {
        inputs.Disable();
        inputs.Menu.Pause.performed -= _ =>
        OnPausePress();

    }
}
