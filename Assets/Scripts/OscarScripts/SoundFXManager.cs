using UnityEngine;


public class SoundFXManager : MonoBehaviour
{
   public static SoundFXManager Instance;
    [SerializeField] AudioSource soundObject;
    

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySoundEffect(AudioClip audio, Transform spawnLocation, float volume)
    {
        AudioSource source = Instantiate(soundObject, spawnLocation.position, Quaternion.identity);

        source.volume = volume;
        source.clip = audio;
        source.Play();

        float audioLength = source.clip.length;

        Destroy(source.gameObject, audioLength);
    }
    public void PlayRandomSoundEffect(AudioClip[] audio, Transform spawnLocation, float volume)
    {
        AudioSource source = Instantiate(soundObject, spawnLocation.position, Quaternion.identity);
        int rand = Random.Range(0, audio.Length);

        source.volume = volume;
        source.clip = audio[rand];
        source.Play();

        float audioLength = source.clip.length;
        Destroy(source.gameObject, audioLength);
    }
    /// <summary>
    /// Change the background music and start playing it
    /// </summary>
    /// <param name="music">Audio file of the new background music</param>
    public void ChangeAndPlayBackgroundMusic(AudioClip music)
    {
        gameObject.GetComponent<AudioSource>().clip = music;
        gameObject.GetComponent<AudioSource>().Play();
    }
    /// <summary>
    /// Stops background music
    /// </summary>
    public void StopBackgroundMusic()
    {
        gameObject.GetComponent<AudioSource>().Stop();
    }
    /// <summary>
    /// Plays background music
    /// </summary>
    public void PlayBackgroundMusic()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }
}
