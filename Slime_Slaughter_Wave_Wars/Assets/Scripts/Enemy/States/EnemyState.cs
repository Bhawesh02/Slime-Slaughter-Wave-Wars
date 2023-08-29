
using UnityEngine;

[RequireComponent(typeof(EnemyView))]
public class EnemyState : MonoBehaviour
{
    [SerializeField]
    protected EnemyView View { get; private set; }
    protected EnemyController Controller { get; private set; }
    protected EnemyModel Model { get; private set; }
    protected virtual void Awake()
    {
        View = GetComponent<EnemyView>();
    }
   
    protected virtual void Start()
    {
        SetModelController();


    }
    protected void SetModelController()
    {
        Controller = View.Controller;
        Model = Controller.Model;
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

