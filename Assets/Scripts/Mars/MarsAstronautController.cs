using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsAstronautController : MonoBehaviour
{
    public Rigidbody rig;                  // Character Rigidbody
    public Transform cameraTransform;     // Camera Transform
    public Animator anim;                 // Animation Controller

    public float mouseSensitivity = 2f;   // Mouse Sensitivity
    public float moveSpeed = 100f;        // Movement Speed
    public float turnSpeed = 100f;        // Turning Speed
    public int forceConst = 4;            // Jump Force

    private float cameraPitch = 0f;       // Camera Pitch
    private bool canJump;                 // Can Jump Flag
    private bool onGround;                // On Ground Flag

    public string groundTag = "Planet";   // Ground Tag
    
    public bool isInsideCar;             //InCar Flag
    public bool isCarryingU = false;            
    private HealthManager healthManager;
    public AudioSource carEngineSound;   
    public AudioSource sandstormSound;    
    public float sandstormMaxDistance = 100f;  
    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;  
        rig.useGravity = false;  
        InvokeRepeating("CheckHealth", 1f, 1f);
        if (carEngineSound != null)
        {
            carEngineSound.loop = true; 
        }
        if (sandstormSound != null)
        {
            sandstormSound.loop = true;  
        }
    }

    void Update()
    {
        HandleMouseLook(); 
        HandleInput();  
        UpdateCarEngineSound();   
        UpdateSandstormSound();   
    }

    void FixedUpdate()
    {
        Move();           
        RotateCharacter(); 
        Jump();           
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        cameraTransform.parent.Rotate(Vector3.up * mouseX);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            canJump = true;
        }

        if (Input.GetKey("w") || Input.GetKey("s"))
        {
            anim.SetInteger("AnimationPar", 1);
        }
        else
        {
            anim.SetInteger("AnimationPar", 0);
        }
    }

    private void Move()
    {
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;
        Vector3 moveDirection = transform.forward * moveZ;
        rig.MovePosition(rig.position + moveDirection);
    }

    private void RotateCharacter()
    {
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime;
        transform.Rotate(0, turn, 0);
    }

    private void Jump()
    {
        if (canJump)
        {
            canJump = false;
            rig.AddForce(Vector3.up * forceConst, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            onGround = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MarsStorm"))
        {
            InvokeRepeating("ApplySandstormDamage", 1f, 1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MarsStorm"))
        {
            CancelInvoke("ApplySandstormDamage");
        }
    }

    private void ApplySandstormDamage()
    {
        if (healthManager != null)
        {
            healthManager.ReduceHealth(5);
        }
        else
        {
            Debug.LogWarning("No HealthManager found!");
        }
    }

    private void CheckHealth()
    {
        if (isCarryingU)
        {
            if (healthManager != null)
            {
                healthManager.ReduceHealth(2); 
            }
            else
            {
                Debug.LogWarning("No HealthManager found!");
            }
        }
        if (isInsideCar)
        {
            if (healthManager != null)
            {
                healthManager.AddHealth(2); 
            }
            else
            {
                Debug.LogWarning("No HealthManager found!");
            }
        }
    }
    private void UpdateCarEngineSound()
    {
        if (isInsideCar && carEngineSound != null)
        {
            float speed = rig.velocity.magnitude; 
            if (speed > 0.1f)
            {
                if (!carEngineSound.isPlaying)
                    carEngineSound.Play();
                carEngineSound.pitch = Mathf.Lerp(1f, 2f, speed / 50f);
            }
            else
            {
                if (carEngineSound.isPlaying)
                    carEngineSound.Stop();
            }
        }
    }

    private void UpdateSandstormSound()
{
    if (sandstormSound != null)
    {
        GameObject sandstormObject = GameObject.FindGameObjectWithTag("MarsStorm");
        
        if (sandstormObject != null)
        {
            float distanceToStorm = Vector3.Distance(transform.position, sandstormObject.transform.position);
            
            if (distanceToStorm < sandstormMaxDistance)
            {
                if (!sandstormSound.isPlaying)
                    sandstormSound.Play();
                sandstormSound.volume = 1 - (distanceToStorm / sandstormMaxDistance); 
            }
            else
            {
                if (sandstormSound.isPlaying)
                    sandstormSound.Stop();
            }
        }
        else
        {
            if (sandstormSound.isPlaying)
                sandstormSound.Stop();
        }
    }
}
}
