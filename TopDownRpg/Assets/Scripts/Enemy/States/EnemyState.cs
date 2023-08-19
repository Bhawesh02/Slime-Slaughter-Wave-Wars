
using UnityEngine;

[RequireComponent(typeof(EnemyView))]
public class EnemyState : MonoBehaviour
{
    [SerializeField]
    protected EnemyView View { get; private set; }
    protected EnemyController Controller { get; private set; }
    protected EnemyModel Model { get; private set; }
    private void Start()
    {
        View = GetComponent<EnemyView>();
        Controller = View.Controller;
        Model = View.Model;
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

