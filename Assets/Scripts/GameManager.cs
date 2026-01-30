using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public Card Card01;
    [HideInInspector]
    public Card Card02;
    [SerializeField]
    private Transform CardsParent;
    private int CardCount;

    void Start()
    {
        CardCount = CardsParent.childCount;
    }
    float CardPickTimer = 5;
    bool timerShouldRun = true;
    void Update()
    {
        if (timerShouldRun)
        {
            //CardPickTimer -= Time.deltaTime;
        }
        if (CardPickTimer <= 0)
        {
            Debug.Log("TOO SLOW");
            CardPickTimer = 5;
            timerShouldRun = false;
            StartCoroutine(GameWaitTimer(2, EnableTimer));
            StartCoroutine(GameWaitTimer(0, DoTimerDamage));

        }
    }
    private void DoTimerDamage()
    {
        GameEventManager.instance.PlayerDamaged(5);
    }
    private void EnableTimer()
    {
        timerShouldRun = true;
    }
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
        timerShouldRun = false;
        CardPickTimer = 5;
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
    private IEnumerator GameWaitTimer(float t, Action a)
    {
        //suspend player input
        yield return new WaitForSeconds(t);
        a();
        //resume player input
    }

}
