using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private float _torchFuel = 1;
    [SerializeField]
    private float _fuelBurnRate = 0.02F;
    public float TorchFuel
    {
        get => _torchFuel;
    }
    private bool _isGameOver = false;

    // Update is called once per frame
    void Update()
    {
        if (_torchFuel > 0 && _isGameOver)
        {
            _torchFuel -= _fuelBurnRate * Time.deltaTime;
        }
        else
        {
            _isGameOver = true;
        }

        if (_isGameOver)
        {

        }
    }

    public void IncreaseFuel(float addedFuel)
    {
        if (_torchFuel + addedFuel > 1)
        {
            _torchFuel = 1;
        } else
        {
            _torchFuel += addedFuel;
        }
    }

}
