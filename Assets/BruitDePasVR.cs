using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BruitDePasVR : MonoBehaviour
{
    [Header("Son des pas")]
    public AudioClip sonPas;
    
    [Header("Réglages")]
    [Tooltip("Vitesse minimale pour déclencher le bruit de pas")]
    public float seuilVitesse = 0.1f;

    private AudioSource audioSource;
    private Vector3 dernierePosition;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        // On configure l'AudioSource pour qu'il puisse tourner en boucle
        audioSource.clip = sonPas;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        
        dernierePosition = transform.position;
    }

    void Update()
    {
        // On calcule la distance parcourue depuis la frame précédente
        float distanceDeplacement = Vector3.Distance(transform.position, dernierePosition);
        float vitesseActuelle = distanceDeplacement / Time.deltaTime;

        // Si le joueur bouge assez vite
        if (vitesseActuelle > seuilVitesse)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // On lance le bruit de pas en boucle
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop(); // On coupe si on fait du surplace
            }
        }

        // On sauvegarde la position pour la frame suivante
        dernierePosition = transform.position;
    }
}