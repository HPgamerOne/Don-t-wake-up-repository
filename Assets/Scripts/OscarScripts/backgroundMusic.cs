using UnityEngine;

public class backgroundMusic : MonoBehaviour
{
    [SerializeField]AudioLibrary library;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundFXManager.Instance.ChangeBackgroundMusic(library.music[0], 0.2f);
            SoundFXManager.Instance.EnableMusicLoop();
            SoundFXManager.Instance.PlayBackgroundMusic();
            Destroy(gameObject);
        }
    }
}
