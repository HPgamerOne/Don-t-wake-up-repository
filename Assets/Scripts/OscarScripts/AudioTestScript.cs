using UnityEngine;

public class AudioTestScript : MonoBehaviour
{
    InteractObject interactObject;
    public AudioLibrary library;
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
        }
    }
}
