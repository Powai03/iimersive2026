using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SerpentQuestManager : MonoBehaviour
{
    [Header("Configuration des Boutons")]
    public List<GameObject> listeBoutons; 
    public int boutonsATrouver = 3;

    [Header("Téléportation de Fin")]
    public GameObject xrOrigin; 
    public Transform destinationFinale; 

    [Header("Vibrations (Haptiques)")]
    public XRBaseInputInteractor manette; 
    public float distanceMaxFroid = 15f;
    public float distanceMinChaud = 1.5f;

    private GameObject boutonActuel;
    private int scoreActuel = 0;
    private float timerVibration = 0f;
    private List<GameObject> boutonsDisponibles;
    private bool queteCommencee = false;
    private XRInteractionManager managerGlobal;

    void Start()
    {
        boutonsDisponibles = new List<GameObject>(listeBoutons);
        
        // On va chercher le gestionnaire XR de la scène
        managerGlobal = Object.FindAnyObjectByType<XRInteractionManager>();

        foreach (var b in listeBoutons) 
        {
            BoutonQuest scriptBouton = b.GetComponent<BoutonQuest>();
            if (scriptBouton != null) scriptBouton.manager = this;
            
            b.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!queteCommencee && (other.CompareTag("Player") || other.name.Contains("XR Origin")))
        {
            Debug.Log("Pièce 2 : Quête lancée !");
            queteCommencee = true;
            ChoisirProchainBouton();
        }
    }

    void Update()
    {
        if (!queteCommencee || boutonActuel == null || manette == null) return;

        float distance = Vector3.Distance(manette.transform.position, boutonActuel.transform.position);
        float t = Mathf.InverseLerp(distanceMinChaud, distanceMaxFroid, distance);
        float intervalle = Mathf.Lerp(0.2f, 1.0f, t);

        timerVibration += Time.deltaTime;
        if (timerVibration >= intervalle)
        {
            manette.SendHapticImpulse(0.4f, 0.1f);
            timerVibration = 0f;
        }
    }

    void ChoisirProchainBouton()
    {
        if (boutonsDisponibles.Count == 0 || scoreActuel >= boutonsATrouver)
        {
            GagnerEtTeleporter();
            return;
        }

        int indexAleatoire = Random.Range(0, boutonsDisponibles.Count);
        boutonActuel = boutonsDisponibles[indexAleatoire];
        
        // 1. On active le bouton
        boutonActuel.SetActive(true);

        // 2. FORCE REGISTER : On force Unity à lier le bouton au système XR immédiatement
        XRGrabInteractable grab = boutonActuel.GetComponent<XRGrabInteractable>();
        if (grab != null && managerGlobal != null)
        {
            managerGlobal.RegisterInteractable(grab as IXRInteractable);
        }

        boutonsDisponibles.RemoveAt(indexAleatoire);
    }

    public void BoutonTrouve()
    {
        // 3. UNREGISTER : On retire le bouton du système XR avant de l'éteindre
        XRGrabInteractable grab = boutonActuel.GetComponent<XRGrabInteractable>();
        if (grab != null && managerGlobal != null)
        {
            managerGlobal.UnregisterInteractable(grab as IXRInteractable);
        }

        boutonActuel.SetActive(false);
        scoreActuel++;
        ChoisirProchainBouton();
    }

    void GagnerEtTeleporter()
    {
        if (xrOrigin != null && destinationFinale != null)
        {
            xrOrigin.transform.position = destinationFinale.position;
            xrOrigin.transform.rotation = destinationFinale.rotation;
        }
    }
}