using UnityEngine;

public class ObjectDissolve : MonoBehaviour, IDissolvable
{
    private Renderer[] renderers;
    private MaterialPropertyBlock propertyBlock;

    private static readonly int cutoffHeightId = Shader.PropertyToID("_Cutoff_Height");

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    public void SetDissolve(float cutoffHeight)
    {
        foreach (Renderer rend in renderers)
        {
            if (!rend) continue;

            rend.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat(cutoffHeightId, cutoffHeight);
            rend.SetPropertyBlock(propertyBlock);
        }
    }
}
public interface IDissolvable
{
    void SetDissolve(float cutoffHeight);
}