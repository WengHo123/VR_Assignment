using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by the sword!");
        }
    }
}
