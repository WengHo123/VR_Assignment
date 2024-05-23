using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    public Transform playerTransform;
    public float attackCooldown = 3.0f;
    public float blockedCooldown = 1.0f; // Cooldown after a blocked attack
    public float rotationSpeed = 5.0f;
    public float attackRange = 2.0f;

    private bool isAttacking = false;
    private float attackCooldownTime = 0.0f;
    private float blockedCooldownTime = 0.0f; // Time remaining for blocked cooldown
    private bool isCoroutineRunning = false;

    NavMeshAgent agent;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking && !isCoroutineRunning)
        {
            CheckDistanceAndAttack();
        }

        if (agent.velocity != Vector3.zero || isAttacking)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        if (attackCooldownTime > 0)
        {
            attackCooldownTime -= Time.deltaTime;
        }

        if (blockedCooldownTime > 0)
        {
            blockedCooldownTime -= Time.deltaTime;
        }
    }

    private void CheckDistanceAndAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > attackRange)
        {
            StartRunning();
        }
        else if (attackCooldownTime <= 0 && blockedCooldownTime <= 0)
        {
            // Player is in attack range and cooldowns are finished, perform an attack
            StartCoroutine(PerformAttack());
        }
    }

    private void StartRunning()
    {
        agent.isStopped = false;
        agent.destination = playerTransform.position;

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void StopRunning()
    {
        agent.isStopped = true;
        animator.SetFloat("Speed", 0);
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        StopRunning();
        isCoroutineRunning = true;

        // Randomly select an attack pattern
        int attackPattern = Random.Range(0, 2); // 0 or 1

        if (attackPattern == 0)
        {
            animator.SetTrigger("inwardSlashTrigger");
        }
        else
        {
            animator.SetTrigger("outwardSlashTrigger");
        }

        // Wait for the attack animation to complete
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Start attack cooldown
        attackCooldownTime = attackCooldown;

        isAttacking = false;
        isCoroutineRunning = false;
    }

    // Function to apply blocked cooldown
    public void ApplyBlockedCooldown()
    {
        blockedCooldownTime = blockedCooldown;
    }
}
