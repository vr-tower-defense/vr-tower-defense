using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class Scoreboard : MonoBehaviour
{
    private AudioSource _audioSource;
    public Text[] LivesTextNodes;

    public Text[] ScoreTextNodes;

    public Text[] WavesTextNodes;

    private string FallbackValue = "∞";

    // The total number of waves
    private string _waveCount;
    private int _currentWave = 0;

    // Start is called before the first frame update
    private void Start()
    {
        var wavesState = GameManager.Instance.GetComponent<WavesState>();

        _audioSource = GetComponent<AudioSource>();

        if (wavesState)
        {
            _waveCount = wavesState.Waves.Length.ToString();
        }

        UpdateWavesText();
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

    private void UpdateWavesText()
    {
        // Gametext Update
        foreach (var textNode in WavesTextNodes)
        {
            textNode.text = _waveCount == null
                ? FallbackValue
                : $"{_currentWave}/{_waveCount}";
        }
    }

    void OnWaveStart()
    {
        // Play new wave audio
        _audioSource?.Play();

        // Increment wave number
        _currentWave += 1;

        UpdateWavesText();
    }
}