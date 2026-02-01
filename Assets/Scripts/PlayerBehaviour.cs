using System.Threading;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;


public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField]
    InputActionAsset inputActions;
    InputActionMap playerMap;
    private int health;
    [SerializeField]
    private Arm leftArm;
    [SerializeField]
    private Arm rightArm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        playerMap = inputActions.FindActionMap("Player");
        health = 12;
        Cursor.lockState = CursorLockMode.Locked;
        GameEventManager.instance.OnPlayerDamaged += PlayerDamage;
        GameEventManager.instance.OnSetPlayerInputState += SetPlayerInput;
        SetPlayerInput(false);
        playerMap.Enable();
    }
    void OnDisable()
    {
        GameEventManager.instance.OnPlayerDamaged -= PlayerDamage;
        GameEventManager.instance.OnSetPlayerInputState -= SetPlayerInput;
    }
    private Vector2 mousePos;
    [SerializeField]
    private LayerMask cardLayerMask;
    private Transform cardHovered;
    // Update is called once per frame
    public Transform pointer;

    [SerializeField] Transform headChopPos;
    void Update()
    {
        mousePos = pointer.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, cardLayerMask))
        {
            if (hit.transform != cardHovered)
            {
                cardHovered = hit.transform;
            }
        }
        else
        {
            cardHovered = null;
        }
    }
    private void PlayerDamage(int damage)
    {
        Arm armChoice = null;
        if (damage == 1)
        {
            if (!leftArm.dead)
            {
                armChoice = leftArm;
                //Cleaver chop event
                //Arm -> location
            }
            else if (!rightArm.dead)
            {
                armChoice = rightArm;
            }
            health -= damage;
        }
        if (damage == 5)
        {
            if (!leftArm.dead && !rightArm.dead)
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    armChoice = rightArm;
                    //right
                }
                else
                {
                    armChoice = leftArm;
                    //left
                }
                //randomize
            }
            else if (!leftArm.dead && rightArm.dead)
            {
                armChoice = leftArm;
                //Left goes
            }
            else if (leftArm.dead && !rightArm.dead)
            {
                armChoice = rightArm;
                //right goes
            }
            else
            {
                //This should be replaced with a special position
                GameEventManager.instance.CleaverHeadChop(headChopPos);
                Debug.Log("PlayerDead");
                playerMap.Disable();
                //GameEventManager.instance.GameEnd(false);
                return;
            }
            health -= armChoice.armHealth;
            armChoice.armHealth = 0;
            //  armChoice.dead = true;

            //  GameEventManager.instance.CleaverChopCalled(armChoice);
        }
        if (armChoice != null)
        {
            StartCoroutine(gameManager.GameWaitTimerArm(0.75f, GameEventManager.instance.CleaverChopCalled, armChoice));
            if (armChoice.armHealth <= 0)
            {
                armChoice.armHealth = 0;
                armChoice.dead = true;
            }
        }
        mouseSensitivity = (float)health / 12;
        if (health == 0) mouseSensitivity = 0.025f;
        if (health < 0)
        {
            GameEventManager.instance.CleaverHeadChop(headChopPos);
            //GameEventManager.instance.GameEnd(false);
            Debug.Log("PlayerDead");
            playerMap.Disable();
        }
    }
    float mouseSensitivity = 1f;
    float baseMouseSensitivityScale = 0.4f;
    public void MouseInput(InputAction.CallbackContext ctx)
    {
        Vector2 mouseDelta = ctx.ReadValue<Vector2>();
        pointer.transform.Translate(mouseDelta * mouseSensitivity * baseMouseSensitivityScale);
    }
    public void InteractInput(InputAction.CallbackContext ctx)
    {
        if (cardHovered != null && ctx.started)
        {
            if (gameManager.Card01 != null)
            {
                if (gameManager.Card01.transform == cardHovered) return;
            }

            Card c = cardHovered.gameObject.GetComponent<Card>();
            c.UpdateSelection(true);
            gameManager.SubmitCard(c);
            //Debug.Log(cardHovered.name);
        }
    }

    private void SetPlayerInput(bool activeState)
    {
        if (activeState)
        {
            playerMap.FindAction("Interact").Enable();
            //  playerMap.Enable();
        }
        else
        {
            playerMap.FindAction("Interact").Disable();
            //playerMap.Disable();
        }
    }

}
