using System.Collections;
using UnityEngine;

public class OpenGateValve : MonoBehaviour
{
    [SerializeField] private CheckValve checkValve;
    [SerializeField] private Animator animatorGarage;
    [SerializeField] private Animator animatorPoolWater;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject poolWater;
    [SerializeField] private GameObject wall;

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
        yield return new WaitForSeconds(3f);

        //playerCamera.enabled = false;
        animatorPoolWater.Play("PoolWater", 0, 0);

        yield return new WaitForSeconds(4f);

        //playerCamera.enabled = true;
        animatorGarage.Play("GarageOpen", 0, 0);

        wall.gameObject.GetComponent<BoxCollider>().enabled = false;

        yield return null;
    }
}
