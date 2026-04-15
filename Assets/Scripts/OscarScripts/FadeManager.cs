using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class FadeManager : MonoBehaviour
{
    [SerializeField] Image fadeScreen;

    public static FadeManager Instance;
    private Coroutine currentFade;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Fade out an object
    /// </summary>
    /// <param name="gameObject">Chosen object to fade out</param>
    /// <param name="duration">Time it takes to fade in the object</param>
    public void FadeOutObject(GameObject gameObject, float duration)
    {
        StartCoroutine(FadeOut(gameObject, duration));
    }
    /// <summary>
    /// Fade in an object 
    /// </summary>
    /// <param name="gameObject">Chosen object to fade in</param>
    /// <param name="duration">Time it takes to fade out the object</param>
    public void FadeInObject(GameObject gameObject, float duration)
    {
        gameObject.GetComponent<Renderer>().enabled = true;
        StartFade(duration, gameObject,1f);
    }
    /// <summary>
    /// Fade screen to black
    /// </summary>
    /// <param name="duration">Time it takes to fade to black</param>
    public void FadeToBlack(float duration)
    {
        StartCoroutine(FadeToBlackIEmum(duration));
    }
    /// <summary>
    /// Fade screen from black
    /// </summary>
    /// <param name="duration">Time it takes to fade from black</param>
    public void FadeFromBlack(float duration)
    {
        StartCoroutine(FadeFromBlackIEnum(duration));
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

    IEnumerator FadeToBlackIEmum(float duration)
    {
        StartFade(duration, fadeScreen.color, Color.black);
        yield return currentFade;
    }
    IEnumerator FadeFromBlackIEnum(float duration)
    {
        StartFade(duration, fadeScreen.color, Color.clear);
        yield return currentFade;
    }

    IEnumerator FadeOut(GameObject gameObject, float duration)
    {
        StartFade(duration, gameObject, 0f);
        yield return currentFade;
        gameObject.GetComponent<Renderer>().enabled = false;
    }
    //Dis shit actually does the fading shit
    IEnumerator Fade(float duration, Color startColor, Color endColor)
    {
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            fadeScreen.color = Color.Lerp(startColor, endColor, t / duration);
            yield return null;

        }
        fadeScreen.color = endColor;
    }
    //Dis shit actually does the fading shit
    IEnumerator Fade(float duration, GameObject gameObject, float targetAlpha)
    {
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
