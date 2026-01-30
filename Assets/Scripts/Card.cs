using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    public int cardID;

    private bool selected;

    private Vector3 defaultPosition;
    void Start()
    {
        defaultPosition = transform.position;
    }
    public void UpdateSelection(bool b)
    {
        selected = b;

        if (selected)
        {
            transform.position = defaultPosition + new Vector3(0, 0.5f, 0);
        }
        else
        {
            transform.position = defaultPosition;
        }
    }
}
