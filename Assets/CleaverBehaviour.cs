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
        defaultLocalPos = HolderObject.localPosition;
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
        currentArm = null;
        animator.SetBool("Moving", true);
        targetReference = t;
        //Set HeadChop animation
    }
    Vector3 startPosition;
    float side;
    Arm currentArm;
    void Chopping(Arm arm)
    {
        currentArm = arm;
        side = arm.side;
        targetReference = arm.targetJoint;


        startPosition = transform.position;

        animator.SetBool("Moving", true);
    }

    [SerializeField] Transform HolderObject;
    void CleaverRecover()
    {
        AudioManager.Instance.Play("CuttingFingers");
        currentArm.Dismember();
        if (targetReference.name.Contains('F') || currentArm.armHealth == 1)
        {
            currentArm.armHealth--;
            if (currentArm.armHealth <= 0) currentArm.dead = true;
        }
        else
        {
            currentArm.armHealth = 0;
            currentArm.dead = true;
        }
        StartCoroutine(InterpolateCleaverRecovery(0.5f, 100));
    }
    void ResetCleaver()
    {
        StartCoroutine(InterpolareCleaverReset(1, 100));
        StartCoroutine(InterpolateRotation(1, 100, defaultRotation));
    }
    void StartMoving()
    {

        if (currentArm == null)
        {
            StartCoroutine(InterpolateHeadPosition(0.5f, 100));

        }
        else
        {

            StartCoroutine(InterpolatePosition(1, 100));
            StartCoroutine(InterpolateRotation(1, 100, targetReference.rotation));
        }
    }

    IEnumerator InterpolateHeadPosition(float timeInSeconds, float timeStep)
    {
        //  Vector3 localOffsetPos = transform.localPosition + new Vector3(0, 0, 1f * side);
        //  float f = 0.01f * side;
        for (float i = 0; i <= timeInSeconds * timeStep; i++)
        {
            float t = i / (timeInSeconds * timeStep);
            transform.position = Vector3.Lerp(startPosition, targetReference.position, t);
            //   transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
            //   float f2 = Mathf.Lerp(0, f, t);
            // HolderObject.localPosition = HolderObject.localPosition + new Vector3(0, 0, f2);
            yield return new WaitForSeconds(1 / timeStep);
        }
        animator.SetTrigger("Head");
        //currentTargetPosition = targetReference.position;
    }

    IEnumerator InterpolatePosition(float timeInSeconds, float timeStep)
    {
        //  Vector3 localOffsetPos = transform.localPosition + new Vector3(0, 0, 1f * side);
        float f = 0.01f * side;
        for (float i = 0; i <= timeInSeconds * timeStep; i++)
        {
            float t = i / (timeInSeconds * timeStep);
            transform.position = Vector3.Lerp(startPosition, targetReference.position, t);
            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
            float f2 = Mathf.Lerp(0, f, t);
            HolderObject.localPosition = HolderObject.localPosition + new Vector3(0, 0, f2);
            yield return new WaitForSeconds(1 / timeStep);
        }
        animator.SetTrigger("Chop");
        //currentTargetPosition = targetReference.position;
    }
    IEnumerator InterpolareCleaverReset(float timeInSeconds, float timeStep)
    {
        Vector3 localPos = HolderObject.localPosition;
        Vector3 pos = transform.position;
        //  Vector3 localOffsetPos = transform.localPosition + new Vector3(0, 0, 1f * side);
        for (float i = 0; i <= timeInSeconds * timeStep; i++)
        {
            float t = i / (timeInSeconds * timeStep);
            transform.position = Vector3.Lerp(pos, defaultPos, t);
            HolderObject.localPosition = Vector3.Lerp(localPos, defaultLocalPos, t);
            yield return new WaitForSeconds(1 / timeStep);
        }
        animator.SetBool("Moving", false);
        //currentTargetPosition = targetReference.position;
    }
    IEnumerator InterpolateCleaverRecovery(float timeInSeconds, float timeStep)
    {
        //  Vector3 localOffsetPos = transform.localPosition + new Vector3(0, 0, 1f * side);
        float f = 0.01f * side;
        for (float i = 0; i <= timeInSeconds * timeStep; i++)
        {
            float t = i / (timeInSeconds * timeStep);
            float f2 = Mathf.Lerp(0, f, t);
            HolderObject.localPosition = HolderObject.localPosition + new Vector3(0, Mathf.Abs(f2), f2);
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
