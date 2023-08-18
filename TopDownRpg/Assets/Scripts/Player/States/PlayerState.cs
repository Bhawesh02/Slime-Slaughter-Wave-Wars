
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
    public virtual void Update()
    {
        playerView.HorizontalInput = Input.GetAxisRaw("Horizontal");
        playerView.VerticalInput = Input.GetAxisRaw("Vertical");
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
