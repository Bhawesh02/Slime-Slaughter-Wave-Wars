
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingletonGeneric<GameManager>
{
    public List<EnemyView> EnemyInScene  { get; private set; }
    [HideInInspector]
    public PlayerView Player;
    


    




    private InputMaster inputs;

    private UIService uIService;
    private bool isPaused;


    protected override void Awake()
    {
        base.Awake();
        
        inputs = new();
        isPaused = false;
        EnemyInScene = new();
    }

    private void Start()
    {
        uIService = UIService.Instance;
        StartCoroutine(uIService.SetPlayerMaxHealthInSlider());
        
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Menu.Pause.performed += _ =>
        OnPausePress();

    }

    
    


    

   

    #region Player Won And dead
    public void PlayedDied()
    {
        SetAllEnemyIdel();
        uIService.PlayerDiedUI();
    }

    public void PlayerWon()
    {
        SetAllEnemyIdel();
        uIService.PlayerWonUI();
    }
    private void SetAllEnemyIdel()
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
