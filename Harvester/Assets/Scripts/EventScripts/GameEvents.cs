using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of the Game Events!");
            return;
        }

        Instance = this;
    }

    public event Action<int> onHazardDamage;
    public void HazardDamage(int id)
    {
        onHazardDamage?.Invoke(id);
    }

    public event Action onFadeInOut;
    public void FadeInOut()
    {
        onFadeInOut?.Invoke();
    }
}
