using UnityEngine;

public class RotationBroche : MonoBehaviour
{
    public enum AxeDeRotation { AxeDuCorps_Z, AxeDuCorps_Y, AxeDuCorps_X }

    [Header("Configuration de la Rotation")]
    [Tooltip("Choisis l'axe qui traverse ton objet de bout en bout")]
    public AxeDeRotation axeChoisi = AxeDeRotation.AxeDuCorps_Z;

    [Tooltip("Vitesse de rotation (positive ou négative pour changer de sens)")]
    public float vitesseRotation = 30f;

    void Update()
    {
        Vector3 vecteurAxe = Vector3.forward; 

        if (axeChoisi == AxeDeRotation.AxeDuCorps_Y)
        {
            vecteurAxe = Vector3.up; 
        }
        else if (axeChoisi == AxeDeRotation.AxeDuCorps_X)
        {
            vecteurAxe = Vector3.right; 
        }

        transform.Rotate(vecteurAxe * vitesseRotation * Time.deltaTime, Space.Self);
    }
}