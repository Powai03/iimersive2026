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
        Vector3 vecteurAxe = Vector3.forward; // Par défaut Z

        // On sélectionne le vrai axe local de l'objet
        if (axeChoisi == AxeDeRotation.AxeDuCorps_Y)
        {
            vecteurAxe = Vector3.up; // Axe Y (souvent la hauteur du cylindre de base)
        }
        else if (axeChoisi == AxeDeRotation.AxeDuCorps_X)
        {
            vecteurAxe = Vector3.right; // Axe X
        }

        // Rotation en Space.Self pour que ça suive l'inclinaison de l'objet
        transform.Rotate(vecteurAxe * vitesseRotation * Time.deltaTime, Space.Self);
    }
}