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

    public event Action<string, float> OnSceneTransition;
    public void SceneTransition(string s, float delay)
    {
        if (OnSceneTransition != null)
        {
            OnSceneTransition(s, delay);
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
    public event Action<Transform> OnCleaverHeadChop;
    public void CleaverHeadChop(Transform t)
    {
        if (OnCleaverHeadChop != null)
        {
            OnCleaverHeadChop(t);
        }
    }
    public event Action OnEnableTimer;
    public void EnableTimer()
    {
        if (OnEnableTimer != null)
        {
            OnEnableTimer();
        }
    }

    public event Action OnRecoverCleaver;
    public void RecoveCleaver()
    {
        if (OnRecoverCleaver != null)
        {
            OnRecoverCleaver();
        }
    }
    public event Action OnResetCleaver;
    public void ResetCleaver()
    {
        if (OnResetCleaver != null)
        {
            OnResetCleaver();
        }
    }
    public event Action OnCleaverMoveStart;
    public void CleaverMoveStart()
    {
        if (OnCleaverMoveStart!= null)
        {
            OnCleaverMoveStart();
        }
    }
    public event Action OnRollHead;
    public void RollHead()
    {
        if (OnRollHead!= null)
        {
            OnRollHead();
        }
    }
  
}
