using UnityEngine;
using System.Collections;

public class EnemyWeapon : MonoBehaviour
{
    public int damage = 10;
    public float attackCooldown = 1.5f; // Cooldown period in seconds
    public float attackRange = 1.5f; // Attack range in units
    private bool isOnCooldown = false;
    private int originalDamage;
    private Coroutine cooldownCoroutine;

    private void Start()
    {
        originalDamage = damage;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isOnCooldown)
            {
                if (RaycastHitShield())
                {
                    Debug.Log("Blocked by the shield!");
                    SetDamage(0); // Temporarily set damage to zero when blocked
                }
                else
                {
                    PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damage);
                    }
                }

                // Start cooldown
                if (cooldownCoroutine == null)
                {
                    cooldownCoroutine = StartCoroutine(StartCooldown());
                }
            }
        }
    }

    private bool RaycastHitShield()
    {
        Vector3 direction = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized;
        RaycastHit hit;

        // Draw the raycast line for debugging
        Debug.DrawLine(transform.position, transform.position + direction * attackRange, Color.red, 1.0f);

        if (Physics.Raycast(transform.position, direction, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Shield"))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
        cooldownCoroutine = null;
    }
}
