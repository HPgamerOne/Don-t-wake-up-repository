using UnityEngine;

public class StopBackgroundMusic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SoundFXManager.Instance.StopBackgroundMusic();
        Destroy(gameObject);
    }
}
