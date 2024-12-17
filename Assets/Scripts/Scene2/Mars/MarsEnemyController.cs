using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsEnemyController : MonoBehaviour
{
    public int damageAmount = 20; 
    public float detectionRangePlayer = 15f; 
    public float detectionRangeEnemy = 20f;
    public float stopDistance = 3f; 
    public float chaseSpeedPlayer = 5f; 
    public float chaseSpeedPlayerAggressive = 7f; 
    public float wanderSpeed = 2f; 

    private Transform player;
    private Transform otherEnemy;
    private bool isChasingPlayer = false;
    private bool isAttackingEnemy = false;

    private Animator anim;

    private Vector3 wanderDirection;
    private float wanderTimer = 0f;
    private float wanderChangeInterval = 2f; 
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
        }
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator not found on enemy!");
        }
    }

    void Update()
    {
        
        if (player != null && Vector3.Distance(transform.position, player.position) > 30f)
        {
            isChasingPlayer = false;
            anim.SetBool("Wander", true);
            anim.SetBool("Attack", false);
            Debug.Log($"{gameObject.name} stopped chasing player, too far away!");
        }

        if (isChasingPlayer && player != null)
        {
            ChasePlayer(isAttackingEnemy ? chaseSpeedPlayerAggressive : chaseSpeedPlayer);
        }
        else if (isAttackingEnemy && otherEnemy != null)
        {
            AttackOtherEnemy();
        }
        else
        {
            anim.SetBool("Wander", true);
            anim.SetBool("Attack", false);
            WanderRandomly();
            DetectPlayerOrEnemy();
        }
    }


    void DetectPlayerOrEnemy()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRangePlayer)
        {
            isChasingPlayer = true;
            Debug.Log($"Player detected by {gameObject.name}, starting chase!");
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRangeEnemy);
        foreach (var hit in hitColliders)
        {
            if ((CompareTag("EnemyA") && hit.CompareTag("EnemyB")) || 
                (CompareTag("EnemyB") && hit.CompareTag("EnemyA")))
            {
                otherEnemy = hit.transform;
                isAttackingEnemy = true;
                Debug.Log($"{gameObject.name} detected {otherEnemy.name}, starting attack!");
                break;
            }
        }
    }

    void ChasePlayer(float speed)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        MoveOnPlanetSurface(direction, speed);
        anim.SetBool("Attack", true);
        anim.SetBool("Wander", false);
        transform.LookAt(player); 
    }

    void AttackOtherEnemy()
    {
        if (Vector3.Distance(transform.position, otherEnemy.position) > stopDistance)
        {
            Vector3 direction = (otherEnemy.position - transform.position).normalized;
            Vector3 targetPosition = transform.position + direction * wanderSpeed * Time.deltaTime;

            Vector3 planetCenter = Vector3.zero; 
            Vector3 surfacePosition = (targetPosition - planetCenter).normalized * Vector3.Distance(transform.position, planetCenter);
            transform.position = surfacePosition;
            transform.LookAt(surfacePosition + direction); 
        }
        else
        {
            // if (anim != null)
            // {
            anim.SetBool("Attack", true);
            anim.SetBool("Wander", false);
            // anim.SetTrigger("Attack");
            // }
            Debug.Log($"{gameObject.name} is attacking {otherEnemy.name}");
        }
    }

    void WanderRandomly()
    {
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderChangeInterval)
        {
            SetRandomWanderDirection();
            wanderTimer = 0f;
        }

        MoveOnPlanetSurface(wanderDirection, wanderSpeed);
        transform.LookAt(transform.position + wanderDirection);
    }

    void SetRandomWanderDirection()
    {
        wanderDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }
    void MoveOnPlanetSurface(Vector3 direction, float speed)
    {
        Vector3 targetPosition = transform.position + direction * speed * Time.deltaTime;

        Vector3 planetCenter = Vector3.zero; 
        Vector3 surfacePosition = (targetPosition - planetCenter).normalized * Vector3.Distance(transform.position, planetCenter);
        transform.position = surfacePosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player collided with {gameObject.name}");

            HealthManager healthManager = FindObjectOfType<HealthManager>();
            if (healthManager != null)
            {
                healthManager.ReduceHealth(damageAmount);
                Debug.Log($"Player took {damageAmount} damage from {gameObject.name}");
            }
            else
            {
                Debug.LogWarning("No HealthManager found!");
            }

            Destroy(gameObject);
        }
    }
}
