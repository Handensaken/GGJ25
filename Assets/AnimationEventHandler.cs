using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public void FinishCleaverAttack()
    {
        GameEventManager.instance.ResetCleaver();
    }
    public void RecoverCleaver()
    {
        GameEventManager.instance.RecoveCleaver();
    }
    public void MoveCleaver()
    {
        GameEventManager.instance.CleaverMoveStart();
    }
    public void StartTimer()
    {
        GameEventManager.instance.EnableTimer();
    }
    public void RollHead()
    {
        GameEventManager.instance.RollHead();
    }
    public void GameEndEvent()
    {
        GameEventManager.instance.GameEnd(false);
    }
    public void PlayThwack()
    {
        AudioManager.Instance.Play("CleaverThwack");
    }
    public void PlayFingie()
    {
        AudioManager.Instance.Play("CuttingFingers");
    }
    public void Shake(int i)
    {
        bool b = false;
        if(i == 1)
        {
            b = true;
        }
        GameEventManager.instance.ScreenShake(b);
    }
}
