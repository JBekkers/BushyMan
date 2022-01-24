using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        target = Camera.main.transform;
    }

    void Update()
    {
        Vector3 targetPostition = new Vector3(target.transform.position.x, -90, target.transform.position.z);
        transform.LookAt(targetPostition);
    }
}
