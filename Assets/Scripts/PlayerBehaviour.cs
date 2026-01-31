using System.Threading;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMap = inputActions.FindActionMap("Player");
        health = 10;
        Cursor.lockState = CursorLockMode.Locked;
        GameEventManager.instance.OnPlayerDamaged += PlayerDamage;
        GameEventManager.instance.OnSetPlayerInputState += SetPlayerInput;
        SetPlayerInput(false);
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
        health -= damage;
        mouseSensitivity = (float)health / 10;
        if (health == 0) mouseSensitivity = 0.025f;
        if (health < 0)
        {
            GameEventManager.instance.GameEnd(false);
            //Debug.Log("PlayerDead");
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
