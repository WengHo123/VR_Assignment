using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Sword : MonoBehaviour
{
    public float baseDamage = 10.0f;
    public float hitCooldown = 0.5f;
    private MeshCollider meshCollider;
    private XRGrabInteractable grabInteractable;
    public bool isHeld = false;

    private float lastHitTime;
    public bool IsHeld { get { return isHeld; } }

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        meshCollider = GetComponent<MeshCollider>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void Start()
    {
        isHeld = false;
        lastHitTime = -hitCooldown;
        Debug.Log("Sword Start: isHeld set to false.");
    }

    private void OnDestroy()
    {
        // Unregister listeners to avoid memory leaks
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (isHeld)
        {
            Debug.LogWarning("OnGrab called but sword is already held. Ignoring.");
            return;
        }

        // Set isHeld to true and make the collider a trigger when the sword is grabbed
        Debug.Log("Sword grabbed.");
        isHeld = true;
        meshCollider.isTrigger = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (!isHeld)
        {
            return;
        }

        Debug.Log("Sword released.");
        isHeld = false;
        meshCollider.isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isHeld)
        {
            Debug.Log("Sword is not held, ignoring collision.");
            return;
        }

        if (Time.time < lastHitTime + hitCooldown)
        {
            Debug.Log("Hit cooldown active, ignoring collision.");
            return;
        }

        // Check if sword hit the enemy
        if (other.CompareTag("Enemy") || other.CompareTag("Head") ||
            other.CompareTag("Torso") || other.CompareTag("Limb"))
        {
            Debug.Log("Sword hit: " + other.gameObject.tag);
            Health health = other.gameObject.GetComponentInParent<Health>();

            if (health != null)
            {
                string bodyPartTag = other.gameObject.tag;
                health.TakeDamage(baseDamage, bodyPartTag);
                lastHitTime = Time.time;
            }
        }
    }
}
