using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class FadeManager : MonoBehaviour
{
    [SerializeField] Image fadeScreen;

    private Coroutine currentFade;

    public void FadeOutObject(GameObject gameObject, float duration)
    {
        StartCoroutine(FadeOutAndDisable(gameObject, duration));
    }
    public void FadeInObject(GameObject gameObject, float duration)
    {
        gameObject.SetActive(true);
        StartFade(duration, gameObject,1f);
    }

    //Dis shit stops dem running coroutine for da next onee
    private void StartFade(float duration, Color startColor, Color endColor)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        currentFade = StartCoroutine(Fade(duration, startColor, endColor));
    }
    //Dis shit stops dem running coroutine for da next onee
    private void StartFade(float duration, GameObject gameObject, float targetAlpha)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        currentFade = StartCoroutine(Fade(duration, gameObject, targetAlpha));
    }
    private void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Surface", 1f);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.renderQueue = 3000;
        mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
    }

    private void SetMaterialOpaque(Material mat)
    {
        mat.SetFloat("_Surface", 0f);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.renderQueue = -1;
        mat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
    }
    public IEnumerator FadeToBlack(float duration)
    {
        //Debug.LogWarning("FadeToBlack Called");
        StartFade(duration, fadeScreen.color, Color.black);

        yield return currentFade;
    }
    public IEnumerator FadeFromBlack(float duration)
    {
        //Debug.LogWarning("FadeFromBlack Called");
        StartFade(duration, fadeScreen.color, Color.clear);
        yield return currentFade;
    }

    private IEnumerator FadeOutAndDisable(GameObject gameObject, float duration)
    {
        StartFade(duration, gameObject, 0f);
        yield return currentFade;
        gameObject.SetActive(false);
    }
    //Dis shit actually does the fading shit
    IEnumerator Fade(float duration, Color startColor, Color endColor)
    {
        Debug.Log("Started fading");
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            fadeScreen.color = Color.Lerp(startColor, endColor, t / duration);
            yield return null;

        }
        fadeScreen.color = endColor;
        Debug.Log("Finished fading");

    }
    //Dis shit actually does the fading shit
    IEnumerator Fade(float duration, GameObject gameObject, float targetAlpha)
    {
        //Shi looks ahhh but lowk works

        //Objekt måste ha surface type transparent 
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Material mat = renderer.material;

        Shader originalShader = mat.shader;
        mat.shader = Shader.Find("Universal Render Pipeline/Lit");
        SetMaterialTransparent(mat);


        Color color = mat.color;
        float startAlpha = color.a;

        for (float t = 0; t <= duration; t += Time.deltaTime)
        { 
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
            mat.color = color;
            yield return null;
        }
        color.a = targetAlpha;
        mat.color = color;

        if(targetAlpha >= 1)
        {
            mat.shader = originalShader;
            SetMaterialOpaque(mat);
        }
    }
}
