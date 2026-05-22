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

        var rightHandDevices = new System.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, rightHandDevices);
        
        if (rightHandDevices.Count > 0)
        {
            rightHandDevices[0].TryGetFeatureValue(CommonUsages.primaryButton, out boutonAPresse);
        }

        var leftHandDevices = new System.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, leftHandDevices);
        
        if (leftHandDevices.Count > 0)
        {
            leftHandDevices[0].TryGetFeatureValue(CommonUsages.primaryButton, out boutonXPresse);
        }

        if (boutonAPresse && boutonXPresse)
        {
            DeclencherReset();
        }
    }

    void DeclencherReset()
    {
        Debug.Log("[Urgence] Reset des filtres et retour au point de départ !");

        if (filtreMaterial != null)
        {
            filtreMaterial.SetFloat("_Mode", 0f);
        }

        if (pointDeDepart != null)
        {
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false; 
            transform.position = pointDeDepart.position;
            transform.rotation = pointDeDepart.rotation;

            if (cc != null) cc.enabled = true;
        }
    }
}