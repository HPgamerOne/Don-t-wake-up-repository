using UnityEngine;

public class backgroundMusic : MonoBehaviour
{
    [SerializeField]AudioLibrary library;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int index = GameManager.Instance.currentScene;
            SoundFXManager.Instance.ChangeBackgroundMusic(library.music[index-1], 0.2f);
            SoundFXManager.Instance.EnableMusicLoop();
            SoundFXManager.Instance.PlayBackgroundMusic();
            Destroy(gameObject);
        }
    }
}
