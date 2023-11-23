using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform cameraOrbit;
    [SerializeField] private Transform parent;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxVel;
    [SerializeField] private float jumpForce;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float jumpCooldownTime;
    
    private bool _grounded;
    private bool _muddy;
    
    private float _tempMaxVel;
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;
    private float _jumpCooldownCounter;
    private float _lookX;
    private float _mouseY;
    private float _controllerY;
    private float _xRot;
    private float _yRot;
    
    private Vector3 _movement;
    private Vector3 _adjustedMovement;
    private LayerMask groundMask;

    private void Start()
    {
        _tempMaxVel = maxVel;
        groundMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            GameManager.Instance.PauseGame();
        }

        if (_jumpCooldownCounter>0f)
        {
            _jumpCooldownCounter -= Time.deltaTime;
        }
        //Check that player can jump
        _grounded = Physics.CheckSphere(transform.position,3f, groundMask);

        if (_grounded)
        {
            _coyoteTimeCounter = coyoteTime;
            
            //Slows down if player touches mud
            if (_muddy)
            {
                if (rb.velocity.z+rb.velocity.x > 5f)
                {
                    rb.velocity = rb.velocity.normalized * 5f;
                }

                maxVel = 5f;
            }
            else
            {
                maxVel = _tempMaxVel; //returns max velocity to normal value
            }
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
            maxVel = _tempMaxVel; //returns max velocity if mid air
        }

        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        
        if (_coyoteTimeCounter > 0 && _jumpBufferCounter > 0&& _jumpCooldownCounter<0.1f)
        {
            //Reset Jump modifiers
            _coyoteTimeCounter = 0;
            _jumpBufferCounter = 0;
            _jumpCooldownCounter = jumpCooldownTime;
            
            //jump
            rb.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
        }

        //Player movement input
        _movement.x = Input.GetAxis("Horizontal");
        _movement.z = Input.GetAxis("Vertical");
        if (!_grounded)
        {
            _movement *= 0.5f; //airspeed
        }

        //player looking input
        _mouseY = Input.GetAxis("Mouse X") * Time.deltaTime * lookSensitivity * 1000;
        _controllerY = Input.GetAxis("Controller X") * Time.deltaTime * lookSensitivity * 1000;
        if (Math.Abs(_mouseY) > Math.Abs(_controllerY))
        {
            _yRot += _mouseY;
        }
        else
        {
            _yRot += _controllerY;
        }

        cameraOrbit.rotation = Quaternion.Euler(0, _yRot, 0);
    }

    private void FixedUpdate()
    {
        //implement movement based on camera direction
        parent.forward = new Vector3(cam.forward.x, 0, cam.forward.z);
        _adjustedMovement = parent.TransformDirection(_movement);
        if (rb.velocity.magnitude < maxVel)
        {
            rb.AddForce(_adjustedMovement * moveSpeed,ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Danger")) //Check for Lava
        {
            rb.velocity *= 0;
            rb.angularVelocity *= 0;
            GameManager.Instance.DamagePlayer();
        }
        else if (other.gameObject.CompareTag("Fin")) //Check for Finish
        {
            GameManager.Instance.EndGame(true);
        }
        else if (other.gameObject.CompareTag("Mud")) //check for Mud sound
        {
            AudioManager.Instance.Mud();
            _muddy = true;
        }
        else
        {
            AudioManager.Instance.Bounce(rb.velocity.y / maxVel); //bounce sound
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Mud")) //check for Mud sound
        {
            _muddy = false;
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ring")) //Check if player moves through Ring
        {
            GameManager.Instance.IncreaseRings();
            other.gameObject.GetComponent<Ring>().Use();
        }
        else if (other.gameObject.CompareTag("CP")) //Check if Player activates CheckPoint
        {
            Checkpoint cp = other.gameObject.GetComponent<Checkpoint>();
            if (!cp.GetIsActive())
            {
                cp.Activate();
            }
        }
    }
}