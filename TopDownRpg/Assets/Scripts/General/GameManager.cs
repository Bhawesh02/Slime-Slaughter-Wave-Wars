
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    [Header("Buttons")]
    [SerializeField]

    private Button[] restartButton;
    [SerializeField]
    private Button[] quitButton;
    private int currWave;

    private int numOfEnemyToSpawn;


    private Coroutine spawnWave;

    protected override void Awake()
    {
        base.Awake();
        currWave = 0;
        for (int i = 0; i < restartButton.Length; i++)
            restartButton[i].onClick.AddListener(restartScene);
        for (int i = 0; i < quitButton.Length; i++)
            quitButton[i].onClick.AddListener(quitScene);
        waveNotification.gameObject.SetActive(false);
        playerDeadUi.SetActive(false);
        playerWonUi.SetActive(false);
    }

    private void quitScene()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit()
#endif
    }

    private void restartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Start()
    {

        StartCoroutine(setPlayerMaxHealthInSlider());
        EventService.Instance.EnemySpawned += increaseEnemyInScene;
        EventService.Instance.EnemyDied += decreaseEnemyInScene;
        spawnWave = StartCoroutine(spawnNewWave());
    }

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
    private IEnumerator setPlayerMaxHealthInSlider()
    {
        yield return null;
        playerHealthSlider.maxValue = Player.PlayerModel.MaxHealth;
    }
    public void SetPlayerHealthInSlider()
    {
        playerHealthSlider.value = Player.PlayerModel.CurrentHealth;
    }

    private void updateEnemyCountUI()
    {
        enemiesCount.text = ": " + enemyInScene.Count;
    }

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
}
