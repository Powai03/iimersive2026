using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BoutonQuest : MonoBehaviour
{
    [HideInInspector] public SerpentQuestManager manager;
    private XRGrabInteractable grabable;

    void Awake()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) box = gameObject.AddComponent<BoxCollider>();
        box.isTrigger = false; 

        grabable = GetComponent<XRGrabInteractable>();
        if (grabable == null) grabable = gameObject.AddComponent<XRGrabInteractable>();

        grabable.trackPosition = false;
        grabable.trackRotation = false;
        grabable.throwOnDetach = false;

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