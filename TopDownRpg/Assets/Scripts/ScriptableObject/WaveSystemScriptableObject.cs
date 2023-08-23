
using UnityEngine;

[CreateAssetMenu(fileName ="NewWaveSystem",menuName ="ScriptableObject/NewWave")]
public class WaveSystemScriptableObject : ScriptableObject
{
    public int NumOfWaves;
    public int NumOfEneimesInFirstWave;
    public int MaxNumOfEneimesIncraseEachWave;
    public int MinNumOfEneimesIncraseEachWave;
    public float WaveNotificationFadeDuration;
}
