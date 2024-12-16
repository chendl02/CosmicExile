using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ship : GravityObject {

    public Vector3 initPosition;
    public Vector3 initVelocity;
    public MotionData motionData;

    public Transform hatch;
    public float hatchAngle;
    public Transform camViewPoint;
    public Transform pilotSeatPoint;
    public LayerMask groundedMask;

    public Camera shipCam;
    private GameObject mapCamObject;

    [Header ("Handling")]
    public float thrustStrength = 50;
    public float rotSpeed = 5;
    public float rollSpeed = 30;
    public float rotSmoothSpeed = 10;
    public bool lockCursor;

    [Header ("Interact")]
    public Interactable flightControls;

    public Rigidbody rb;
    Quaternion targetRot;
    Quaternion smoothedRot;

    Vector3 thrusterInput;
    //PlayerController pilot;
    //bool shipIsPiloted;
    int numCollisionTouches;
    bool hatchOpen;

    KeyCode ascendKey = KeyCode.Space;
    KeyCode descendKey = KeyCode.LeftShift;
    KeyCode rollCounterKey = KeyCode.Q;
    KeyCode rollClockwiseKey = KeyCode.E;
    KeyCode forwardKey = KeyCode.W;
    KeyCode backwardKey = KeyCode.S;
    KeyCode leftKey = KeyCode.A;
    KeyCode rightKey = KeyCode.D;

    public TextMeshProUGUI positionText;
    public TextMeshProUGUI velocityText;

    void Start () {
        InitRigidbody ();
        targetRot = transform.rotation;
        smoothedRot = transform.rotation;
        rb.MovePosition(initPosition);
        motionData.Position = initPosition;
        motionData.Velocity = initVelocity;

        // if (lockCursor) {
        //     Cursor.lockState = CursorLockMode.Locked;
        //     Cursor.visible = false;
        // }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        shipCam.transform.parent = camViewPoint;
    }

    void Update () {

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("switch camera");
           
            if (shipCam.enabled == true)
            {
                shipCam.enabled = false;
                mapCamObject = new GameObject("Map Camera");
                CameraBehavior cameraBehavior = mapCamObject.AddComponent<CameraBehavior>();
                cameraBehavior.Initialize(this.transform);
                
            }
            else
            {
                Destroy(mapCamObject);
                shipCam.enabled = true;
                
            }
        }
        
        HandleMovement ();

        // Animate hatch
        //float hatchTargetAngle = (hatchOpen) ? hatchAngle : 0;
        //hatch.localEulerAngles = Vector3.right * Mathf.LerpAngle (hatch.localEulerAngles.x, hatchTargetAngle, Time.deltaTime);
    }

    void HandleMovement () {
        // DebugHelper.HandleEditorInput (lockCursor);
        // Thruster input
        int thrustInputX = GetInputAxis (leftKey, rightKey);
        int thrustInputY = GetInputAxis (descendKey, ascendKey);
        int thrustInputZ = GetInputAxis (backwardKey, forwardKey);
        thrusterInput = new Vector3 (thrustInputX, thrustInputY, thrustInputZ);

        // Rotation input
        float yawInput = Input.GetAxisRaw ("Mouse X") * rotSpeed;
        float pitchInput = Input.GetAxisRaw ("Mouse Y") * rotSpeed;
        float rollInput = GetInputAxis (rollCounterKey, rollClockwiseKey) * rollSpeed * Time.deltaTime;

        // Calculate rotation
        if (numCollisionTouches == 0) {
            var yaw = Quaternion.AngleAxis (yawInput, transform.up);
            var pitch = Quaternion.AngleAxis (-pitchInput, transform.right);
            var roll = Quaternion.AngleAxis (-rollInput, transform.forward);

            targetRot = yaw * pitch * roll * targetRot;
            smoothedRot = Quaternion.Slerp (transform.rotation, targetRot, Time.deltaTime * rotSmoothSpeed);
        } else {
            targetRot = transform.rotation;
            smoothedRot = transform.rotation;
        }
    }

    void FixedUpdate () {
        if (Clock.speed == 0)
            return;

        if (shipCam.enabled)
        {
            // Thrusters
            Vector3 thrustDir = transform.TransformVector(thrusterInput);
            motionData.Velocity += thrustDir * thrustStrength;

            // Rotate
            if (numCollisionTouches == 0)
            {
                rb.MoveRotation(smoothedRot);
            }
        }


        // Gravity
        motionData = NBodySimulation.RK4(motionData, Clock.dayTime, Universe.physicsTimeStep * Universe.timeCoefficient);
        rb.MovePosition(motionData.Position);



        //Vector3 gravity = NBodySimulation.CalculateAcceleration (rb.position);
        //rb.AddForce (gravity, ForceMode.Acceleration);



        
        


        positionText.text = $"Position: {motionData.Position}";
        velocityText.text = $"Velocity: {motionData.Velocity}";

    }

    int GetInputAxis (KeyCode negativeAxis, KeyCode positiveAxis) {
        int axis = 0;
        if (Input.GetKey (positiveAxis)) {
            axis++;
        }
        if (Input.GetKey (negativeAxis)) {
            axis--;
        }
        return axis;
    }

    void InitRigidbody () {
        rb = GetComponent<Rigidbody> ();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.centerOfMass = Vector3.zero;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    /*
    public void ToggleHatch () {
        hatchOpen = !hatchOpen;
    }

    public void TogglePiloting () {
        if (shipIsPiloted) {
            StopPilotingShip ();
        } else {
            PilotShip ();
        }
    }
    */

    /*
    public void PilotShip () {
        pilot = FindObjectOfType<PlayerController> ();
        shipIsPiloted = true;
        pilot.Camera.transform.parent = camViewPoint;
        pilot.Camera.transform.localPosition = Vector3.zero;
        pilot.Camera.transform.localRotation = Quaternion.identity;
        pilot.gameObject.SetActive (false);
        hatchOpen = false;

        
    }
    */
    /*
    void StopPilotingShip () {
        shipIsPiloted = false;
        pilot.transform.position = pilotSeatPoint.position;
        pilot.transform.rotation = pilotSeatPoint.rotation;
        pilot.Rigidbody.velocity = rb.velocity;
        pilot.gameObject.SetActive (true);
        pilot.ExitFromSpaceship ();
    }*/

    void OnCollisionEnter (Collision other) {
        if (groundedMask == (groundedMask | (1 << other.gameObject.layer))) {
            numCollisionTouches++;
        }
    }

    void OnCollisionExit (Collision other) {
        if (groundedMask == (groundedMask | (1 << other.gameObject.layer))) {
            numCollisionTouches--;
        }
    }

    public void SetVelocity (Vector3 velocity) {
        rb.velocity = velocity;
    }

    public bool ShowHUD {
        get {
            return true;
        }
    }
    public bool HatchOpen {
        get {
            return false;
        }
    }

    /*public bool IsPiloted {
        get {
            return shipIsPiloted;
        }
    }*/

    public Rigidbody Rigidbody {
        get {
            return rb;
        }
    }

}