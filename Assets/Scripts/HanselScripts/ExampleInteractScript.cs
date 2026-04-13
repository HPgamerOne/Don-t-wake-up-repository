using System.Collections;
using UnityEngine;

public class ExampleInteractScript : MonoBehaviour
{
    InteractObject interactObject;

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

            // If you want it to be a coroutine, you could also just do your logic here instead of calling the coroutine, up to you
            StartCoroutine(DoThing());
        }

        // Have done thing, but not interacting (have this code if action is repeatable and you want it to reset when stop interacting)
        if (doneThing && !interactObject.interacted)
        {
            doneThing = false;
        }
    }

    private IEnumerator DoThing()
    {
        print("Did thing");
        yield return null;
    }
}
