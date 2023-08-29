
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
        playerController = playerView.Controller;
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



