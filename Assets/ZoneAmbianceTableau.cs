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
        audioSource.loop = true;      // Le son boucle tant qu'on reste dedans
        audioSource.playOnAwake = false;
        
        // Optionnel : configure le son en 3D pour qu'il vienne du tableau
        audioSource.spatialBlend = 1.0f; 
    }

    // Dès que le joueur (ou sa tête/main) entre dans la zone
    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si c'est bien le joueur VR qui entre dans la zone
        if (other.CompareTag("MainCamera") || other.GetComponentInParent<BruitDePasVR>() != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log($"[Audio] Entrée dans la zone de : {gameObject.name}");
            }
        }
    }

    // Dès que le joueur sort de la zone
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