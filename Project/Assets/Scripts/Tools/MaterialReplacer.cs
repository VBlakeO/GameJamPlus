using UnityEngine;

public class MaterialReplacer : MonoBehaviour
{
    public bool changeMaterials = false;
    [Space]

    public Material replacementMaterial;

    private void OnValidate() 
    {
        if (changeMaterials)
        {
            ReplaceMaterialsInScene();
            changeMaterials = false;
        }
    }

    public void ReplaceMaterialsInScene()
    {
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            Material[] originalMaterials = renderer.sharedMaterials;
            Material[] newMaterials = new Material[originalMaterials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = replacementMaterial;
            }

            renderer.sharedMaterials = newMaterials;
        }
    }
}

