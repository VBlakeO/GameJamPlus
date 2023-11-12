using UnityEngine;

public class RenderPlantColorControl : MonoBehaviour
{
    public string property = "_BaseColor";

    public Color[] baseColors = new Color[1];
    public Color[] dryColors = new Color[1];


    private void OnValidate()
    {
        UpadateColor(baseColors);
    }
    private void Awake()
    {
        UpadateColor(baseColors);
    }

    private void OnEnable() 
    {
        UpadateColor(baseColors);
    }

    public void ActivateDryColor()
    {
        UpadateColor(dryColors);
    }

    public void UpadateColor(Color[] colors)
    {
        Renderer renderer = GetComponent<Renderer>();
        int materialCount = renderer.sharedMaterials.Length;

        for (int i = 0; i < materialCount && i < colors.Length; i++)
        {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock, i);
            propertyBlock.SetColor(property, colors[i]);
            renderer.SetPropertyBlock(propertyBlock, i);
        }
    }
}
