using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;
using UnityEngine.Rendering;

public class EnemySword : MonoBehaviour
{
    public float damage = 10.0f;
    public float hitCooldown = 0.5f;
    public float blockCooldown = 0.5f;
    private float lastHitTime;
    public Animator enemyAnimator;
    public AudioClip blockSound;
    private AudioSource audioSource;
    public AILocomotion aiLocomotion;
    public Sword playerSword;
    private bool isBlock = false;
    public bool IsBlock { get { return isBlock; } }

    private void Start()
    {
        lastHitTime = -hitCooldown;
        enemyAnimator = GetComponentInParent<Animator>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Time.time < lastHitTime + hitCooldown + blockCooldown)
            return;

        if (other.CompareTag("PlayerSword") && playerSword.IsHeld)
        {
            Debug.Log("Attack blocked by player!");
            isBlock = true;

            // Play block sound effect
            if (blockSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(blockSound);
            }

            aiLocomotion.ApplyBlockedCooldown();

            lastHitTime = Time.time;
        }
        else if(other.CompareTag("Player"))
        {
            isBlock = false;
            CapsuleCollider playerCapsule = other.GetComponent<CapsuleCollider>();
            if (playerCapsule != null)
            {
                Debug.Log("Player hit via CapsuleCollider");
                Health health = other.gameObject.GetComponentInParent<Health>();

                if (health != null && aiLocomotion.IsAttacking == true)
                {
                    health.PlayerTakeDamage(damage);
                    lastHitTime = Time.time;
                }
            }
        }
    }
}
