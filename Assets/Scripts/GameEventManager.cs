using UnityEngine;
using System;
public class GameEventManager : MonoBehaviour
{
    static public GameEventManager instance { get; private set; }
    void Awake()
    {
        instance = this;
    }

    public event Action<int> OnPlayerDamaged;
    public void PlayerDamaged(int i)
    {
        if (OnPlayerDamaged != null)
        {
            OnPlayerDamaged(i);
        }
    }
}
