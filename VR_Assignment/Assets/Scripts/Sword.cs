using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Sword : MonoBehaviour
{
    public float baseDamage = 10.0f;
    public MeshCollider meshCollider;
    private XRGrabInteractable grabInteractable;
    private bool isHeld;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEntered.AddListener(OnGrab);
        grabInteractable.onSelectExited.AddListener(OnRelease);
        meshCollider = GetComponent<MeshCollider>();
        isHeld = false;
    }

    void OnGrab(XRBaseInteractor interactor)
    {
        isHeld = true;
        meshCollider.isTrigger = true;
    }

    void OnRelease(XRBaseInteractor interactor)
    {
        isHeld = false;
        meshCollider.isTrigger = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isHeld)
            return;

        //Check if sword hit the enemy
        if(other.CompareTag("Enemy") || other.CompareTag("Head") || 
            other.CompareTag("Torso") || other.CompareTag("Limb"))
        {
            Debug.Log("Sword hit: " + other.gameObject.tag);
            Health health = other.gameObject.GetComponentInParent<Health>();

            if (health != null)
            {
                string bodyPartTag = other.gameObject.tag;
                //Debug.Log(bodyPartTag);
                health.TakeDamage(baseDamage, bodyPartTag);
            }
        }
    }
}
