using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEventManager.instance.OnSceneTransition += TransitionScene;
    }
    void OnDisable()
    {
        GameEventManager.instance.OnSceneTransition -= TransitionScene;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void TransitionScene(string s)
    {
        StartCoroutine(LoadDelay(s));
    }
    IEnumerator LoadDelay(string s)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(s);
    }
}
