using System.Collections;
using UnityEngine;

public class OpenGateValve : MonoBehaviour
{
    [SerializeField] private CheckValve checkValve;
    [SerializeField] private Animator animatorGarage;
    [SerializeField] private Animator animatorPoolWater;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject poolWater;

    private OpenGateValve openGateValve;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        openGateValve = GetComponent<OpenGateValve>();
    }

    // Update is called once per frame
    void Update()
    {
        if (checkValve.IsValveInPlace() == true)
        {
            StartCoroutine(DoThing());

            openGateValve.enabled = false;
        }
    }

    private IEnumerator DoThing()
    {
        yield return new WaitForSeconds(3f);

        playerCamera.enabled = false;

        animatorPoolWater.Play("PoolWater", 0, 0);

        yield return new WaitForSeconds(4f);

        playerCamera.enabled = true;

        animatorGarage.Play("GarageOpen", 0, 0);

        yield return null;
    }
}
