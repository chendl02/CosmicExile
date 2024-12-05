// using UnityEngine;
// using System.Collections;
// public class PlayerController : MonoBehaviour
// {
//     public Rigidbody rig;
//     RaycastHit hit;
//     public bool freezeRotation = true;

//     public int forceConst = 4;
//     private bool canJump;

//     void Start()
//     {
//         Ini();
//     }

//     void Update()
//     {
//         // Raycast (doesn't affect gameplay)
//         if (Physics.Raycast(transform.position, -transform.up, out hit))
//         {
//             Debug.DrawLine(transform.position, hit.point, Color.cyan);
//         }
//         // Jump Action
//         if (Input.GetKeyUp(KeyCode.Space))
//         {
//             canJump = true;
//         }
//     }

//     void FixedUpdate()
//     {
//         Move();
//         Jump();
//     }

//     /* Some initializations
//         */
//     private void Ini()
//     {
//         rig.useGravity = false; // Disables Gravity
//         if (freezeRotation)
//         {
//             rig.constraints = RigidbodyConstraints.FreezeRotation;
//         }
//         else
//         {
//             rig.constraints = RigidbodyConstraints.None;
//         }
//     }

//     /* Character jump
//         */
//     private void Jump()
//     {
//         if (canJump)
//         {
//             canJump = false;
//             // AddForce (useless)
//             //rig.AddForce(0, forceConst, 0, ForceMode.Impulse);
//             // AddForceAtPosition (useless too)
//             //rig.AddForceAtPosition(new Vector3(0, 0, forceConst), rig.transform.position, ForceMode.Impulse);
//             // AddRelativeForce (successful)
//             rig.AddRelativeForce(0, forceConst, 0, ForceMode.Impulse);
//         }
//     }

//     /* Character movement
//         */
//     private void Move()
//     {
//         var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
//         var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

//         transform.Rotate(0, x, 0);
//         transform.Translate(0, 0, z);
//     }
// }
using UnityEngine;
using System.Collections;

public class TruckController : MonoBehaviour
{
    public Rigidbody rig;
    public Transform cameraTransform; // 摄像机的 Transform

    //public Transform sphereCenter; // 球体的 Transform
    public float mouseSensitivity = 100f; // 鼠标灵敏度
    public float verticalLookLimit = 80f; // 限制垂直旋转角度
    private float verticalRotation = 0f; // 当前摄像机的垂直旋转角度

    RaycastHit hit;
    public bool freezeRotation = true;
    public int forceConst = 4;
    private bool canJump;

    void Start()
    {
        Ini();
        // 隐藏并锁定鼠标光标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //RotateTowardsSphereCenter(); // 让 Y 轴指向球心
        
        //MouseControl(); // 处理鼠标输入

        // Raycast (doesn't affect gameplay)
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            Debug.DrawLine(transform.position, hit.point, Color.cyan);
        }

        // Jump Action
        if (Input.GetKeyUp(KeyCode.Space))
        {
            canJump = true;
        }
    }

    /* 让 Player 的 Y 轴指向球心 */
    // private void RotateTowardsSphereCenter()
    // {
    //     if (sphereCenter == null) return;

    //     // 计算指向球心的方向向量
    //     Vector3 directionToCenter = (sphereCenter.position - transform.position).normalized;

    //     // 更新旋转：让 Y 轴指向球心
    //     Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, directionToCenter);
    //     transform.rotation = targetRotation * Quaternion.Euler(0, transform.eulerAngles.y, 0);
    // }

    void FixedUpdate()
    {
        Move();
        Jump();
    }

    /* 初始化 */
    private void Ini()
    {
        rig.useGravity = false; // Disables Gravity
        if (freezeRotation)
        {
            rig.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rig.constraints = RigidbodyConstraints.None;
        }
    }

    /* 鼠标控制逻辑 */
    private void MouseControl()
    {
        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 水平旋转 Player
        transform.Rotate(0, mouseX, 0);

        // 垂直旋转摄像机（限制角度范围）
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        // 更新摄像机旋转
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    /* 跳跃逻辑 */
    private void Jump()
    {
        if (canJump)
        {
            canJump = false;
            rig.AddRelativeForce(0, forceConst, 0, ForceMode.Impulse);
        }
    }

    /* 移动逻辑 */
    private void Move()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }
}