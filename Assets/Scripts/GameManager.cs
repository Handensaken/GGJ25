using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //====================================================
    //CARDS
    [HideInInspector]
    public Card Card01;
    [HideInInspector]
    public Card Card02;
    [SerializeField]
    private Transform CardsParent;
    private int CardCount;
    //====================================================
    //Timer Stuff
    [SerializeField]
    [Header("Timer")]
    [Tooltip("Set the standard timer value for picking two cards. Changes what number to count down from in seconds")]
    private float CardTimerValue;
    float CardPickTimer;
    bool timerShouldRun = true;
    [SerializeField]
    private Image timerImage;
    //====================================================

    void Start()
    {
        CardCount = CardsParent.childCount;
        CardPickTimer = CardTimerValue;
        GameEventManager.instance.OnGameEnd += GameEnd;
    }
    void OnDisable()
    {
        GameEventManager.instance.OnGameEnd -= GameEnd;
    }

    void Update()
    {
        if (timerShouldRun)
        {
            CardPickTimer -= Time.deltaTime;
            SetTimerFill();
            if (CardPickTimer <= 0)
            {
                Debug.Log("TOO SLOW");
                GameEventManager.instance.SetPlayerInputState(false);
                timerShouldRun = false;
                StartCoroutine(RefillTimer(1, 100));
                StartCoroutine(GameWaitTimer(2, EnableTimer));
                StartCoroutine(GameWaitTimer(0, DoTimerDamage));
            }
        }
    }

    //TIMER
    private void SetTimerFill()
    {
        float f = CardPickTimer / CardTimerValue;
        timerImage.fillAmount = f;
    }
    private void DoTimerDamage()
    {
        GameEventManager.instance.PlayerDamaged(5);
    }
    private void EnableTimer()
    {
        timerShouldRun = true;
        ResumePlayerInput();
    }
    IEnumerator RefillTimer(float timeInSeconds, float timeStep)
    {
        for (float i = 0; i <= timeInSeconds * timeStep; i++)
        {
            float t = i / (timeInSeconds * timeStep);
            timerImage.fillAmount = Mathf.Lerp(CardPickTimer / CardTimerValue, 1, t);
            yield return new WaitForSeconds(1 / timeStep);
        }
        CardPickTimer = CardTimerValue;
    }

    //CARDS
    public void SubmitCard(Card submittedCard)
    {
        if (Card01 == null)
        {
            Card01 = submittedCard;
        }
        else
        {
            Card02 = submittedCard;
            CompareCards();
        }
    }
    public void CompareCards()
    {
        GameEventManager.instance.SetPlayerInputState(false);

        timerShouldRun = false;

        StartCoroutine(RefillTimer(1, 100));
        StartCoroutine(GameWaitTimer(2, EnableTimer));

        if (Card01.cardID == Card02.cardID)
        {
            StartCoroutine(GameWaitTimer(2, RemoveSelection));

        }
        else
        {
            StartCoroutine(GameWaitTimer(2, DisableSelection));
        }
    }
    private void RemoveSelection()
    {
        Card01.gameObject.SetActive(false);
        Card02.gameObject.SetActive(false);
        Card01 = null;
        Card02 = null;
        CardCount -= 2;
        if (CardCount == 0)
        {
            GameEventManager.instance.GameEnd(true);
            Debug.Log("Win Game");
        }
    }
    private void DisableSelection()
    {
        GameEventManager.instance.PlayerDamaged(1);
        Card01.UpdateSelection(false);
        Card02.UpdateSelection(false);
        Card01 = null;
        Card02 = null;
    }

    //GAME WAIT TIME
    private void GameEnd(bool won)
    {
        timerShouldRun = false;
        string s = won ? "Win" : "Lose";
        StopAllCoroutines();
        //Do things that happen on game end, then transition
        StartCoroutine(GameEndTimer(5, SceneEnd, s));
    }
    void SceneEnd(string s)
    {
        GameEventManager.instance.SceneTransition(s);
    }
    private IEnumerator GameEndTimer(float t, Action<string> a, string s)
    {
        //suspend player input
        yield return new WaitForSeconds(t);
        a(s);

        //resume player input
    }
    private IEnumerator GameWaitTimer(float t, Action a)
    {
        //suspend player input
        yield return new WaitForSeconds(t);
        a();
        //resume player input
    }
    private void ResumePlayerInput()
    {
        GameEventManager.instance.SetPlayerInputState(true);

    }

}
