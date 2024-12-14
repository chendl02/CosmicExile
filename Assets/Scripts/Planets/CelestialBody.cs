using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof (Rigidbody))]
public class CelestialBody : OrbitalMotion {


    //distance(10^9 m)
    //mass(10^18 kg)
    //acceleration(m/s^2)
    //velocity(m/s)

    public float radius;
    public float surfaceGravity;
    //public Vector3 initialVelocity;
    public string bodyName = "Unnamed";
    public Transform meshHolder;
    public float gravitationalConstant = 6.674e-11f;


    public Vector3 initVelocity;
    public Vector3 velocity { get; private set; }
    public float mass;
    Rigidbody rb;

    private bool enableVirtualMesh = false;
    private Transform virtualMesh;

    void Awake () {
        rb = GetComponent<Rigidbody> ();
        //rb.mass = mass;
        float t = Universe.physicsTimeStep * Universe.timeCoefficient / 3600.0f / 24.0f;
        initVelocity = (GetRealPosition(t) - GetRealPosition(0)) / t * (1e+9f / 3600 / 24);
        this.transform.position = GetRealPosition(0);


        // 创建一个 meshHolder 的副本
        if (enableVirtualMesh)
        {
            virtualMesh = Instantiate(meshHolder, meshHolder.position, meshHolder.rotation);

            // 移除 Sphere Collider
            SphereCollider sphereCollider = virtualMesh.GetComponent<SphereCollider>();
            if (sphereCollider != null)
            {
                Destroy(sphereCollider);
            }

            // 设置 virtualMesh 为未启用状态
            virtualMesh.gameObject.SetActive(false);
        }

    }

    public void ActivateVirtualMesh(Vector3 position)
    {
        if (!enableVirtualMesh)
            return;
        if (virtualMesh != null)
        {
            virtualMesh.gameObject.SetActive(true); // 启用
            virtualMesh.position = position; // 设置绝对位置
        }
        else
        {
            Debug.LogError("Virtual Mesh 未正确初始化！");
        }
    }

    public void DeactivateVirtualMesh()
    {
        if (!enableVirtualMesh)
            return;
        if (virtualMesh != null)
        {
            virtualMesh.gameObject.SetActive(false); // 禁用
        }
        else
        {
            Debug.LogError("Virtual Mesh 未正确初始化！");
        }
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
        rb.MovePosition(GetRealPosition(Clock.dayTime));
    }
    void OnValidate () {
        meshHolder = transform.GetChild (0);
        meshHolder.localScale = Vector3.one * radius * 2;
        gameObject.name = bodyName;
    }

    public Rigidbody Rigidbody {
        get {
            return rb;
        }
    }

    public Vector3 Position {
        get {
            return transform.position;
        }
    }

}