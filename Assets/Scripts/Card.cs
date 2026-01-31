using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class Card : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField]

    private int _cardID;
    public int cardID
    {
        get => _cardID;
        set
        {
            _cardID = value;
            image.color = colors[value];
            //  SetColor();
        }
    }

    Color[] colors =
    {
        Color.red,
        Color.magenta,
        Color.black,
        Color.yellow,
        Color.blue,
        Color.green,
        Color.cyan,
        Color.grey
    };
    private bool selected;

    private Vector3 defaultPosition;
    void Start()
    {
        defaultPosition = transform.position;
    }
    void SetColor()
    {
        image.color = colors[cardID];

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
