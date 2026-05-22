using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; 

public class BoutonFiltreDaltonisme : MonoBehaviour
{
    [Header("Réglage du Filtre")]
    [Tooltip("0 = Normal, 1 = Protanopie, 2 = Deuteranopie, 3 = Tritanopie")]
    public int modeId = 0;

    [Header("Matériau du Filtre")]
    public Material filtreMaterial;

    private XRGrabInteractable grabable;

    void Awake()
    {
        
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = false; 

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        grabable = GetComponent<XRGrabInteractable>();
        if (grabable == null) grabable = gameObject.AddComponent<XRGrabInteractable>();

        grabable.trackPosition = false;
        grabable.trackRotation = false;
        grabable.throwOnDetach = false;

        grabable.selectEntered.AddListener(OnBoutonGrabbed);
    }

    void OnDestroy()
    {
        if (grabable != null)
            grabable.selectEntered.RemoveListener(OnBoutonGrabbed);
    }

    private void OnBoutonGrabbed(SelectEnterEventArgs args)
    {
        if (filtreMaterial != null)
        {
            filtreMaterial.SetFloat("_Mode", (float)modeId);
            Debug.Log($"[Grab VR Auto] Filtre changé sur le mode : {modeId} via {gameObject.name}");
        }
        else
        {
            Debug.LogError($"Il manque le matériel de filtre sur : {gameObject.name}");
        }
    }
}