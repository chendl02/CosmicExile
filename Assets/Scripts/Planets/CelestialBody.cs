using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof (Rigidbody))]
public class CelestialBody : OrbitalMotion {


    //distance(10^9 m)
    //mass(10^18 kg)
    //acceleration(m/s^2)
    //velocity(m/s)

    public bool isMoon;

    public float siderealRotationPeriod;

    public float axialTilt;


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

    public bool inner;

    void Awake () {



        //Debug.Log(Clock.dayTime);
        rb = GetComponent<Rigidbody> ();
        //rb.mass = mass;
        float t = Universe.physicsTimeStep * Universe.timeCoefficient / 3600.0f / 24.0f;
        initVelocity = (GetRealPosition(t + Clock.startDay) - GetRealPosition(Clock.startDay)) / t * (1e+9f / 3600 / 24);
        this.transform.position = GetRealPosition(Clock.startDay);


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

        transform.rotation = Quaternion.Euler(-90, axialTilt, rb.rotation.eulerAngles.z);

    }
    void FixedUpdate()
    {
        if (Clock.speed == 0)
            return;
        // Calculate rotation speed (degrees per hour)
        float rotationSpeed = 360f / siderealRotationPeriod;

        // Apply rotation
        Quaternion deltaRotation = Quaternion.Euler(0, -rotationSpeed * (Universe.physicsTimeStep * Universe.timeCoefficient / 3600.0f), 0);
        rb.rotation = rb.rotation * deltaRotation;
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