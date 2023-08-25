
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoSingletonGeneric<GameManager>
{
    [SerializeField]
    private WaveSystemScriptableObject waveSystem;
    [SerializeField]
    private List<EnemyView> enemyInScene;
    [HideInInspector]
    public PlayerView Player;
    [Header("UI Elements")]
    [Header("Always Visisble")]
    [SerializeField]
    private Slider playerHealthSlider;
    [SerializeField]
    private TextMeshProUGUI enemiesCount;
    [Header("New Wave")]

    [SerializeField]
    private TextMeshProUGUI waveNotification;
    [Header("Player Dead")]
    [SerializeField]
    private GameObject playerDeadUi;
    [SerializeField]
    private TextMeshProUGUI wavesCoveredInfo;

    [Header("Player Won")]
    [SerializeField]
    private GameObject playerWonUi;
    [Header("Game Pause")]
    [SerializeField]
    private GameObject gamePauseUi;
    [SerializeField]
    private Button gameResumeButton;

    [Header("Settings")]
    [SerializeField]
    private GameObject settingUi;
    [Header("Buttons")]
    [SerializeField]

    private Button[] restartButton;
    [SerializeField]
    private Button[] quitButton;
    [SerializeField]
    private Button settingsButton;


    private int currWave;

    private int numOfEnemyToSpawn;


    private Coroutine spawnWave;

    private bool isPaused;

    private InputMaster inputs;

    protected override void Awake()
    {
        base.Awake();
        currWave = 0;
        for (int i = 0; i < restartButton.Length; i++)
            restartButton[i].onClick.AddListener(restartScene);
        for (int i = 0; i < quitButton.Length; i++)
            quitButton[i].onClick.AddListener(quitScene);
        settingsButton.onClick.AddListener(showSettings);
        gameResumeButton.onClick.AddListener(ResumeGame);
        waveNotification.gameObject.SetActive(false);
        playerDeadUi.SetActive(false);
        playerWonUi.SetActive(false);
        gamePauseUi.SetActive(false);
        isPaused = false;
        inputs = new();

    }

    private void Start()
    {

        StartCoroutine(setPlayerMaxHealthInSlider());
        EventService.Instance.EnemySpawned += increaseEnemyInScene;
        EventService.Instance.EnemyDied += decreaseEnemyInScene;
        spawnWave = StartCoroutine(spawnNewWave());
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Menu.Pause.performed += _ =>
        OnPausePress();

    }

    
    #region Button Functions
    private void showSettings()
    {
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);

        settingUi.SetActive(true);
    }

    private void quitScene()
    {
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit()
#endif
    }

    private void restartScene()
    {
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion


    #region Spwan Wave
    private IEnumerator spawnNewWave()
    {
        currWave++;
        if (currWave > waveSystem.NumOfWaves)
        {
            PlayerWon();
            yield break;
        }
        waveNotification.text = "Wave: " + currWave;
        yield return StartCoroutine(showWaveNotification());
        calculateNumOfEnemyToSpawn();
        for (int i = 0; i < numOfEnemyToSpawn; i++)
        {
            EnemySpawner.Instance.SpawnEnemy();
        }
    }
    private IEnumerator showWaveNotification()
    {
        waveNotification.gameObject.SetActive(true);
        yield return StartCoroutine(chaneAlphaOfWaveNotification(1f));
        yield return StartCoroutine(chaneAlphaOfWaveNotification(0f));
        waveNotification.gameObject.SetActive(false);
    }

    private IEnumerator chaneAlphaOfWaveNotification(float newAlpha)
    {
        float elapsedTime = 0f;
        Color initialColor = waveNotification.color;
        while (elapsedTime < waveSystem.WaveNotificationFadeDuration)
        {
            float normalizedTime = elapsedTime / waveSystem.WaveNotificationFadeDuration;
            Color newColor = initialColor;
            newColor.a = Mathf.Lerp(initialColor.a, newAlpha, normalizedTime);

            waveNotification.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Color finalColor = waveNotification.color;
        finalColor.a = newAlpha;
        waveNotification.color = finalColor;
    }

    private void calculateNumOfEnemyToSpawn()
    {
        if (currWave == 1)
        {
            numOfEnemyToSpawn = waveSystem.NumOfEneimesInFirstWave;
            return;
        }
        int additionEnemy = Random.Range(waveSystem.MinNumOfEneimesIncraseEachWave, waveSystem.MinNumOfEneimesIncraseEachWave + 1);
        numOfEnemyToSpawn += additionEnemy;
    }
    #endregion

    #region EnemyCount Update
    private void increaseEnemyInScene(EnemyView enemyView)
    {
        enemyInScene.Add(enemyView);
        updateEnemyCountUI();
    }

    private void decreaseEnemyInScene(EnemyView enemyView)
    {
        enemyInScene.Remove(enemyView);
        updateEnemyCountUI();

        if (enemyInScene.Count == 0)
            spawnWave = StartCoroutine(spawnNewWave());

    }
    private void updateEnemyCountUI()
    {
        enemiesCount.text = ": " + enemyInScene.Count;
    }


    #endregion

    #region Player Health UI
    private IEnumerator setPlayerMaxHealthInSlider()
    {
        yield return null;
        playerHealthSlider.maxValue = Player.PlayerModel.MaxHealth;
    }
    public void SetPlayerHealthInSlider()
    {
        playerHealthSlider.value = Player.PlayerModel.CurrentHealth;
    }
    #endregion

    #region Player Won And dead
    public void PlayedDied()
    {
        setAllEnemyIdel();
        wavesCoveredInfo.text = "Waves Covered: " + (currWave - 1);
        playerDeadUi.SetActive(true);
    }

    public void PlayerWon()
    {
        setAllEnemyIdel();
        playerWonUi.SetActive(true);
    }
    private void setAllEnemyIdel()
    {
        for (int i = 0; i < enemyInScene.Count; i++)
        {
            enemyInScene[i].ChangeState(enemyInScene[i].IdelState);
        }
    }
    #endregion


    #region Pause & Resume Game

    private void OnPausePress()
    {
        if (!isPaused)
            PauseGame();
        else if (settingUi.activeSelf)
            settingUi.SetActive(false);
        else
            ResumeGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        gamePauseUi.SetActive(true);
        Player.enabled = false;
        Time.timeScale = 0;

    }
    private void ResumeGame()
    {
        isPaused = false;
        gamePauseUi.SetActive(false);

        Player.enabled = true;
        Time.timeScale = 1f;
    }
    #endregion

    private void OnDisable()
    {
        inputs.Disable();
        inputs.Menu.Pause.performed -= _ =>
        OnPausePress();

    }
}
