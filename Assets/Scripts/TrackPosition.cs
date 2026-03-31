using UnityEngine;

public class TrackPosition : MonoBehaviour
{
    [SerializeField] private GameObject tracker;
    [SerializeField] private Material grass;
    private Vector3 trackerPos;

    void Update()
    {
        trackerPos = tracker.GetComponent<Transform>().position;
        grass.SetVector("_TrackerPosition", trackerPos);
    }
}
