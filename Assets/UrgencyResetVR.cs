using UnityEngine;
using UnityEngine.XR;

public class UrgencyResetVR : MonoBehaviour
{
    [Header("Matériau du Filtre")]
    public Material filtreMaterial;

    [Header("Point de Départ")]
    [Tooltip("Glisse ici l'objet vide (SpawnPoint) où le joueur doit réapparaître")]
    public Transform pointDeDepart;

    void Update()
    {
        bool boutonAPresse = false;
        bool boutonXPresse = false;

        // 1. On check la manette DROITE pour le bouton A
        var rightHandDevices = new System.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, rightHandDevices);
        
        if (rightHandDevices.Count > 0)
        {
            // primaryButton correspond au bouton A sur Oculus/Meta Quest
            rightHandDevices[0].TryGetFeatureValue(CommonUsages.primaryButton, out boutonAPresse);
        }

        // 2. On check la manette GAUCHE pour le bouton X
        var leftHandDevices = new System.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, leftHandDevices);
        
        if (leftHandDevices.Count > 0)
        {
            // primaryButton correspond au bouton X sur la manette gauche
            leftHandDevices[0].TryGetFeatureValue(CommonUsages.primaryButton, out boutonXPresse);
        }

        // 3. Si les deux sont enfoncés en même temps -> RESET !
        if (boutonAPresse && boutonXPresse)
        {
            DeclencherReset();
        }
    }

    void DeclencherReset()
    {
        Debug.Log("[Urgence] Reset des filtres et retour au point de départ !");

        // Reset du filtre en mode Normal (0)
        if (filtreMaterial != null)
        {
            filtreMaterial.SetFloat("_Mode", 0f);
        }

        // Téléportation du joueur au Spawn Point
        if (pointDeDepart != null)
        {
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false; // Désactive le CC temporairement pour éviter les bugs physiques

            transform.position = pointDeDepart.position;
            transform.rotation = pointDeDepart.rotation;

            if (cc != null) cc.enabled = true;
        }
    }
}