using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BoutonQuest : MonoBehaviour
{
    [HideInInspector] public SerpentQuestManager manager;
    private XRGrabInteractable grabable;

    void Awake()
    {
        // 1. On configure le Collider exactement comme ta tige (Solide et ajusté)
        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) box = gameObject.AddComponent<BoxCollider>();
        box.isTrigger = false; 

        // 2. On récupère ou ajoute le Grab Interactable comme sur la tige
        grabable = GetComponent<XRGrabInteractable>();
        if (grabable == null) grabable = gameObject.AddComponent<XRGrabInteractable>();

        // 3. Sécurité pour pas que le bouton se décroche du serpent au clic
        grabable.trackPosition = false;
        grabable.trackRotation = false;
        grabable.throwOnDetach = false;

        // 4. On écoute le clic (SelectEntered, comme ta fonction OnTigeAttrapee)
        grabable.selectEntered.AddListener(OnBoutonClic);
    }

    void OnDestroy()
    {
        if (grabable != null)
            grabable.selectEntered.RemoveListener(OnBoutonClic);
    }

    private void OnBoutonClic(SelectEnterEventArgs args)
    {
        if (manager != null)
        {
            manager.BoutonTrouve();
        }
    }
}