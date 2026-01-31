using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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

    public Vector3 defaultPosition;
    void Start()
    {
    }
    void SetColor()
    {
        image.color = colors[cardID];

    }
    [SerializeField] float yOffset = 0.1f;
    public void UpdateSelection(bool b)
    {
        GetComponent<Animator>().SetTrigger("Flip");
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            GetComponent<Animator>().SetBool("Rev", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("Rev", false);
        }
        selected = b;

        if (selected)
        {
            StartCoroutine(MoveLerp(0.25f, 100, defaultPosition, defaultPosition + new Vector3(0, yOffset, 0), 0));
            // transform.position = defaultPosition + new Vector3(0, 0.5f, 0);
        }
        else
        {
            StartCoroutine(MoveLerp(0.25f, 100, defaultPosition + new Vector3(0, yOffset, 0), defaultPosition, 0.5f));
            // transform.position = defaultPosition;
        }
    }

    IEnumerator MoveLerp(float timeInSeconds, float timeStep, Vector3 start, Vector3 end, float delay)
    {
        //yield return new WaitForSeconds(delay);
        for (float i = 0; i <= timeInSeconds * timeStep; i++)
        {
            float t = i / (timeInSeconds * timeStep);
            transform.position = Vector3.Lerp(start, end, t);
            yield return new WaitForSeconds(1 / timeStep);
        }
    }
}
