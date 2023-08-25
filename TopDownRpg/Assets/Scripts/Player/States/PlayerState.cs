
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerView))]
public class PlayerState : MonoBehaviour
{
    protected PlayerView playerView;
    protected PlayerController playerController;


    private void Awake()
    {
        playerView = GetComponent<PlayerView>();
    }
    private void Start()
    {
        playerController = playerView.PlayerController;
    }



    public virtual void OnStateEnter()
    {
        this.enabled = true;
    }
    public virtual void OnStateExit()
    {
        this.enabled = false;
    }
}

public enum LookDirection
{
    Null,
    Down,
    Up,
    Left,
    Right
}

public enum PlayerAnimationStates
{
    Null,
    Down_Idel,
    Down_Running,
    Down_Fight,
    Up_Idel,
    Up_Running,
    Up_Fight,
    Horizontal_Idel,
    Horizontal_Running,
    Horizontal_Fight,
    Took_Damage,
    Dead
}