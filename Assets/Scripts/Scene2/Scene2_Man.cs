using UnityEngine;

public class TruckController : MonoBehaviour
{
    public Rigidbody rig;                  // 角色刚体
    public Transform cameraTransform;     // 摄像机的 Transform
    public LayerMask groundLayer;         // 用于检测地面的层级

    public float mouseSensitivity = 200f; // 鼠标灵敏度
    public float verticalLookLimit = 80f; // 限制垂直旋转角度
    public float moveSpeed = 5f;          // 移动速度
    public float turnSpeed = 100f;        // 角色转向速度
    public float jumpForce = 5f;          // 跳跃力度

    private float verticalRotation = 0f;  // 摄像机垂直旋转角度
    private float horizontalRotation = 0f; // 摄像机水平旋转角度
    private bool canJump;                 // 是否可以跳跃

    void Start()
    {
        Ini();
        Cursor.lockState = CursorLockMode.Locked; // 隐藏并锁定鼠标光标
        Cursor.visible = false;
    }

    void Update()
    {
        MouseControl();  // 鼠标控制视角
        CheckGround();   // 检查是否接触地面

        // 跳跃输入检测
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Move(); // 处理移动逻辑
    }

    private void Ini()
    {
        rig.useGravity = true; 
        rig.constraints = RigidbodyConstraints.FreezeRotation; 
    }

    /* 鼠标控制逻辑 */
    private void MouseControl()
    {
        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 水平旋转摄像机（不影响角色）
        horizontalRotation += mouseX;
        cameraTransform.localRotation = Quaternion.Euler(0, horizontalRotation, 0);

        // 垂直旋转摄像机
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

    /* 检测是否接触地面 */
    private void CheckGround()
    {
        canJump = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    /* 跳跃逻辑 */
    private void Jump()
    {
        rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    /* 移动和转向逻辑 */
    private void Move()
    {
        // 前后移动
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;
        rig.MovePosition(rig.position + transform.forward * moveZ);

        // 左右转向由 A/D 控制
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime;
        transform.Rotate(0, turn, 0);
    }
}
