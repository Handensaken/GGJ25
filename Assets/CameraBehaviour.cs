using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    Animator a;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        a = GetComponent<Animator>();
        GameEventManager.instance.OnRollHead += Roll;
    }
    void OnDisable()
    {
        GameEventManager.instance.OnRollHead -= Roll;

    }
    void Roll()
    {
        a.SetTrigger("Roll");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
