using System.Runtime.CompilerServices;
using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField]
    private Transform[] dismemberPoints;
    public int armHealth = 5;
    public bool dead = false;

    Transform targetJoiaaaant;

    public Transform targetJoint
    {
        get => dismemberPoints[5 - armHealth];

        private set { targetJoiaaaant = value; }
    }
    void Start()
    {
        //targetJoint = dismemberPoints[5 - armHealth];
    }
}
