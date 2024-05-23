using UnityEngine;

public class SlashSound : MonoBehaviour
{
    public AudioClip[] slashSounds;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing");
        }
    }

    public void PlayRandomSlashSound()
    {
        if (slashSounds.Length == 0)
        {
            Debug.LogWarning("No slash sounds assigned.");
            return;
        }

        //Play random slash sound
        int randomIndex = Random.Range(0, slashSounds.Length);
        AudioClip randomSlashSound = slashSounds[randomIndex];
        audioSource.PlayOneShot(randomSlashSound);
    }
}
