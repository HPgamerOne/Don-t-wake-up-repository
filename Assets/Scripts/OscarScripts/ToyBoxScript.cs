using Unity.Jobs;
using UnityEngine;

public class ToyBoxScript : MonoBehaviour
{
    private BlockChecker[] checkers;
    [SerializeField]  Animator animator;
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
        animator.Play("ToyboxOpen",0,0);
    }
}
