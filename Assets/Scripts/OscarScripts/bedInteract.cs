using UnityEngine;
using System.Collections;

public class BedInteract : MonoBehaviour
{
    InteractObject interactObject;
    public float fadeDuration;
    private bool doneThing = false;
    void Start()
    {
        interactObject = GetComponent<InteractObject>();
    }

    void Update()
    {
        // Have not done thing, first time interacting
        if (!doneThing && interactObject.interacted)
        {
            doneThing = true;


            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {
        doneThing = true;
        FadeManager.Instance.FadeToBlack(fadeDuration);

        Timer.Instance.StopTimer();
        //Timer.Instance.ResetTimer();

        yield return new WaitForSeconds(fadeDuration);

        Timer.Instance.ResetTimer();
        GameManager.Instance.NextScene();
        
        FadeManager.Instance.FadeFromBlack(fadeDuration);
        yield return null;
    }
}
