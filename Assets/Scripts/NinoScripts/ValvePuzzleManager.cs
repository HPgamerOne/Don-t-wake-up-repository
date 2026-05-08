using System.Collections;
using UnityEngine;

public class ValvePuzzleManager : MonoBehaviour
{
    public static ValvePuzzleManager Instance;
    [SerializeField] private Animator animatorGarage;
    [SerializeField] private Animator animatorPoolWater;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject wall;

    private int valvesPlaced = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void ValvePlaced()
    {
        valvesPlaced++;

        if (valvesPlaced == 1)
        {
            TriggerLabyrinth();
        }
        else if (valvesPlaced >= 2)
        {
            TriggerWater();
        }
    }

    void TriggerLabyrinth()
    {
        animatorGarage.Play("GarageOpen", 0, 0);
    }

    void TriggerWater()
    {
        StartCoroutine(Cutscene());
        //animatorPoolWater.Play("PoolWater", 0, 0);
    }

    private IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(3f);

        playerCamera.enabled = false;
        animatorPoolWater.Play("PoolWater", 0, 0);

        yield return new WaitForSeconds(4f);

        playerCamera.enabled = true;

        wall.gameObject.GetComponent<BoxCollider>().enabled = false;

        yield return null;
    }
}