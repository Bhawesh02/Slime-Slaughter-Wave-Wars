
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private GameObject settings;


    private void Awake()
    {
        settings.SetActive(false);
        playButton.onClick.AddListener(playGame);
        quitButton.onClick.AddListener(quitGame);
        settingsButton.onClick.AddListener(showSettings);
    }

    private void showSettings()
    {
        settings.SetActive(true);
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
    }

    private void quitGame()
    {
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
        Application.Quit();
    }

    private void playGame()
    {
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
        SceneManager.LoadScene(1);
    }
}
