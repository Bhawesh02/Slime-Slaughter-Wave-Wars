
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
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
        settingsButton.onClick.AddListener(ShowSettings);
    }

    private void ShowSettings()
    {
        settings.SetActive(true);
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
    }

    private void QuitGame()
    {
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
        Application.Quit();
    }

    private void PlayGame()
    {
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
        SceneManager.LoadScene(1);
    }
}
