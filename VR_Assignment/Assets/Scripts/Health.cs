using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    Ragdoll ragdoll;
    public float destructionDelay = 5.0f;
    private bool isDead = false;
    private bool isAlive = true;

    public bool IsDead { get { return isDead; } }

    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, string bodyPart)
    {
        float actualDamage = damage;

        // Adjust damage based on the body part
        switch (bodyPart)
        {
            case "Head":
                actualDamage = damage * 2.0f; // Double damage for headshots
                break;
            case "Torso":
                actualDamage = damage; // Normal damage for torso
                break;
            case "Limb":
                actualDamage = damage * 0.5f; // Half damage for limbs
                break;
            default:
                actualDamage = damage; // Default damage
                break;
        }

        Debug.Log("Damage: " + actualDamage);
        currentHealth -= actualDamage;
        Debug.Log("Enemy Health: " + currentHealth);
        if(currentHealth <= 0.0f)
        {
            EnemyDie();
        }
    }

    private void EnemyDie()
    {
        do
        {
            Debug.Log("Enemy Died");
            isDead = true;
            ragdoll.ActivateRagdoll();

            //disable navMeshAgent
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = false;
            }
            Destroy(gameObject, destructionDelay);
            
        } while(!isActiveAndEnabled && !isDead);
    }

    public void PlayerTakeDamage(float damage)
    {
        Debug.Log("Player take damage");
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);
        if (currentHealth <= 0.0f)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        do
        {
            isAlive = false;

            CharacterController characterController = GetComponent<CharacterController>();
            if(characterController != null)
            {
                characterController.enabled = false;
            }
            Debug.Log("Game Over!");
        } while (isAlive);
        
    }
}
