using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Игрок попал в радиус атаки!");

            var player = other.GetComponent<PlayerRespawn>();
            if (player != null)
            {
                player.RespawnAtCheckpoint();
            }
        }
    }
}
