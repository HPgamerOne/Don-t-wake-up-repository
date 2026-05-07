using UnityEngine;
using UnityEngine.Rendering;


public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    [SerializeField] AudioSource soundObject;
    [SerializeField] AudioSource stepsSource;
    public AudioLibrary library;
    PlayerController playerController;
    float pitchVariance = 0.1f;
    bool footStepsPlaying = false;

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
        if (playerController != null)
        {
            playerController = GameObject.FindAnyObjectByType<PlayerController>();

        }
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
    public void ChangeBackgroundMusic(AudioClip music)
    {
        gameObject.GetComponent<AudioSource>().clip = music;
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
    public void PlayGrassFootsteps(float volume)
    {

        stepsSource.clip = library.grassFootsteps;
        stepsSource.volume = volume;
        stepsSource.Play();
        footStepsPlaying = true;
        float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        stepsSource.pitch = randPitch;
    }
    public void PlayConcreteFootsteps(float volume)
    {

        stepsSource.clip = library.concreteFootsteps;
        stepsSource.volume = volume;
        stepsSource.Play();
        footStepsPlaying = true;
        float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        stepsSource.pitch = randPitch;
    }
    public void PlayWoodFootsteps(float volume)
    {

        stepsSource.clip = library.woodFootsteps;
        stepsSource.volume = volume;
        stepsSource.Play();
        footStepsPlaying = true;
        float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        stepsSource.pitch = randPitch;
    }
    public void PlayWaterFootsteps(float volume)
    {
        
        stepsSource.clip = library.waterFootsteps;
        stepsSource.volume = volume;
        stepsSource.Play();
        footStepsPlaying = true;
        float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        stepsSource.pitch = randPitch;
        
   
    }
    public void StopFootsteps()
    {
        stepsSource.Stop();
        footStepsPlaying = false;
    }
    public bool FootStepsPlaying 
    {
        get { return footStepsPlaying; }
    }
}
