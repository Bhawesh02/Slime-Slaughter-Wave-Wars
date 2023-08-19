using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingletonGeneric<T> : MonoBehaviour where T : MonoSingletonGeneric<T>
{

    public static T Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
