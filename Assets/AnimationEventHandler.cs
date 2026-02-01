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
}
