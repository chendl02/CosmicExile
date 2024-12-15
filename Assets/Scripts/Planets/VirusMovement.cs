using UnityEngine;

public class VirusMovement : MonoBehaviour
{
    public float moveSpeed = 5f;         // 移动速度
    public float detectionRange = 10f;  // 检测范围
    public float planetRadius = 184f;   // 球体半径
    public Vector3 planetCenter = Vector3.zero; // 球心位置（默认原点）

    private Transform playerTransform;

    void Start()
    {
        // 找到玩家对象并获取其 Transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // 计算与玩家的距离
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // 如果玩家在检测范围内，病毒朝向玩家移动
            if (distanceToPlayer <= detectionRange)
            {
                // 计算从病毒指向玩家的方向
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

                // 计算病毒沿球体表面的切线方向移动
                Vector3 virusToCenter = (transform.position - planetCenter).normalized; // 从球心指向病毒的方向
                Vector3 tangentDirection = Vector3.Cross(virusToCenter, Vector3.Cross(directionToPlayer, virusToCenter)).normalized;

                // 更新病毒的位置
                Vector3 newPosition = transform.position + tangentDirection * moveSpeed * Time.deltaTime;

                // 重新约束到球体表面
                newPosition = planetCenter + (newPosition - planetCenter).normalized * planetRadius;

                // 更新位置
                transform.position = newPosition;

                // 始终保持朝向地心
                transform.LookAt(planetCenter);
            }
        }
    }

    // 可视化检测范围（仅编辑器中显示）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
