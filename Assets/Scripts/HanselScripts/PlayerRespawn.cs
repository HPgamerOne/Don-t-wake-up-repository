using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [System.Serializable]
    public class RespawnPair
    {
        public GameObject CheckPoint;
        public GameObject RespawnPoint;
    }

    [Header("Respawn")]
    [SerializeField] private float respawnY = -50f;

    [Header("Checkpoints/Respawns")]
    [SerializeField] private List<RespawnPair> respawnPairs = new List<RespawnPair>();

    private GameObject player;
    private CharacterController characterController;

    private RespawnPair lastTouchedRespawnPair;

    void Start()
    {
        player = GameObject.Find("Player");
        characterController = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckCheckpointTouch();
        if (player.transform.position.y < respawnY)
        {
            RespawnPlayer();
        }
    }

    private void CheckCheckpointTouch()
    {
        foreach (RespawnPair respawnPair in respawnPairs)
        {
            BoxCollider checkpointCollider = respawnPair.CheckPoint.GetComponent<BoxCollider>();

            if (checkpointCollider.bounds.Contains(player.transform.position))
            {
                lastTouchedRespawnPair = respawnPair;
            }
        }
    }
    private void RespawnPlayer()
    {
        characterController.enabled = false;
        player.transform.position = lastTouchedRespawnPair.RespawnPoint.transform.position;
        characterController.enabled = true;
    }
}