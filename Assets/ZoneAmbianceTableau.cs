using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ZoneAmbianceTableau : MonoBehaviour
{
    [Header("Musique / Ambiance du Tableau")]
    public AudioClip musiqueTableau;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musiqueTableau;
        audioSource.loop = true;     
        audioSource.playOnAwake = false;
        
        audioSource.spatialBlend = 1.0f; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") || other.GetComponentInParent<BruitDePasVR>() != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log($"[Audio] Entrée dans la zone de : {gameObject.name}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") || other.GetComponentInParent<BruitDePasVR>() != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log($"[Audio] Sortie de la zone de : {gameObject.name}");
            }
        }
    }
}