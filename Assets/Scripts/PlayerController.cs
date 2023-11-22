using System.Collections;
using UnityEngine;
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
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBufferTime;
    private float _tempMaxVel;
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;
    private bool _grounded;
    private float _lookX;
    private float _lookY;
    private float _xRot;
    private float _yRot;
    private Vector3 _movement;
    private Vector3 _adjustedMovement;

    private void Start()
    {
        _tempMaxVel = maxVel;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            GameManager.Instance.PauseGame();
        }

        //Check that player can jump
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        _grounded = Physics.Raycast(ray, out hit, 1.8f);

        if (_grounded)
        {
            _coyoteTimeCounter = coyoteTime;
            
            //Slows down if player touches mud
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

        if (_coyoteTimeCounter > 0 && _jumpBufferCounter > 0) 
        {
            //jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _coyoteTimeCounter = 0;
            _jumpBufferCounter = 0;
        }

        //Player movement input
        _movement.x = Input.GetAxis("Horizontal");
        _movement.z = Input.GetAxis("Vertical");
        if (!_grounded)
        {
            _movement *= 0.5f; //airspeed
        }

        //player looking input (Uses controller joystick Only)
        _lookY = Input.GetAxis("Mouse X") * Time.deltaTime * lookSensitivity * 1000;
        _yRot += _lookY;
        cameraOrbit.rotation = Quaternion.Euler(0, _yRot, 0);
    }

    private void FixedUpdate()
    {
        //implement movement based on camera direction
        parent.forward = new Vector3(cam.forward.x, 0, cam.forward.z);
        _adjustedMovement = parent.TransformDirection(_movement);
        if (rb.velocity.magnitude < maxVel)
        {
            rb.AddForce(_adjustedMovement * acceleration, ForceMode.Acceleration);
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
        }
        else
        {
            AudioManager.Instance.Bounce(rb.velocity.y / 10); //bounce sound
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