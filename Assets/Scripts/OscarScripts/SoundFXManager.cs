using UnityEngine;
using UnityEngine.Rendering;


public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    [SerializeField] AudioSource soundObject;
    PlayerController playerController;
    float pitchVariance = 0.1f;
    public AudioLibrary library;

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
    public void PlayGrassFootsteps(Transform playerPosition, float volume)
    {
        if (playerController.CurrentHorizontalSpeed() != 0)
        {
            AudioSource source = Instantiate(soundObject, playerPosition.position - Vector3.down*4, Quaternion.identity);

            source.volume = volume;
            float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);

            source.pitch = randPitch;
            source.clip = library.grassFootsteps;
            source.Play();

            float audioLength = source.clip.length;
            Destroy(source.gameObject, audioLength);
        }
        
    }
    public void PlayConcreteFootsteps(Transform playerPosition, float volume)
    {
        if (playerController.CurrentHorizontalSpeed() != 0)
        {
            AudioSource source = Instantiate(soundObject, playerPosition.position - Vector3.down * 4, Quaternion.identity);

            source.volume = volume;
            float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);

            source.pitch = randPitch;
            source.clip = library.concreteFootsteps;
            source.Play();

            float audioLength = source.clip.length;
            Destroy(source.gameObject, audioLength);
        }
    }
    public void PlayWoodFootsteps(Transform playerPosition, float volume)
    {
        if (playerController.CurrentHorizontalSpeed() != 0)
        {
            AudioSource source = Instantiate(soundObject, playerPosition.position - Vector3.down * 4, Quaternion.identity);

            source.volume = volume;
            float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);

            source.pitch = randPitch;
            source.clip = library.woodFootsteps;
            source.Play();

            float audioLength = source.clip.length;
            Destroy(source.gameObject, audioLength);
        }
    }
    public void PlayWaterFootsteps(Transform playerPosition, float volume)
    {
        
        AudioSource source = Instantiate(soundObject, playerPosition.position - Vector3.down * 4, Quaternion.identity);
        source.clip = library.waterFootsteps;
        source.volume = volume;
        source.Play();
        float randPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        source.pitch = randPitch;
        
        //if (playerController.CurrentHorizontalSpeed() != 0)
        //{
        //    source.loop = true;
        //}
        //if(playerController.CurrentHorizontalSpeed() == 0)
        //{
        //    source.loop = false;
        //    Destroy(source);
        //}
        source.loop = true;
    }
}
