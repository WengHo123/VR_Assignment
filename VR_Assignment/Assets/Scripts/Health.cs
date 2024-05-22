using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    Ragdoll ragdoll;

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
        Debug.Log("Current Health: " + currentHealth);
        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        ragdoll.ActivateRagdoll();
    }

    public void PlayerTakeDamage(float damage)
    {
        Debug.Log("Player take damage");
        currentHealth -= damage;
        Debug.Log("Current Health: " + currentHealth);
        if (currentHealth <= 0.0f)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
    }
}
