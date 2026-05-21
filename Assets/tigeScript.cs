using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class tigeScript : MonoBehaviour
{
    [Header("Cibles de l'action")]
    public GameObject pommeEntiere;    
    public GameObject obstacleFenetre; 
    
    private XRGrabInteractable grabInteractable;
    private bool actionDeclenchee = false;

    void Start()
    {
        // Sécurité : ajoute un collider si manquant pour que le laser puisse cibler la tige
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<MeshCollider>();
        }

        // Récupère ou ajoute le XR Grab Interactable
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
        }
        
        // Écoute le moment précis où la gâchette/laser attrape l'objet
        grabInteractable.selectEntered.AddListener(OnTigeAttrapee);
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnTigeAttrapee);
    }

    private void OnTigeAttrapee(SelectEnterEventArgs args)
{
    if (actionDeclenchee) return;
    actionDeclenchee = true;

    // Coupe le script de croissance
    PommeCroissance croissance = FindFirstObjectByType<PommeCroissance>();
    if (croissance != null) croissance.enabled = false;

    // Fait disparaître la pomme
    if (pommeEntiere != null) pommeEntiere.SetActive(false);

    // Ouvre la fenêtre
    if (obstacleFenetre != null) obstacleFenetre.SetActive(false);

    // --- LIBÉRATION DE LA PHYSIQUE ---
    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = false; // Permet à l'objet de bouger physiquement
        rb.useGravity = true;   // Permet de la lancer et qu'elle retombe
    }

    // Taille normale dans la main
    transform.localScale = new Vector3(1f, 1f, 1f); 

    Debug.Log("Tige chopée ! Pomme supprimée, physique activée.");
}
}