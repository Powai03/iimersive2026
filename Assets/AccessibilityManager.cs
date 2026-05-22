using UnityEngine;

public class AccessibilityManager : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Glisse ici le Material qui utilise le shader DaltonismeFilter")]
    public Material filtreMaterial;

    void Start()
    {
        ActiverModeNormal();
    }

    public void ActiverModeNormal() => AppliquerMode(0);
    public void ActiverProtanopie() => AppliquerMode(1);
    public void ActiverDeuteranopie() => AppliquerMode(2);
    public void ActiverTritanopie() => AppliquerMode(3);

    private void AppliquerMode(int modeId)
    {
        if (filtreMaterial != null)
        {
            filtreMaterial.SetFloat("_Mode", (float)modeId);
            Debug.Log($"[Filtre] Mode actuel changé sur : {modeId}");
        }
        else
        {
            Debug.LogError("Il manque le Material sur le script AccessibilityManager !");
        }
    }
}