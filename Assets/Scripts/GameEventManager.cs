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

    public event Action<bool> OnGameEnd;
    public void GameEnd(bool won)
    {
        if (OnGameEnd != null)
        {
            OnGameEnd(won);
        }
    }

    public event Action<string> OnSceneTransition;
    public void SceneTransition(string s)
    {
        if (OnSceneTransition != null)
        {
            OnSceneTransition(s);
        }
    }
    public event Action<bool> OnSetPlayerInputState;
    public void SetPlayerInputState(bool state)
    {
        if (OnSetPlayerInputState != null)
        {
            OnSetPlayerInputState(state);
        }
    }
    public event Action<Arm> OnCleaverChopCalled;
    public void CleaverChopCalled(Arm a)
    {
        if (OnCleaverChopCalled != null)
        {
            OnCleaverChopCalled(a);
        }
    }
}
