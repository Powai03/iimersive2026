using UnityEngine;
using System.Collections.Generic;

public class ZoneYayoiDots : MonoBehaviour
{
    [Header("Configuration des Taches")]
    public int nombreMaxDots = 5; // Nombre de points simultanés à l'écran
    public float dureePulsation = 2f; // Temps pour faire 0 -> 1 -> 0 en opacité

    private Transform cameraJoueur;
    private List<GameObject> activeDots = new List<GameObject>();
    private bool joueurDansLaZone = false;
    private Texture2D rondNoirTexture;

    void Start()
    {
        // Sécurité : Vérifie si le collider est bien un Trigger
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;

        // Création dynamique d'une texture de rond noir smooth pour éviter d'avoir à importer un PNG
        CreerTextureRondNoir();
    }

    void OnTriggerEnter(Collider other)
    {
        // Dès que le joueur (XR Origin / Main Camera) entre
        if (other.CompareTag("MainCamera") || other.GetComponentInChildren<Camera>() != null)
        {
            cameraJoueur = other.transform;
            joueurDansLaZone = true;
            
            // Lance la génération des points
            for (int i = 0; i < nombreMaxDots; i++)
            {
                GererNouveauDot();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == cameraJoueur || other.CompareTag("MainCamera"))
        {
            joueurDansLaZone = false;
            // Nettoie les points quand on sort
            foreach (var dot in activeDots) Destroy(dot);
            activeDots.Clear();
        }
    }

    void GererNouveauDot()
    {
        if (!joueurDansLaZone || cameraJoueur == null) return;

        // 1. Création d'un Quad pour afficher le point
        GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Destroy(dot.GetComponent<MeshCollider>()); // Pas de physique sur le point

        // 2. Application de la texture noire transparente
        MeshRenderer renderer = dot.GetComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        renderer.material.SetTexture("_BaseMap", rondNoirTexture);
        renderer.material.SetFloat("_Surface", 1); // Mode Transparent
        renderer.material.SetFloat("_Blend", 0); // Alpha Blend
        renderer.material.renderQueue = 3000; // S'affiche par-dessus le décor

        // 3. Positionnement aléatoire dans le champ de vision (devant la caméra)
        float distanceDuVisage = Random.Range(0.5f, 1.2f); // Pas trop près pour pas que ça louche
        float offsetX = Random.Range(-0.4f, 0.4f);
        float offsetY = Random.Range(-0.3f, 0.3f);
        
        dot.transform.SetParent(cameraJoueur);
        dot.transform.localPosition = new Vector3(offsetX, offsetY, distanceDuVisage);
        dot.transform.localRotation = Quaternion.identity;

        // 4. Taille aléatoire
        float taille = Random.Range(0.05f, 0.25f);
        dot.transform.localScale = new Vector3(taille, taille, 1f);

        activeDots.Add(dot);

        // 5. Lancement de la routine de pulsation (Fade In / Fade Out)
        StartCoroutine(RoutinePulsation(dot, renderer.material));
    }

    System.Collections.IEnumerator RoutinePulsation(GameObject dot, Material mat)
    {
        float temps = 0f;
        Color col = Color.black;

        // Phase 1 : Opacité 0 vers 1 (Montée)
        while (temps < dureePulsation / 2f && dot != null)
        {
            temps += Time.deltaTime;
            col.a = Mathf.Lerp(0f, 0.85f, temps / (dureePulsation / 2f)); // Max 85% pour pas aveugler totalement
            mat.SetColor("_BaseColor", col);
            yield return null;
        }

        // Phase 2 : Opacité 1 vers 0 (Descente)
        temps = 0f;
        while (temps < dureePulsation / 2f && dot != null)
        {
            temps += Time.deltaTime;
            col.a = Mathf.Lerp(0.85f, 0f, temps / (dureePulsation / 2f));
            mat.SetColor("_BaseColor", col);
            yield return null;
        }

        // Destruction et remplacement
        if (dot != null)
        {
            activeDots.Remove(dot);
            Destroy(dot);
        }

        // Si le joueur est encore là, on en repop un nouveau à une autre position !
        if (joueurDansLaZone)
        {
            GererNouveauDot();
        }
    }

    void CreerTextureRondNoir()
    {
        int size = 128;
        rondNoirTexture = new Texture2D(size, size);
        float center = size / 2f;
        float radius = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                if (dist < radius)
                {
                    // Effet de dégradé sur les bords du point pour qu'il soit smooth
                    float alpha = Mathf.Clamp01((radius - dist) / 4f);
                    rondNoirTexture.SetPixel(x, y, new Color(0, 0, 0, alpha));
                }
                else
                {
                    rondNoirTexture.SetPixel(x, y, Color.clear);
                }
            }
        }
        rondNoirTexture.Apply();
    }
}