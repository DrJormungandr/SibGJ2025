using UnityEngine;

public class Torch : MonoBehaviour
{

    [SerializeField]
    private GameObject _lightObject;
    private GameManager _gameManager;
    private Light _lightSource;

    void Awake()
    {
        _lightSource = _lightObject.GetComponent<Light>();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        _lightSource.intensity = _gameManager.TorchFuel;
    }
}
