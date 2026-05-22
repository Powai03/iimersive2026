using UnityEngine;
using System.Collections.Generic;

public class ZoneYayoiDots : MonoBehaviour
{
    [Header("Configuration des Taches")]
    public int nombreMaxDots = 10; 
    public float dureePulsation = 3f; 

    private Transform cameraJoueur;
    private List<GameObject> activeDots = new List<GameObject>();
    private bool joueurDansLaZone = false;
    private Texture2D rondNoirTexture;
    private BoxCollider zoneCollider;

    void Start()
    {
        zoneCollider = GetComponent<BoxCollider>();
        if (zoneCollider != null) zoneCollider.isTrigger = true;

        CreerTextureRondNoir();
    }

    void Update()
    {
        if (joueurDansLaZone && cameraJoueur != null && activeDots.Count > 0)
        {
            foreach (GameObject dot in activeDots)
            {
                if (dot != null)
                {
                    dot.transform.LookAt(cameraJoueur.position);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") || other.GetComponentInChildren<Camera>() != null)
        {
            cameraJoueur = other.CompareTag("MainCamera") ? other.transform : other.GetComponentInChildren<Camera>().transform;
            joueurDansLaZone = true;
            
            for (int i = 0; i < nombreMaxDots; i++)
            {
                GererNouveauDot();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") || (cameraJoueur != null && other.transform == cameraJoueur))
        {
            joueurDansLaZone = false;
            foreach (var dot in activeDots) Destroy(dot);
            activeDots.Clear();
        }
    }

    void GererNouveauDot()
    {
        if (!joueurDansLaZone || zoneCollider == null) return;

        GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Destroy(dot.GetComponent<MeshCollider>()); 

        MeshRenderer renderer = dot.GetComponent<MeshRenderer>();
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        
        mat.SetFloat("_Surface", 1f); 
        mat.SetFloat("_Blend", 0f);  
        mat.SetTexture("_BaseMap", rondNoirTexture);
        mat.SetColor("_BaseColor", new Color(0, 0, 0, 0)); 
        renderer.material = mat;

        Bounds bounds = zoneCollider.bounds;
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        dot.transform.position = new Vector3(randomX, randomY, randomZ);
        
        dot.transform.SetParent(this.transform);

        float taille = Random.Range(0.2f, 0.6f); 
        dot.transform.localScale = new Vector3(taille, taille, 1f);

        activeDots.Add(dot);

        StartCoroutine(RoutinePulsation(dot, mat));
    }

    System.Collections.IEnumerator RoutinePulsation(GameObject dot, Material mat)
    {
        float temps = 0f;
        Color col = Color.black;

        while (temps < dureePulsation / 2f && dot != null)
        {
            temps += Time.deltaTime;
            col.a = Mathf.Lerp(0f, 1f, temps / (dureePulsation / 2f));
            mat.SetColor("_BaseColor", col);
            yield return null;
        }

        temps = 0f;
        while (temps < dureePulsation / 2f && dot != null)
        {
            temps += Time.deltaTime;
            col.a = Mathf.Lerp(1f, 0f, temps / (dureePulsation / 2f));
            mat.SetColor("_BaseColor", col);
            yield return null;
        }

        if (dot != null)
        {
            activeDots.Remove(dot);
            Destroy(dot);
        }

        if (joueurDansLaZone)
        {
            GererNouveauDot();
        }
    }

    void CreerTextureRondNoir()
    {
        int size = 128;
        rondNoirTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        float center = size / 2f;
        float radius = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                if (dist < radius)
                {
                    float alpha = Mathf.Clamp01((radius - dist) / 3f);
                    rondNoirTexture.SetPixel(x, y, new Color(0, 0, 0, alpha));
                }
                else
                {
                    rondNoirTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }
        }
        rondNoirTexture.Apply();
    }
}