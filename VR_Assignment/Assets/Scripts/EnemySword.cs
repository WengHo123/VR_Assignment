using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    public float damage = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            Health health = other.gameObject.GetComponentInParent<Health>();

            if (health != null)
            {
                health.PlayerTakeDamage(damage);
            }
        }
    }
}
