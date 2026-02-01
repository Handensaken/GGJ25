using UnityEngine;
using UnityEngine.AI;

public class CameraBehaviour : MonoBehaviour
{
    Animator a;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        a = GetComponent<Animator>();
        GameEventManager.instance.OnRollHead += Roll;
        GameEventManager.instance.OnScreenShake += Fuck;

    }
    void OnDisable()
    {
        GameEventManager.instance.OnRollHead -= Roll;
        GameEventManager.instance.OnScreenShake -= Fuck;

    }
    void Roll()
    {
        a.SetTrigger("Roll");
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void Fuck(bool large)
    {
        if (large)
        {
            a.SetTrigger("ShakeLarge");
        }
        else
        {
            a.SetTrigger("ShakeSmall");

        }
    }
}
