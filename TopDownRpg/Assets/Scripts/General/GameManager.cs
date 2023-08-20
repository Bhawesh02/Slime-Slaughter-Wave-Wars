
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingletonGeneric<GameManager>
{
    public PlayerView Player;
    [SerializeField]
    private Slider playerHealthSlider;

    private void Start()
    {
        if (Player != null)
            setPlayerMaxHealthInSlider();
    }
    private void setPlayerMaxHealthInSlider()
    {
        playerHealthSlider.maxValue = Player.PlayerModel.MaxHealth;
    }
    public void SetPlayerHealthInSlider()
    {
        playerHealthSlider.value = Player.PlayerModel.CurrentHealth;
    }
}
