using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;

public class CleaverBehaviour : MonoBehaviour
{

    [SerializeField] private Transform target;
    Transform targetReference;
    Quaternion defaultRotation;

    Vector3 defaultLocalPos;
    Vector3 defaultPos;
    [SerializeField] Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEventManager.instance.OnCleaverChopCalled += Chopping;
        GameEventManager.instance.OnCleaverHeadChop += HeadChop;
        GameEventManager.instance.OnRecoverCleaver += CleaverRecover;
        GameEventManager.instance.OnResetCleaver += ResetCleaver;
        GameEventManager.instance.OnCleaverMoveStart += StartMoving;

        //defaultTargetPosition = new Vector3(target.position.x, 1.154f, target.position.z);
        defaultRotation = transform.rotation;
        defaultLocalPos = transform.localPosition;
        defaultPos = transform.position;
    }
    void OnDisable()
    {
        GameEventManager.instance.OnCleaverChopCalled -= Chopping;
        GameEventManager.instance.OnCleaverHeadChop -= HeadChop;
        GameEventManager.instance.OnResetCleaver -= ResetCleaver;
        GameEventManager.instance.OnRecoverCleaver -= CleaverRecover;
        GameEventManager.instance.OnCleaverMoveStart -= StartMoving;
    }
    void HeadChop(Transform t)
    {
        //Set HeadChop animation
    }
    Vector3 startPosition;
    float side;
    void Chopping(Arm arm)
    {
        side = arm.side;
        targetReference = arm.targetJoint;

        if (targetReference.name.Contains('F'))
        {
            Debug.Log("Fingie");
            arm.armHealth--;
        }

        //  transform.position = targetReference.position;
        startPosition = transform.position;
        Debug.Log(targetReference);
        animator.SetBool("Moving", true);


        //  transform.localPosition = transform.localPosition + new Vector3(0, 0, 0.5f * arm.side);

        //  target.position = targetReference.position;
        //Interpolate position
        //Play movement animation

        //Call animation when in position
    }
    // Update is called once per frame
    void Update()
    {
        /*if (!interpolating)
        {
            target.position = currentTargetPosition;
        }*/
        //transform.LookAt(target);
    }
    void CleaverRecover()
    {
        StartCoroutine(InterpolateCleaverRecovery(0.5f, 100));
    }
    void ResetCleaver()
    {
        StartCoroutine(InterpolareCleaverReset(1, 100));
        StartCoroutine(InterpolateRotation(1, 100, defaultRotation));
    }
    void StartMoving()
    {
        StartCoroutine(InterpolatePosition(1, 100));
        StartCoroutine(InterpolateRotation(1, 100, targetReference.rotation));
    }
    IEnumerator InterpolatePosition(float timeInSeconds, float timeStep)
    {
        //  Vector3 localOffsetPos = transform.localPosition + new Vector3(0, 0, 1f * side);
        float f = 0.5f * side;
        for (float i = 0; i <= timeInSeconds * timeStep; i++)
        {
            float t = i / (timeInSeconds * timeStep);
            transform.position = Vector3.Lerp(startPosition, targetReference.position, t);
            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
            float f2 = Mathf.Lerp(0, f, t);
            transform.localPosition = transform.localPosition + new Vector3(f2, 0, 0);
            yield return new WaitForSeconds(1 / timeStep);
        }
        animator.SetTrigger("Chop");
        //currentTargetPosition = targetReference.position;
    }
    IEnumerator InterpolareCleaverReset(float timeInSeconds, float timeStep)
    {
        Vector3 localPos = transform.localPosition;
        Vector3 pos = transform.position;
        //  Vector3 localOffsetPos = transform.localPosition + new Vector3(0, 0, 1f * side);
        float f = 0.4f * side;
        for (float i = 0; i <= timeInSeconds * timeStep; i++)
        {
            float t = i / (timeInSeconds * timeStep);
            transform.position = Vector3.Lerp(pos, defaultPos, t);
            transform.localPosition = Vector3.Lerp(localPos, defaultLocalPos, t);
            yield return new WaitForSeconds(1 / timeStep);
        }
        animator.SetBool("Moving", false);
        //currentTargetPosition = targetReference.position;
    }
    IEnumerator InterpolateCleaverRecovery(float timeInSeconds, float timeStep)
    {
        Vector3 localPos = transform.localPosition;
        //  Vector3 localOffsetPos = transform.localPosition + new Vector3(0, 0, 1f * side);
        float f = 0.01f * side;
        for (float i = 0; i <= timeInSeconds * timeStep; i++)
        {
            float t = i / (timeInSeconds * timeStep);
            float f2 = Mathf.Lerp(0, f, t);
            transform.localPosition = transform.localPosition + new Vector3(f2, Mathf.Abs(f2), 0);
            yield return new WaitForSeconds(1 / timeStep);
        }
        //currentTargetPosition = targetReference.position;
    }

    IEnumerator InterpolateRotation(float timeInSeconds, float timeStep, Quaternion target)
    {
        Quaternion targetStartRot = transform.rotation;
        for (float i = 0; i <= timeInSeconds * timeStep; i++)
        {
            float t = i / (timeInSeconds * timeStep);
            transform.rotation = Quaternion.Lerp(targetStartRot, target, t);
            yield return new WaitForSeconds(1 / timeStep);
        }
    }
}
