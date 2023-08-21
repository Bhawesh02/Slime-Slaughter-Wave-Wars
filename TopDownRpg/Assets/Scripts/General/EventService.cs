
using System;

public class EventService : SingletonGeneric<EventService> 
{
    public event Action EnemySpawned;

    public void InvokeEnemySpawned()
    {
        EnemySpawned?.Invoke();
    }
}
