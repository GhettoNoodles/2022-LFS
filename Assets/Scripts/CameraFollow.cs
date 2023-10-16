using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 target;

    private Vector3 _offset;
    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - target;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3((target.x + _offset.x), (target.y + _offset.y), (target.z+ _offset.z));
    }
}
