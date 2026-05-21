using System.Collections;
using UnityEngine;

/*
Öppnar dörr och spelar ljud
*/

public class OpenGateValve : MonoBehaviour
{
    [SerializeField] private CheckValve checkValve;
    [SerializeField] private Animator animatorGarage;
    [SerializeField] private Animator animatorPoolWater;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject poolWater;
    [SerializeField] private GameObject wall;
    [SerializeField] private AudioLibrary library;

    private OpenGateValve openGateValve;

    void Start()
    {
        openGateValve = GetComponent<OpenGateValve>();
    }

    void Update()
    {
        if (checkValve.IsValveInPlace() == true)
        {
            StartCoroutine(Cutscene());

            openGateValve.enabled = false;
        }
    }

    private IEnumerator Cutscene()
    {
        animatorGarage.Play("GarageOpen", 0, 0);
        SoundFXManager.Instance.PlaySoundEffectAtPosition(library.others[0], animatorGarage.transform, 1f);

        yield return null;
    }
}
