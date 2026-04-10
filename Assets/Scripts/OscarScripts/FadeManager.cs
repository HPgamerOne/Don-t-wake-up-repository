using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class FadeManager : MonoBehaviour
{
    [SerializeField] Image fadeScreen;


    public  void FadeToBlackScreen(float duration)
    {
        StartCoroutine(Fade(duration));
    }
    public void UnfadeScreen(float duration)
    {
        StartCoroutine(FadeOut(duration));
    }

    public void FadeObject(GameObject gameObject)
    {

    }
    public void UnfadeObject(GameObject gameObject)
    {

    }

    IEnumerator Fade(float duration)
    {
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            fadeScreen.color = new Color(1f, 1f, 1f, t / duration);

        }
        yield return null;
    }
    IEnumerator FadeOut(float duration)
    {
        for (float t = 1; t < duration; t -= Time.deltaTime)
        {
            fadeScreen.color = new Color(1f, 1f, 1f, t / duration);

        }
        yield return null;
    }


}
