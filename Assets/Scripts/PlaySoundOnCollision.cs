using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip collisionClip; // Audio clip to play

    [Header("Pitch Settings")]
    [Range(0.1f, 3f)] public float minPitch = 0.8f; // Minimum pitch value
    [Range(0.1f, 3f)] public float maxPitch = 1.2f; // Maximum pitch value

    private void Reset()
    {
        // Automatically adds an AudioSource component if not already attached
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (audioSource != null && collisionClip != null)
        {
            // Ensure the pitch is always different
            float newPitch;
            do
            {
                newPitch = Random.Range(minPitch, maxPitch);
            } while (Mathf.Approximately(newPitch, audioSource.pitch));

            audioSource.pitch = newPitch;
            audioSource.PlayOneShot(collisionClip);
        }
    }
   
}
