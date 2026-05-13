using UnityEngine;

public class backgroundMusic : MonoBehaviour
{
    [SerializeField]AudioLibrary library;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundFXManager.Instance.ChangeBackgroundMusic(library.music[1], 0.2f);
            SoundFXManager.Instance.EnableMusicLoop();
            SoundFXManager.Instance.PlayBackgroundMusic();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundFXManager.Instance.StopBackgroundMusic();
            Destroy(gameObject);
        }
    }
}
