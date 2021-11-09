using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        Vector3 targetPostition = new Vector3(target.transform.position.x, -90, target.transform.position.z);
        transform.LookAt(targetPostition);
    }
}
