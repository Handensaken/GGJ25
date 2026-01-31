using System;
using Unity.Mathematics;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    int cardAmount;
    [SerializeField] int rows;

    [SerializeField] int columns;
    [SerializeField]
    float horizontalCardOffset = 1.5f;
    [SerializeField]
    float verticalCardOffset = 2.0f;

    [SerializeField] GameObject CardPrefab;
    Transform[,] cardTransforms;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardAmount = rows * columns;
        cardTransforms = new Transform[columns, rows];

        //Skapa kort och ID
        int idCounter = 0;
        int IDtoAssign = 0;
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject g = Instantiate(CardPrefab, transform.position, quaternion.identity);
                cardTransforms[i, j] = g.transform;
                cardTransforms[i, j].parent = transform;
                if (idCounter >= 2)
                {
                    IDtoAssign++;
                    idCounter = 0;
                }
                idCounter++;
                g.GetComponent<Card>().cardID = IDtoAssign;

            }
        }

        //shuffla arrayen 
        Shuffle(cardTransforms);

        //positionera kort
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Vector3 offset = new Vector3(i * horizontalCardOffset, 0, j * verticalCardOffset);
                cardTransforms[i, j].position += offset;
            }
        }
    }

    public void Shuffle<T>(T[,] array)
    {
        System.Random random = new System.Random();
        int lengthRow = array.GetLength(1);
        for (int i = array.Length - 1; i > 0; i--)
        {
            int i0 = i / lengthRow;
            int i1 = i % lengthRow;

            int j = random.Next(i + 1);
            int j0 = j / lengthRow;
            int j1 = j % lengthRow;

            T temp = array[i0, i1];
            array[i0, i1] = array[j0, j1];
            array[j0, j1] = temp;
        }
    }
}
