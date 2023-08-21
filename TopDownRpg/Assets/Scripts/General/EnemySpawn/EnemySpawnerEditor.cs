using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        EnemySpawner enemySpawner = EnemySpawner.Instance;
        if(GUILayout.Button("Spawn Enemy"))
        {
            if (!Application.isPlaying)
            {
                return;
            }
            enemySpawner.SpawnEnemy();
        }
       
    }
}


