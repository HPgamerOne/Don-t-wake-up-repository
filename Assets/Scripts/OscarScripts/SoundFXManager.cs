using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
   public static SoundFXManager Instance;
    [SerializeField] AudioSource soundObject;

    private void Awake()
    {
       if (Instance == null)
        {
            Instance = this;
        }
    }
    
    public void PlaySoundEffect(AudioClip audio, Transform spawnLocation)
    {
        //instantiate an object
        AudioSource source = Instantiate(soundObject, spawnLocation.position, Quaternion.identity);
        //play dat shit
        source.clip = audio;

        source.Play();
        //wait x amount of time for audio to finish
        float audioLength = source.clip.length;

        Destroy(source.gameObject, audioLength);
    }
}
