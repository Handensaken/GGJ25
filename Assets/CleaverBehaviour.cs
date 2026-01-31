using UnityEngine;

public class CleaverBehaviour : MonoBehaviour
{
    [SerializeField] private Transform target;

    Transform targetReference;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEventManager.instance.OnCleaverChopCalled += Chopping;
    }
    void Chopping(Arm arm)
    {
        targetReference = arm.targetJoint;
        transform.position = targetReference.position;
        transform.localPosition = transform.localPosition + new Vector3(0, 0, -0.5f);
        target.position = targetReference.position;
    }
    // Update is called once per frame
    void Update()
    {
        target.position = new Vector3(target.position.x, 1.154f, target.position.z);
        transform.LookAt(target);
    }
}
