// /*******************************************************************************************
// * Author: German L.G Fica
// * Websites: http://germanfica.xyz
// * Description: Basic gravity attractor.
// *******************************************************************************************/
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace xyz.germanfica.unity.planet.gravity
// {
//     public class Attractor : MonoBehaviour
//     {
//         public static List<Attractor> Attractors;
//         public float gravity = -10;
//         private Transform m_Transform;
//         private Rigidbody m_Rigidbody;

//         /* Apply gravity to the game object
//          */
//         public void Attract(Transform body)
//         {
//             Vector3 gravityUp = (body.position - transform.position).normalized;
//             Vector3 bodyUp = body.up;
//             body.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);
//             Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;

//             body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
//         }

//         /* All necessary variables to use gravity are initialized 
//          */
//         void Start()
//         {
//             m_Rigidbody = GetComponent<Rigidbody>();
//             m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
//             m_Rigidbody.useGravity = false;

//             m_Transform = GetComponent<Transform>();
//         }

//         /* Applies gravity to game objects associated with the script
//          * 
//          * Note: each game object can have its own gravity
//          */
//         void FixedUpdate()
//         {
//             if (m_Rigidbody != null && m_Transform != null)
//             {
//                 foreach (Attractor attractor in Attractors)
//                 {
//                     if (attractor != this)
//                         attractor.Attract(m_Transform);
//                         Debug.Log("Attracting: " + m_Transform.name + " to " + attractor.name);
//                 }
//             }
//         }

//         /* Add this game object to the list
//          */
//         void OnEnable()
//         {
//             if (Attractors == null)
//                 Attractors = new List<Attractor>();

//             Attractors.Add(this);
//             Debug.Log("Added to Attractors list: " + gameObject.name);
//         }

//         /* When disabled, it must be deleted from the list
//          */
//         void OnDisable()
//         {
//             Attractors.Remove(this);
//         }
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public static List<Attractor> Attractors; // List of all attractors in the scene
    public float gravity = -10f; // Positive gravity strength for attraction
    private Rigidbody m_Rigidbody;

    //public Rigidbody Player;

    public Transform sphere; // 接受的 Sphere 的 Transform

    public bool if_allow_rotate;


        // Initialize necessary components
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        if (m_Rigidbody != null)
        {
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            m_Rigidbody.useGravity = true; // Disable Unity's default gravity
        }
    }

    // Apply gravity to objects in the attractor list
    void Update()
    {
        if (sphere != null && m_Rigidbody != null)
        {
            ProcessAttract();
        }
    }

    void ProcessAttract(){
        // m_Rigidbody.AddRelativeForce(-Vector3.up * 350f * Time.deltaTime);
        // 计算指向 Sphere 中心的方向
        Vector3 directionToCenter = (sphere.position - transform.position).normalized;

        // 计算物体与 Sphere 中心的距离
        float distance = Vector3.Distance(sphere.position, transform.position);

        // 防止距离为 0 导致力无限大
        distance = Mathf.Max(distance, 0.1f);

        // 计算引力大小：F = G / r^2
        float forceMagnitude = gravity / (distance * distance);

        // 施加引力
        m_Rigidbody.AddForce(directionToCenter * forceMagnitude, ForceMode.Acceleration);

        //Player.AddForce(directionToCenter * forceMagnitude, ForceMode.Acceleration);
        // 同步旋转，使物体的 Y 轴对准球心
        if(if_allow_rotate){
            AlignRotation(directionToCenter);
        }
        
    }

    // Add this attractor to the global list
    void OnEnable()
    {
        if (Attractors == null)
            Attractors = new List<Attractor>();

        Attractors.Add(this);
        Debug.Log("Added to Attractors list: " + gameObject.name);
    }

        // Remove this attractor from the global list
    void OnDisable()
        {
            Attractors.Remove(this);
            Debug.Log("Removed from Attractors list: " + gameObject.name);
        }
    void AlignRotation(Vector3 directionToCenter)
    {
        // 当前物体的“上”方向（Y 轴）
        Vector3 bodyUp = -transform.up;

        // 计算目标旋转：让物体的 Y 轴对准球心方向
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, directionToCenter) * transform.rotation;

        // 平滑插值旋转
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
    }
}
