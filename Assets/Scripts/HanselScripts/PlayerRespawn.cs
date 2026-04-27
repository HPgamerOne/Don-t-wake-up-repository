using System.Collections;
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
    private bool isRespawning = false;
    void Start()
    {
        player = GameObject.Find("Player");
        characterController = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckCheckpointTouch();

        if (!isRespawning && player.transform.position.y < respawnY)
        {
            StartCoroutine(RespawnPlayer());
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

    IEnumerator RespawnPlayer()
    {
        isRespawning = true;

        FadeManager.Instance.FadeToBlack(0.5f);
        yield return new WaitForSeconds(1);
        characterController.enabled = false;
        player.transform.position = lastTouchedRespawnPair.RespawnPoint.transform.position;
        characterController.enabled = true;
        FadeManager.Instance.FadeFromBlack(0.5f);
        isRespawning = false;
    }
}