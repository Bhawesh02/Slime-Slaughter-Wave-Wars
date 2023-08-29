
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIService : MonoSingletonGeneric<UIService>
{

    private GameManager gameManager ;


    [Header("UI Elements")]
    [Header("Always Visisble")]
    [SerializeField]
    private Slider playerHealthSlider;
    [SerializeField]
    private TextMeshProUGUI enemiesCount;
    [Header("New Wave")]

    public TextMeshProUGUI WaveNotification;
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
    public GameObject SettingUi;
    [Header("Buttons")]
    [SerializeField]

    private Button[] restartButton;
    [SerializeField]
    private Button[] quitButton;
    [SerializeField]
    private Button settingsButton;


    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < restartButton.Length; i++)
            restartButton[i].onClick.AddListener(RestartScene);
        for (int i = 0; i < quitButton.Length; i++)
            quitButton[i].onClick.AddListener(QuitScene);
        settingsButton.onClick.AddListener(ShowSettings);
        gameResumeButton.onClick.AddListener(ResumeGameUI);
        WaveNotification.gameObject.SetActive(false);
        playerDeadUi.SetActive(false);
        playerWonUi.SetActive(false);
        gamePauseUi.SetActive(false);
        

    }
    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    #region Button Functions
    private void ShowSettings()
    {
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
        SettingUi.SetActive(true);
    }

    private void QuitScene()
    {
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
        Application.Quit();
    }

    private void RestartScene()
    {
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region Spawn Wave Notification

    // Using corountine with yield return null to give a fading effect to text
    public IEnumerator ShowWaveNotification()
    {
        WaveNotification.gameObject.SetActive(true);
        yield return StartCoroutine(ChaneAlphaOfWaveNotification(1f));
        yield return StartCoroutine(ChaneAlphaOfWaveNotification(0f));
        WaveNotification.gameObject.SetActive(false);
    }

    private IEnumerator ChaneAlphaOfWaveNotification(float newAlpha)
    {
        float elapsedTime = 0f;
        Color initialColor = WaveNotification.color;
        while (elapsedTime < gameManager.WaveSystem.WaveNotificationFadeDuration)
        {
            float normalizedTime = elapsedTime / gameManager.WaveSystem.WaveNotificationFadeDuration;
            Color newColor = initialColor;
            newColor.a = Mathf.Lerp(initialColor.a, newAlpha, normalizedTime);

            WaveNotification.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Color finalColor = WaveNotification.color;
        finalColor.a = newAlpha;
        WaveNotification.color = finalColor;
    }
    #endregion

    public void UpdateEnemyCountUI()
    {
        enemiesCount.text = ": " + gameManager.EnemyInScene.Count;
    }
    #region Player Health UI
    public IEnumerator SetPlayerMaxHealthInSlider()
    {
        yield return null;
        playerHealthSlider.maxValue = gameManager.Player.Controller.Model.MaxHealth;
    }
    public void SetPlayerHealthInSlider()
    {
        playerHealthSlider.value = gameManager.Player.Controller.Model.CurrentHealth;
    }
    #endregion

    #region Player Won And Dead

    public void PlayerDiedUI()
    {
        wavesCoveredInfo.text = "Waves Covered: " + (gameManager.CurrWave - 1);
        playerDeadUi.SetActive(true);
    }
    public void PlayerWonUI()
    {
       playerWonUi.SetActive(true);
    }

    #endregion

    #region Pause And Resume Game
    public void PauseGameUI()
    {
        gamePauseUi.SetActive(true);
        
        Time.timeScale = 0;

    }
    public void ResumeGameUI()
    {
        gamePauseUi.SetActive(false);

        Time.timeScale = 1f;
    }
    #endregion
}
