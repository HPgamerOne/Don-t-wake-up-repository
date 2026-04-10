using UnityEngine;

public class ObjectHighlight : MonoBehaviour, IHighlightable
{
    private Renderer[] renderers;
    private MaterialPropertyBlock propertyBlock;

    private static readonly int highlightStrengthId = Shader.PropertyToID("_HighlightStrength");

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    public void SetHighlight(float strength)
    {
        foreach (Renderer rend in renderers)
        {
            if (!rend) continue;

            rend.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat(highlightStrengthId, strength);
            rend.SetPropertyBlock(propertyBlock);
        }
    }
}
public interface IHighlightable
{
    void SetHighlight(float strength);
}