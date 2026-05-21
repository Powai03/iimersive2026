using UnityEngine;

public class PommeCroissance : MonoBehaviour
{
    [Header("Objets a controler")]
    public Transform pomme; 
    public Transform tige;  
    
    [Header("Reglages")]
    public float vitesseCroissance = 1.0f;

    private Vector3 pommeScaleMax;
    private Vector3 tigeScaleMax;
    private Vector3 pommeScaleInitiale;
    private Vector3 tigeScaleInitiale;
    private Vector3 positionInitialeTigeRelative; 

    private bool joueurDansLaSalle = false;
    private Vector3 dernierePositionJoueur;
    private Transform transformJoueur;
    private float progression = 0f;
    private MeshCollider pommeCollider;

    void Start()
    {
        if (pomme != null && tige != null)
        {
            pommeScaleMax = pomme.localScale;
            tigeScaleMax = tige.localScale;

            pommeScaleInitiale = pommeScaleMax * 0.05f;
            tigeScaleInitiale = tigeScaleMax * 0.05f;

            // Enregistre l'emplacement exact de la tige par rapport au centre de la pomme
            positionInitialeTigeRelative = pomme.InverseTransformPoint(tige.position);

            pomme.localScale = pommeScaleInitiale;
            tige.localScale = tigeScaleInitiale;

            pommeCollider = pomme.GetComponent<MeshCollider>();
            if (pommeCollider == null) pommeCollider = pomme.gameObject.AddComponent<MeshCollider>();
            pommeCollider.convex = false; 
            pommeCollider.isTrigger = false; 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            joueurDansLaSalle = true;
            transformJoueur = other.transform;
            dernierePositionJoueur = transformJoueur.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            joueurDansLaSalle = false;
        }
    }

    void Update()
    {
        if (pommeCollider != null) pommeCollider.enabled = joueurDansLaSalle;

        if (pomme != null && tige != null)
        {
            // --- 1. CALCUL DE LA CROISSANCE (Uniquement si le joueur bouge dans la salle) ---
            if (joueurDansLaSalle && progression < 1f)
            {
                float distanceDeplacee = Vector3.Distance(transformJoueur.position, dernierePositionJoueur);

                if (distanceDeplacee > 0.001f)
                {
                    progression += (distanceDeplacee * vitesseCroissance) / 10f;
                    progression = Mathf.Clamp01(progression);

                    // On fait grandir la pomme et la tige
                    pomme.localScale = Vector3.Lerp(pommeScaleInitiale, pommeScaleMax, progression);
                    tige.localScale = Vector3.Lerp(tigeScaleInitiale, tigeScaleMax, progression);
                }

                dernierePositionJoueur = transformJoueur.position;
            }

            // --- 2. REPOSITIONNEMENT DE LA TIGE (En permanence, même à l'arrêt) ---
            // Cette ligne s'exécute TOUT LE TEMPS pour forcer la tige à suivre la surface de la pomme
            tige.position = pomme.TransformPoint(positionInitialeTigeRelative);
        }
    }
}