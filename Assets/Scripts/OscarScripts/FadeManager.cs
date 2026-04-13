using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FadeManager : MonoBehaviour
{
    [SerializeField] Image fadeScreen;
    //[SerializeField] TextMeshProUGUI test;
    Color black = Color.black;
    Color transparent = Color.clear;
    private Coroutine currentFade;

    public IEnumerator FadeToBlack(float duration)
    {
        Debug.LogWarning("FadeToBlack Called");
        StartFade(duration, fadeScreen.color, black);

        yield return currentFade;
    }
    public IEnumerator FadeFromBlack(float duration)
    {
        Debug.LogWarning("FadeFromBlack Called");
        StartFade(duration, fadeScreen.color, transparent);
        yield return currentFade;
    }

    public IEnumerator FadeObject(GameObject gameObject, float duration)
    {
        StartFade(duration,  gameObject, 0f);
        yield return currentFade;
    }
    public IEnumerator UnfadeObject(GameObject gameObject, float duration)
    {
        StartFade(duration, gameObject, 1f);
        yield return currentFade;
    }

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
    IEnumerator Fade(float duration, GameObject gameObject, float targetAlpha)
    {
        //Shi looks ahhh but lowk works

        //Objekt måste ha surface type transparent 
        Debug.Log("Started fading");
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Color color = renderer.material.color;
        float startAlpha = color.a;

        for (float t = 0; t <= duration; t += Time.deltaTime)
        { 
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
            renderer.material.color = color;
            yield return null;
        }
        color.a = targetAlpha;
        renderer.material.color = color;
        Debug.Log("Finished fading");
    }

    private void StartFade(float duration, Color startColor, Color endColor)
    {
        if(currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        currentFade = StartCoroutine(Fade(duration, startColor, endColor));

    }
    private void StartFade(float duration, GameObject gameObject,float targetAlpha)
    {

        //test.text =$"StartFade called | start: {startColor} | end: {endColor} | currentFade was null: {currentFade == null}";
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        currentFade = StartCoroutine(Fade(duration,gameObject,targetAlpha));

    }
}
