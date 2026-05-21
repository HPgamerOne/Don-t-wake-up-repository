using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;


public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    [SerializeField] AudioSource soundObject;
    [SerializeField] AudioSource stepsSource;
    [SerializeField] AudioSource source;
    public AudioLibrary library;
    float pitchVariance = 0.15f;
    private float pitch = 0f;

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

    public void PlaySoundEffectAtPosition(AudioClip audio, Transform spawnLocation, float volume)
    {
        AudioSource source = Instantiate(soundObject, spawnLocation.position, Quaternion.identity);

        source.volume = volume;
        source.clip = audio;
        source.Play();

        float audioLength = source.clip.length;

        Destroy(source.gameObject, audioLength);
    }
    public void PlaySoundEffect(AudioClip audio, float volume)
    {
        source.volume = volume;
        source.clip = audio;
        source.Play();

        float audioLength = source.clip.length;

    }
    public void PlayRandomSoundEffectAtPosition(AudioClip[] audio, Transform spawnLocation, float volume)
    {
        AudioSource source = Instantiate(soundObject, spawnLocation.position, Quaternion.identity);
        int rand = Random.Range(0, audio.Length);

        source.volume = volume;
        float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        source.pitch = randPitch;
        source.clip = audio[rand];
        source.Play();

        float audioLength = source.clip.length;
        Destroy(source.gameObject, audioLength);
    }
    public void PlayRandomSoundEffect(AudioClip[] audio, float volume)
    {
        int rand = Random.Range(0, audio.Length);

        source.volume = volume;
        float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        source.pitch = randPitch;
        source.clip = audio[rand];
        source.Play();

        float audioLength = source.clip.length;
    }
    /// <summary>
    /// Change the background music and start playing it
    /// </summary>
    /// <param name="music">Audio file of the new background music</param>
    public void ChangeBackgroundMusic(AudioClip music, float volume)
    {
        source.clip = music;
        source.volume = volume;
    }
    /// <summary>
    /// Stops background music
    /// </summary>
    public void StopBackgroundMusic()
    {
        if (source != null)
        {
            source.Stop();
        }
        source.Stop();
    }
    /// <summary>
    /// Plays background music
    /// </summary>
    public void PlayBackgroundMusic()
    {
        source.Play();
    }
    public void PlayGrassFootsteps(float volume)
    {
        int index = Random.Range(0, 3);
        stepsSource.clip = library.grassFootsteps[index];
        stepsSource.volume = volume;
        float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        stepsSource.pitch = randPitch;
        stepsSource.Play();
        
    }
    public void PlayConcreteFootsteps(float volume)
    {
        int index = Random.Range(0, 3);
        stepsSource.clip = library.concreteFootsteps[index];
        stepsSource.volume = volume;
        float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        stepsSource.pitch = randPitch;
        stepsSource.Play();
        
    }
    public void PlayWoodFootsteps(float volume)
    {
        int index = Random.Range(0,3);
        stepsSource.clip = library.woodFootsteps[index];
        stepsSource.volume = volume;
        float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        stepsSource.pitch = randPitch;
        stepsSource.Play();
    }
    public void PlayWaterFootsteps(float volume)
    {
        int index = Random.Range(0,3);
        stepsSource.clip = library.waterFootsteps[index];
        stepsSource.volume = volume;
        float randPitch = Random.Range(1f - pitchVariance + pitch, 1f + pitchVariance + pitch);
        stepsSource.pitch = randPitch;
        stepsSource.Play();
    }
    public void EnableMusicLoop()
    {
        source.loop = true;
    }
    public void DisableMusicLoop()
    {
        source.loop = false;
    }
    public void SpeedUpClip(float speed)
    {
        pitch = speed;
    }
    public void ReturnClipSpeed()
    {
        pitch = 0f;
    }
}
