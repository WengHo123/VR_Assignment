using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySword : MonoBehaviour
{
    public float damage = 10.0f;
    public float hitCooldown = 0.5f;
    private float lastHitTime;
    public Animator enemyAnimator;
    public ParticleSystem blockEffect;
    public AudioClip blockSound;
    private AudioSource audioSource;
    public AILocomotion aiLocomotion;
    public Sword playerSword;

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
        if (Time.time < lastHitTime + hitCooldown)
            return;

        if (other.CompareTag("PlayerSword") && playerSword.IsHeld)
        {
            Debug.Log("Attack blocked by player!");
            if (blockEffect != null)
            {
                ParticleSystem effect = Instantiate(blockEffect, transform.position, Quaternion.identity);
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration);
            }

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
            CapsuleCollider playerCapsule = other.GetComponent<CapsuleCollider>();
            if (playerCapsule != null)
            {
                Debug.Log("Player hit via CapsuleCollider");
                Health health = other.gameObject.GetComponentInParent<Health>();

                if (health != null)
                {
                    health.PlayerTakeDamage(damage);
                    lastHitTime = Time.time;
                }
            }
        }
    }
}
