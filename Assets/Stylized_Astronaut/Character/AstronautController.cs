using UnityEngine;

public class AstronautController : MonoBehaviour
{
    public Rigidbody rig;                  // 角色的刚体
    public Transform cameraTransform;     // 摄像机的 Transform
    public Animator anim;                 // 动画控制器

    public float mouseSensitivity = 2f;   // 鼠标灵敏度
    public float moveSpeed = 3f;          // 移动速度
    public float turnSpeed = 100f;        // 角色转向速度
    public int forceConst = 4;            // 跳跃力度

    private float cameraPitch = 0f;       // 摄像机俯仰角
    private bool canJump;                 // 是否可以跳跃
    private bool onGround;                // 是否在地面上

    public string groundTag = "Planet";   // 用于检测地面的标签

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标光标
        Cursor.visible = false;                  // 隐藏鼠标光标
        rig.useGravity = false;                  // 禁用重力
    }

    void Update()
    {
        HandleMouseLook(); // 鼠标控制摄像机
        HandleInput();     // 跳跃和动画控制
    }

    void FixedUpdate()
    {
        Move();            // 角色前后移动
        RotateCharacter(); // A/D 键控制角色水平转向
        Jump();            // 跳跃逻辑
    }

    /* 鼠标控制摄像机 */
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 更新摄像机俯仰角
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        // 更新摄像机旋转
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

        // 鼠标水平滑动独立控制摄像机视角，不影响角色
        cameraTransform.parent.Rotate(Vector3.up * mouseX);
    }

    /* 跳跃和动画控制 */
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            canJump = true;
        }

        // 更新动画
        if (Input.GetKey("w") || Input.GetKey("s"))
        {
            anim.SetInteger("AnimationPar", 1);
        }
        else
        {
            anim.SetInteger("AnimationPar", 0);
        }
    }

    /* 角色前后移动 */
    private void Move()
    {
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;
        Vector3 moveDirection = transform.forward * moveZ; // 前后移动基于角色的朝向
        rig.MovePosition(rig.position + moveDirection);
    }

    /* A/D 键控制角色水平转向 */
    private void RotateCharacter()
    {
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime;
        transform.Rotate(0, turn, 0); // A/D 键控制角色水平旋转
    }

    /* 跳跃逻辑 */
    private void Jump()
    {
        if (canJump)
        {
            canJump = false;
            rig.AddForce(Vector3.up * forceConst, ForceMode.Impulse);
        }
    }

    /* 碰撞检测 */
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
}
