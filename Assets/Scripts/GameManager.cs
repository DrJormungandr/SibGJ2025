using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _gameOverText;
    [SerializeField]
    private TextMeshProUGUI _victoryText;
    private float _torchFuel = 1;
    [SerializeField]
    private float _fuelBurnRate = 0.02F;
    public float TorchFuel
    {
        get => _torchFuel;
    }
    private bool _isGameOver = false;
    [SerializeField]
    private int _checkpointsToWin = 10;
    private int _currentCheckpoints = 0;

    void Start()
    {
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        if (_torchFuel > 0)
        {
            _torchFuel -= _fuelBurnRate * Time.deltaTime;
        }
        else
        {
            _isGameOver = true;
        }

        if (_isGameOver)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _fuelBurnRate = 0;
        _torchFuel = 0.0001F;
        _isGameOver = false;
        _gameOverText.enabled = true;
        StartCoroutine(RestartGame());

    }
    private void WinSequence()
    {
        _fuelBurnRate = 0;
        _victoryText.enabled = true;
        StartCoroutine(RestartGame());
    }

    public void IncreaseFuel(float addedFuel)
    {
        if (_torchFuel + addedFuel > 1)
        {
            _torchFuel = 1;
        }
        else
        {
            _torchFuel += addedFuel;
        }
    }

    public void OnCheckpointPickUp()
    {
        _currentCheckpoints++;
        UpdateScoreText();
        if (_currentCheckpoints >= _checkpointsToWin)
        {
            WinSequence();
        }
    }

    private void UpdateScoreText()
    {
        _scoreText.text = $"{_currentCheckpoints}/{_checkpointsToWin}";
    }

    public void OnPlayerDeath()
    {
        GameOverSequence();
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
