using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 checkpointPosition;
    private CharacterController characterController;

    private void Start()
    {
        checkpointPosition = transform.position;
        characterController = GetComponent<CharacterController>();
    }

    public void SetCheckpoint(Vector3 newPosition)
    {
        checkpointPosition = newPosition;
    }

    public void RespawnAtCheckpoint()
    {
        Debug.Log("Респаун игрока в чекпоинте!");
        if (characterController != null)
            characterController.enabled = false;
        transform.position = checkpointPosition;
        characterController.enabled = true;
    }
}
