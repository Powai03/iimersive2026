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
        audioSource.clip = sonPas;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        
        dernierePosition = transform.position;
    }

    void Update()
    {
        float distanceDeplacement = Vector3.Distance(transform.position, dernierePosition);
        float vitesseActuelle = distanceDeplacement / Time.deltaTime;

        if (vitesseActuelle > seuilVitesse)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); 
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop(); 
            }
        }

        dernierePosition = transform.position;
    }
}