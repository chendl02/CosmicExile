using UnityEngine;

public class VirusMovement : MonoBehaviour
{
    public float moveSpeed = 8f;         // 病毒移动速度
    public float detectionRange = 30f;   // 病毒检测玩家的范围
    public float predictionTime = 1f;    // 预测玩家位置的时间（秒）
    public float planetRadius = 184f;    // 球体半径
    public Vector3 planetCenter = Vector3.zero; // 球心位置

    private Transform playerTransform;
    private Rigidbody playerRigidbody;

    void Start()
    {
        // 获取玩家对象及其 Transform 和 Rigidbody
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerRigidbody = player.GetComponent<Rigidbody>();

            if (playerRigidbody == null)
            {
                Debug.LogWarning("Player Rigidbody not found! Predictive movement may not work.");
            }
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

            // 如果玩家在检测范围内，病毒进行追踪
            if (distanceToPlayer <= detectionRange)
            {
                // 预测玩家的未来位置
                Vector3 predictedPosition = PredictPlayerPosition();

                // 计算从病毒当前位置指向预测位置的方向
                Vector3 directionToTarget = (predictedPosition - transform.position).normalized;

                // 沿球体表面移动的切线方向
                Vector3 virusToCenter = (transform.position - planetCenter).normalized;
                Vector3 tangentDirection = Vector3.Cross(virusToCenter, Vector3.Cross(directionToTarget, virusToCenter)).normalized;

                // 更新病毒的位置
                Vector3 newPosition = transform.position + tangentDirection * moveSpeed * Time.deltaTime;

                // 约束到球体表面
                newPosition = planetCenter + (newPosition - planetCenter).normalized * planetRadius;

                // 更新位置
                transform.position = newPosition;

                // 始终保持朝向地心
                transform.LookAt(planetCenter);
            }
        }
    }

    // 预测玩家的未来位置
    Vector3 PredictPlayerPosition()
    {
        if (playerRigidbody != null)
        {
            // 使用玩家当前速度和预测时间来计算未来位置
            Vector3 playerVelocity = playerRigidbody.velocity;
            return playerTransform.position + playerVelocity * predictionTime;
        }
        else
        {
            // 如果没有 Rigidbody，直接返回玩家当前位置
            return playerTransform.position;
        }
    }

    // 可视化检测范围（仅编辑器中显示）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
