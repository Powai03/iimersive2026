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
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<MeshCollider>();
        }

        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
        }
        
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

    PommeCroissance croissance = FindFirstObjectByType<PommeCroissance>();
    if (croissance != null) croissance.enabled = false;

    if (pommeEntiere != null) pommeEntiere.SetActive(false);

    if (obstacleFenetre != null) obstacleFenetre.SetActive(false);

    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = false; 
        rb.useGravity = true; 
    }

    transform.localScale = new Vector3(1f, 1f, 1f); 

    Debug.Log("Tige chopée ! Pomme supprimée, physique activée.");
}
}