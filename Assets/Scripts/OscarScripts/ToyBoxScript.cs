using Unity.Jobs;
using UnityEngine;

public class ToyBoxScript : MonoBehaviour
{
    private BlockChecker[] checkers;
    GameObject lid;
    Transform lidTransform;
    bool rotating = false;
    Vector3 rotation = new Vector3(10, 0, 0);
    // Update is called once per frame
    private void Start()
    {
        checkers = FindObjectsByType<BlockChecker>(FindObjectsSortMode.None);
    }

    public void UpdateCheckers()
    {
        foreach (BlockChecker checker in checkers)
        {
            if (!checker.isFilled)
            {
                return;
            }
        }
        OpenBox();
    }
    private void OpenBox()
    {
        lid = GameObject.Find("LidHolder");
        lidTransform = lid.GetComponent<Transform>();
        rotating = true;

    }
    private void Update()
    {
        if (rotating)
        {
            transform.RotateAround(transform.position, Vector3.forward, 10f*Time.deltaTime);
            print(transform.rotation.x);
            if(transform.rotation.x <= -0.40)
            {
                rotating = false;
            }

        }
    }
}
