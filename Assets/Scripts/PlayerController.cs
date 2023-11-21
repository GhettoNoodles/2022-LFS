using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float maxVel;
    [SerializeField] private float jumpForce;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform cameraOrbit;
    [SerializeField] private Transform parent;
    [SerializeField] private float xsens;
    [SerializeField] private float ysens;
    private float tempmaxVel;
    private bool _grounded;
    private float _lookX;
    private float _lookY;
    private float _xRot;
    private float _yRot;
    private Vector3 _movement;
    private Vector3 _adjustedMovement;

    private void Start()
    {
        tempmaxVel = maxVel;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        _grounded = Physics.Raycast(ray, out hit, 2.66f);
        if (_grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            if (hit.collider.gameObject.CompareTag("Mud"))
            {
                if (rb.velocity.magnitude > 5f)
                {
                    rb.velocity = rb.velocity.normalized * 5f;
                }
                
                maxVel = 5f;
            }
            else
            {
                maxVel = tempmaxVel;
            }
        }
        else
        {
            maxVel = tempmaxVel;
        }
        _movement.x = Input.GetAxis("Horizontal");
        _movement.z = Input.GetAxis("Vertical");
        if (!_grounded)
        {
            _movement *= 0.5f;
        }

        _lookY = Input.GetAxis("Mouse X") * Time.deltaTime * xsens * 1000;
        _lookX = Input.GetAxis("Mouse Y") * Time.deltaTime * ysens * 1000;
        _yRot += _lookY;
        _xRot -= _lookX;
        _xRot = Mathf.Clamp(_xRot, -90f, 90f);
        cam.rotation = Quaternion.Euler(_xRot, 0, 0);
        cameraOrbit.rotation = Quaternion.Euler(0, _yRot, 0);
    }

    private void FixedUpdate()
    {
        parent.forward = new Vector3(cam.forward.x, 0, cam.forward.z);
        _adjustedMovement = parent.TransformDirection(_movement);
        if (rb.velocity.magnitude < maxVel)
        {
            rb.AddForce(_adjustedMovement * acceleration, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Danger"))
        {
            rb.velocity *= 0;
            rb.angularVelocity *= 0;
            GameManager.Instance.DamagePlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ring"))
        {
            GameManager.Instance.IncreaseRings();
        }
        else if (other.gameObject.CompareTag("CP"))
        {
            Checkpoint cp = other.gameObject.GetComponent<Checkpoint>();
            if (!cp.GetIsActive())
            {
                cp.Activate();
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ring"))
        {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Mud"))
        {
            maxVel = tempmaxVel;
        }
    }
}