using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class Scoreboard : MonoBehaviour, IOnGameLossTarget
{
    public AudioClip BossSpawnedClip;
    public float Score;
    public float TimeBetweenWaves = 30;
    public float Timer;
    public int TotalNumberOfWaves = 15;
    public int WaveNumber;

    private Text _creditsValue;
    private Text _creditsValueBack;

    private Text _livesValue;
    private Text _livesValueBack;

    private Text _nextWaveTimerValue;
    private Text _nextWaveTimerValueBack;

    private PlayerStatistics _player;

    private Text _scoreValue;
    private Text _scoreValueBack;

    private AudioSource _source;

    private Text _timerValue;
    private Text _timerValueBack;

    private float _timeToNextWave;

    private Text _wavesValue;
    private Text _wavesValueBack;

    //When the player loses
    public void OnGameLoss()
    {
        _livesValue.text = _player.Lives.ToString();
        _livesValueBack.text = _livesValue.text;
        enabled = false;
    }


    // Start is called before the first frame update
    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _timeToNextWave = TimeBetweenWaves;
        _player = Player.instance.gameObject.GetComponent<PlayerStatistics>();

        //Front
        _scoreValue = gameObject.transform.Find("PlayerTextCanvas").Find("ScoreValueText").gameObject
            .GetComponent<Text>();
        _creditsValue = gameObject.transform.Find("PlayerTextCanvas").Find("CreditsValueText").gameObject
            .GetComponent<Text>();
        _livesValue = gameObject.transform.Find("PlayerTextCanvas").Find("LivesValueText").gameObject
            .GetComponent<Text>();

        //Right side
        _timerValue = gameObject.transform.Find("GameTextCanvas").Find("TimerValueText").gameObject
            .GetComponent<Text>();
        _nextWaveTimerValue = gameObject.transform.Find("GameTextCanvas").Find("TimeToNextWaveValueText").gameObject
            .GetComponent<Text>();
        _wavesValue = gameObject.transform.Find("GameTextCanvas").Find("WaveValueText").gameObject.GetComponent<Text>();

        //Back
        _scoreValueBack = gameObject.transform.Find("PlayerTextBackCanvas").Find("ScoreValueBackText").gameObject
            .GetComponent<Text>();
        _creditsValueBack = gameObject.transform.Find("PlayerTextBackCanvas").Find("CreditsValueBackText").gameObject
            .GetComponent<Text>();
        _livesValueBack = gameObject.transform.Find("PlayerTextBackCanvas").Find("LivesValueBackText").gameObject
            .GetComponent<Text>();

        //Left side
        _timerValueBack = gameObject.transform.Find("GameTextBackCanvas").Find("TimerValueBackText").gameObject
            .GetComponent<Text>();
        _nextWaveTimerValueBack = gameObject.transform.Find("GameTextBackCanvas").Find("TimeToNextWaveValueBackText")
            .gameObject.GetComponent<Text>();
        _wavesValueBack = gameObject.transform.Find("GameTextBackCanvas").Find("WaveValueBackText").gameObject
            .GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Playertext Update
        _scoreValue.text = $"{Score}";
        _scoreValueBack.text = _scoreValue.text;

        _livesValue.text = $"{_player.Lives}";
        _livesValueBack.text = _livesValue.text;

        _creditsValue.text = $"{_player.Credits}";
        _creditsValueBack.text = _creditsValue.text;

        //Timer Update
        Timer += Time.deltaTime;
        var minutes = Mathf.Floor(Timer / 60).ToString("00");
        var seconds = Mathf.Floor(Timer % 60).ToString("00");

        //Time until next wave
        if (WaveNumber != TotalNumberOfWaves)
        {
            _timeToNextWave -= Time.deltaTime;

            if (_timeToNextWave <= 0.0f)
            {
                WaveNumber++;
                if (WaveNumber % 5 == 0)
                    _source?.PlayOneShot(BossSpawnedClip);
                else
                    _source?.Play();

                if (WaveNumber == TotalNumberOfWaves)
                {
                    _nextWaveTimerValue.text = "N/A";
                    _nextWaveTimerValueBack.text = _nextWaveTimerValue.text;
                    return;
                }

                _timeToNextWave = TimeBetweenWaves;
            }

            var minutesToNextWave = Mathf.Floor(_timeToNextWave / 60).ToString("00");
            var secondsToNextWave = Mathf.Ceil(_timeToNextWave % 60).ToString("00");

            _nextWaveTimerValue.text = $"{minutesToNextWave}:{secondsToNextWave}";
            _nextWaveTimerValueBack.text = _nextWaveTimerValue.text;
        }

        //Gametext Update

        _timerValue.text = $"{minutes}:{seconds}";
        _timerValueBack.text = _timerValue.text;

        _wavesValue.text = $"{WaveNumber}/{TotalNumberOfWaves}";
        _wavesValueBack.text = _wavesValue.text;
    }

    //Points gained when enemy dies
    public void PointGain(float points)
    {
        Score += points;
    }
}