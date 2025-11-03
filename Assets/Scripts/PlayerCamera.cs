using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Vector3 _offset = new(0,0,-7);
    void Start()
    {
        if (_player == null)
        {
            Debug.LogError("No Player Attached to camera");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = _player.transform.position;
        transform.position = new Vector3(playerPosition.x + _offset.x, transform.position.y, playerPosition.z + _offset.z);
    }

}
