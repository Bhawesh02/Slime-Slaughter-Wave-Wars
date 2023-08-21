
using System;

public class EventService : SingletonGeneric<EventService> 
{
    public event Action<EnemyView> EnemySpawned;
    public event Action<EnemyView> EnemyDied;

    public void InvokeEnemySpawned(EnemyView enemyView)
    {
        EnemySpawned?.Invoke(enemyView);
    }
    public void InvokeEnemyDied(EnemyView enemyView)
    {
        EnemyDied?.Invoke(enemyView);
    }
}
