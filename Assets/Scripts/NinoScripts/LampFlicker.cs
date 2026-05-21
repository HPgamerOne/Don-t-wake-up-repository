using UnityEngine;
using System.Collections;

/*
Lampa slocknar och lyser i intervall
*/

public class LampFlicker : MonoBehaviour
{
    [SerializeField] private Light lampLight;
    [SerializeField] private Renderer lampRenderer;
    [SerializeField] private Material lightOnMaterial;
    [SerializeField] private Material lightOffMaterial;

    [SerializeField] private float minFlickerTime = 0.05f;
    [SerializeField] private float maxFlickerTime = 0.3f;

    [SerializeField] private float minPauseTime = 1f;
    [SerializeField] private float maxPauseTime = 4f;

    private void Start()
    {
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime));

            int flickerCount = Random.Range(2, 8);

            for (int i = 0; i < flickerCount; i++)
            {
                ToggleLamp(false);
                yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));

                ToggleLamp(true);
                yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));
            }
        }
    }

    void ToggleLamp(bool state)
    {
        lampLight.enabled = state;

        if (lampRenderer != null)
        {
            lampRenderer.material = state ? lightOnMaterial : lightOffMaterial;
        }
    }
}