using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform OrbitPoint;
    void Update()
    {
        OrbitPoint.position = target.position;
        transform.LookAt(target.position);
       }
}
