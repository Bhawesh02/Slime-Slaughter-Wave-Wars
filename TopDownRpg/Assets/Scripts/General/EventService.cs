
using System;

public class EventService : SingletonGeneric<EventService> 
{
    public event Action EnemySpawned;
    public event Action EnemyDied;

    public void InvokeEnemySpawned()
    {
        EnemySpawned?.Invoke();
    }
    public void InvokeEnemyDied()
    {
        EnemyDied?.Invoke();
    }
}
