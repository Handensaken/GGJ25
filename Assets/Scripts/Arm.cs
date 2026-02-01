using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using System.Collections.Generic;


public class Arm : MonoBehaviour
{
    [SerializeField] Transform Point01;
    [SerializeField] Transform Point02;
    [SerializeField] Transform Point03;
    [SerializeField] Transform Point04;
    [SerializeField] Transform Point05;
    [SerializeField] Transform Point06;
    private Transform[] dismemberPoints;

    public List<GameObject> BodyParts;
    public int armHealth = 6;
    public bool dead = false;
    public float side;
    Transform targetJoiaaaant;

    [SerializeField] Transform ChoppingPos;

    Vector3 defaultPosition;
    public Transform targetJoint
    {
        get => dead ? dismemberPoints[dismemberPoints.Length - 1] : dismemberPoints[6 - armHealth];

        private set { targetJoiaaaant = value; }
    }
    void Start()
    {
        defaultPosition = transform.position;
        dismemberPoints = new Transform[]
        {
            Point01,
            Point02,
            Point03,
            Point04,
            Point05,
            Point06,
        };
        GameEventManager.instance.OnPlayerDamaged += PositionHand;
        GameEventManager.instance.OnResetCleaver += ResetHandPos;
        //targetJoint = dismemberPoints[5 - armHealth];
    }
    public void Dismember()
    {
        //   Debug.Log(BodyParts.Count);
        GameObject g = dead ? BodyParts[BodyParts.Count - 1] : BodyParts[6 - armHealth];
        g.transform.parent = null;
        BoxCollider mC = g.AddComponent<BoxCollider>();
        Rigidbody rb = g.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce(Vector3.forward * -0.5f, ForceMode.Impulse);
       // mC.convex = true;
    }
    void OnDisable()
    {
        GameEventManager.instance.OnPlayerDamaged -= PositionHand;
        GameEventManager.instance.OnResetCleaver -= ResetHandPos;

    }
    bool hasMovedDead = false;
    private void ResetHandPos()
    {
        if (!hasMovedDead)
        {
            StartCoroutine(InterpolatePosition(0.5f, 100, ChoppingPos.position, defaultPosition));
            if (dead) hasMovedDead = true;
        }
    }
    private void PositionHand(int trashValue)
    {
        if (!hasMovedDead)
        {
            StartCoroutine(InterpolatePosition(0.5f, 100, defaultPosition, ChoppingPos.position));

        }
    }

    IEnumerator InterpolatePosition(float timeInSeconds, float timeStep, Vector3 startPos, Vector3 endPos)
    {
        //Vector3 localPos = transform.localPosition;
        //  Vector3 localOffsetPos = transform.localPosition + new Vector3(0, 0, 1f * side);
        for (float i = 0; i <= timeInSeconds * timeStep; i++)
        {
            float t = i / (timeInSeconds * timeStep);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return new WaitForSeconds(1 / timeStep);
        }

        //currentTargetPosition = targetReference.position;
    }
    IEnumerator InterpolateRotation(float timeInSeconds, float timeStep, Quaternion targetRot)
    {
        Quaternion targetStartRot = transform.rotation;
        for (float i = 0; i <= timeInSeconds * timeStep; i++)
        {
            float t = i / (timeInSeconds * timeStep);
            transform.rotation = Quaternion.Lerp(targetStartRot, targetRot, t);

            // target.position = Vector3.Lerp(targetStartPos, targetReference.position, t);

            yield return new WaitForSeconds(1 / timeStep);
        }
    }
}
