using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class VRBoss : MonoBehaviour
{
    public Animator animator;
    public Transform player; // Assign the player transform
    public float attackCooldown = 3f; // Cooldown between attacks
    public float runSpeed = 3f; // Speed at which the boss runs
    public float wanderSpeed = 1.5f; // Speed of rotation towards the player
    public float wanderRadius = 2f; // Radius around the player to wander
    public float rotationSpeed = 5f; // Speed of rotation towards the player
    public float attackRange = 1.5f; // Distance from the player within which the boss can attack
    public float raycastHeightOffset = 1.0f; // Height offset for the raycast
    public AudioSource blockAudioSource; // Reference to the AudioSource component for block sound
    public AudioSource hitAudioSource; // Reference to the AudioSource component for player hit sound

    private bool isAttacking = false;
    private bool isWandering = false;
    private bool isCoroutineRunning = false;
    private float attackCooldownTime = 0f;
    private NavMeshAgent navAgent;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (player == null)
        {
            Debug.LogError("Player transform not assigned.");
        }

        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found.");
        }
        else
        {
            navAgent.updateRotation = false;
        }


        if (blockAudioSource == null)
        {
            Debug.LogError("Block audio source not assigned.");
        }

        if (hitAudioSource == null)
        {
            Debug.LogError("Hit audio source not assigned.");
        }
    }

    void Update()
    {
        if (!isAttacking && !isWandering && !isCoroutineRunning)
        {
            CheckDistanceAndAct();
        }

        // Ensure the enemy is always facing the player when wandering or running
        if (navAgent.velocity != Vector3.zero || isAttacking || isWandering)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // Update the attack cooldown timer
        if (attackCooldownTime > 0)
        {
            attackCooldownTime -= Time.deltaTime;
        }
    }

    private void CheckDistanceAndAct()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            // Player is out of attack range, run towards the player
            StartRunning();
        }
        else if (attackCooldownTime <= 0)
        {
            // Player is in attack range and cooldown is finished, perform an attack
            StartCoroutine(PerformAttack());
        }
        else
        {
            // Player is in attack range but cooldown is not finished, wander around
            if (!isCoroutineRunning)
            {
                StartCoroutine(WanderAndCooldown());
            }
        }
    }

    private void StartRunning()
    {
        navAgent.isStopped = false;
        navAgent.speed = runSpeed;
        navAgent.SetDestination(player.position);
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsWandering", false);
    }

    private void StopRunning()
    {
        navAgent.isStopped = true;
        animator.SetBool("IsRunning", false);
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        StopRunning();
        navAgent.isStopped = true;

        // Randomly select an attack pattern
        int attackPattern = Random.Range(0, 2); // 0 or 1

        if (attackPattern == 0)
        {
            animator.SetTrigger("TriggerAttack1");
        }
        else
        {
            animator.SetTrigger("TriggerAttack2");
        }

        // Wait for the attack animation to complete
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Perform raycasting to detect if the shield or the player is hit
        if (RaycastHitShieldOrPlayer())
        {
            Debug.Log("Hit detected.");
        }



        // Start cooldown and wandering
        attackCooldownTime = attackCooldown;
        if (!isCoroutineRunning)
        {
            StartCoroutine(WanderAndCooldown());
        }

        isAttacking = false;
    }

    private bool RaycastHitShieldOrPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 raycastOrigin = transform.position + Vector3.up * raycastHeightOffset;
        RaycastHit hit;

        // Draw the raycast line for debugging
        Debug.DrawLine(raycastOrigin, raycastOrigin + direction * attackRange, Color.red, 1.0f);

        if (Physics.Raycast(raycastOrigin, direction, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Shield"))
            {
                Debug.Log("Blocked by the shield!");

                // Play block sound
                if (blockAudioSource != null)
                {
                    blockAudioSource.Play();
                }

                return true; // Hit the shield
            }
            else if (hit.collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(10); // Replace 10 with the appropriate damage value
                }
                Debug.Log("Player hit!");

                // Play hit sound
                if (hitAudioSource != null)
                {
                    hitAudioSource.Play();
                }

                return true; // Hit the player
            }
        }

        return false; // No hit
    }

    private IEnumerator WanderAndCooldown()
    {
        isCoroutineRunning = true;
        isWandering = true;
        navAgent.speed = wanderSpeed;
        animator.SetBool("IsWandering", true);

        float elapsedTime = 0f;

        while (elapsedTime < attackCooldown)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer > 10)
            {
                break;
            }

            Vector3 wanderTarget;
            int randomDirection = Random.Range(0, 3); // 0 = backward, 1 = left, 2 = right

            switch (randomDirection)
            {
                case 0: // Backward
                    wanderTarget = transform.position - transform.forward * wanderRadius;
                    break;
                case 1: // Left
                    wanderTarget = transform.position - transform.right * wanderRadius;
                    break;
                case 2: // Right
                    wanderTarget = transform.position + transform.right * wanderRadius;
                    break;
                default:
                    wanderTarget = transform.position - transform.forward * wanderRadius;
                    break;
            }

            wanderTarget.y = transform.position.y; // Keep the same height
            navAgent.SetDestination(wanderTarget);
            navAgent.isStopped = false;

            yield return new WaitForSeconds(1f);

            elapsedTime += 1f;
        }

        navAgent.isStopped = true;
        animator.SetBool("IsWandering", false);
        isWandering = false;
        isCoroutineRunning = false;

        CheckDistanceAndAct();
    }
}
