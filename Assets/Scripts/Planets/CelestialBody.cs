using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof (Rigidbody))]
public class CelestialBody : OrbitalMotion {


    //distance(10^9 m)
    //mass(10^18 kg)
    //acceleration(m/s^2)
    //velocity(m/s)

    //10^36kg/10^18 m =10^18kg

    public float radius;
    public float surfaceGravity;
    public Vector3 initialVelocity;
    public string bodyName = "Unnamed";
    Transform meshHolder;
    public float gravitationalConstant = 6.674e-11f;

    public Vector3 velocity { get; private set; }
    public float mass;
    Rigidbody rb;

    void Awake () {
        rb = GetComponent<Rigidbody> ();
        //rb.mass = mass;
        velocity = initialVelocity;
        this.transform.position = GetRealPosition(0);
    }
    /*
    protected override void Start()
    {
        base.Start();
        //gravityCamera.orthographicSize = radius * 5;
    }

    public void UpdateVelocity (CelestialBody[] allBodies, float timeStep) {
        foreach (var otherBody in allBodies) {
            if (otherBody != this) {
                float sqrDst = (otherBody.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rb.position - rb.position).normalized;

                Vector3 acceleration = forceDir * gravitationalConstant * otherBody.mass / sqrDst;
                velocity += acceleration * timeStep;
            }
        }
    }

    public void UpdateVelocity (Vector3 acceleration, float timeStep) {
        velocity +=  acceleration * timeStep;
    }

    public void UpdatePosition (float timeStep) {
        rb.MovePosition (rb.position +  velocity * timeStep / Universe.distanceCoefficint);

    }*/

    public void UpdatePosition()
    {
        rb.MovePosition(GetRealPosition(Clock.time));
    }
    void OnValidate () {
        meshHolder = transform.GetChild (0);
        meshHolder.localScale = Vector3.one * radius;
        gameObject.name = bodyName;
    }

    public Rigidbody Rigidbody {
        get {
            return rb;
        }
    }

    public Vector3 Position {
        get {
            return rb.position;
        }
    }

}