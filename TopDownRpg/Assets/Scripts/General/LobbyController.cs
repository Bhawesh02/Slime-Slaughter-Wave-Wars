
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(playGame);
        quitButton.onClick.AddListener(quitGame);
    }

    private void quitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void playGame()
    {
        SceneManager.LoadScene(1);
    }
}
