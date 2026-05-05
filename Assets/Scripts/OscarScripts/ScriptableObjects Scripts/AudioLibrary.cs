using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Scriptable Objects/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    public AudioClip waterFootsteps;
    public AudioClip woodFootsteps;
    public AudioClip grassFootsteps;
    public AudioClip concreteFootsteps;
    public AudioClip jump;
    public AudioClip success;
    public AudioClip warning;
}
