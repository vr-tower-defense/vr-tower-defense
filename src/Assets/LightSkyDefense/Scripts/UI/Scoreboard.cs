using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class Scoreboard : MonoBehaviour
{
    private AudioSource _audioSource;

    public Text[] LivesTextNodes;

    public Text[] ScoreTextNodes;

    public Text[] WavesTextNodes;

    private int _currentWave = 0;

    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnPlayerStatisticsUpdate(PlayerStatistics playerStatistics)
    {
        foreach (var textNode in LivesTextNodes)
        {
            textNode.text = $"{playerStatistics.Lives}/{playerStatistics.InitialLives}";
        }

        foreach (var textNode in ScoreTextNodes)
        {
            textNode.text = $"{playerStatistics.Score}";
        }
    }

    void OnWaveStart()
    {
        // Play new wave audio
        _audioSource?.Play();

        // Increment wave number
        _currentWave += 1;

        // Gametext Update
        foreach (var textNode in WavesTextNodes)
        {
            textNode.text = $"{_currentWave}/{5}";
        }
    }
}