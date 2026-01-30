using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 10;
        Cursor.lockState = CursorLockMode.Locked;
        GameEventManager.instance.OnPlayerDamaged += PlayerDamage;
    }
    void OnDisable()
    {
        GameEventManager.instance.OnPlayerDamaged -= PlayerDamage;
    }
    private Vector2 mousePos;
    [SerializeField]
    private LayerMask cardLayerMask;
    private Transform lastCardHovered;
    // Update is called once per frame
    public Transform pointer;
    void Update()
    {
        mousePos = pointer.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, cardLayerMask))
        {
            if (hit.transform != lastCardHovered)
            {
                lastCardHovered = hit.transform;

            }
        }
        else
        {
            lastCardHovered = null;
            //  Debug.Log("No hit");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && lastCardHovered != null)
        {
            if (gameManager.Card01 != null)
            {
                if (gameManager.Card01.transform == lastCardHovered) return;
            }

            Card c = lastCardHovered.gameObject.GetComponent<Card>();
            c.UpdateSelection(true);
            gameManager.SubmitCard(c);
            Debug.Log(lastCardHovered.name);

        }

    }
    private void PlayerDamage(int damage)
    {
        health -= damage;
        mouseSensitivity = (float)health / 10;
        if (health == 0) mouseSensitivity = 0.025f;
        if (health < 0) Debug.Log("PlayerDead");
    }
    float mouseSensitivity = 1f;
    float baseMouseSensitivityScale = 0.4f;
    public void MouseInput(InputAction.CallbackContext ctx)
    {
        Vector2 mouseDelta = ctx.ReadValue<Vector2>();
        pointer.transform.Translate(mouseDelta * mouseSensitivity * baseMouseSensitivityScale);
    }
}
