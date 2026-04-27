using UnityEngine;

public class ToyBoxScript : MonoBehaviour
{
    BlockChecker[] checkers;
    int totalCheckers;
    int totalR, totalC, totalA, totalE;


    // Update is called once per frame
    private void Awake()
    {
        checkers = FindObjectsByType<BlockChecker>(FindObjectsSortMode.None);
    }

    void Update()
    {
        
        
        foreach (BlockChecker checker in checkers)
        {
            
        }


    }

    public void UpdateCheckers()
    {
        foreach (BlockChecker checker in checkers)
        {

        }
    }
}
