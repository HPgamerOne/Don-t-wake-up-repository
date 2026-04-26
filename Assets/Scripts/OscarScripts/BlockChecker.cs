using UnityEngine;

public class BlockChecker : MonoBehaviour
{
    public string requiredID;

    private void OnTriggerEnter(Collider other)
    {
        WoodBlock block = other.GetComponent<WoodBlock>();
        if (block != null)
        {
            if(block.id == requiredID)
            {
                //turn a boolean on
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WoodBlock block = other.GetComponent<WoodBlock>();
        if (block != null)
        {
            if (block.id == requiredID)
            {
                //turn a boolean off
            }
        }
    }
}
