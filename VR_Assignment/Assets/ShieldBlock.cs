using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShieldBlock : MonoBehaviour
{
    public BoxCollider boxCollider;
    private XRGrabInteractable grabInteractable;
    private bool isHeld;
    private Dictionary<EnemyWeapon, int> originalDamageValues = new Dictionary<EnemyWeapon, int>();

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEntered.AddListener(OnGrab);
        grabInteractable.onSelectExited.AddListener(OnRelease);
        boxCollider = GetComponent<BoxCollider>();
        isHeld = false;
    }

    void OnGrab(XRBaseInteractor interactor)
    {
        isHeld = true;
        boxCollider.isTrigger = true;
    }

    void OnRelease(XRBaseInteractor interactor)
    {
        isHeld = false;
        boxCollider.isTrigger = false;

        // Ensure all weapons have their original damage restored when the shield is released
        foreach (var entry in originalDamageValues)
        {
            entry.Key.SetDamage(entry.Value);
        }
        originalDamageValues.Clear();
    }


}
