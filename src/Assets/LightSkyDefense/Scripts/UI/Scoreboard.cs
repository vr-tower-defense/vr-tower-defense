using UnityEngine;
using Valve.VR.InteractionSystem;

public class Scoreboard : MonoBehaviour, IOnGameLoseTarget
{
    public AudioClip BossSpawnedClip;
    public float Score;
    public float TimeBetweenWaves = 0;
    public float Timer;
    public int TotalNumberOfWaves = 15;
    public int WaveNumber;

    private TextMesh _credits;
    private TextMesh _creditsClone;
    private float _playercredits;

    private TextMesh _lives;
    private TextMesh _livesClone;
    private float _remaininglives;

    private TextMesh _score;
    private TextMesh _scoreClone;

    private AudioSource _source;

    private TextMesh _timer;
    private TextMesh _timerClone;

    private TextMesh _timerToNextWave;
    private TextMesh _timerToNextWaveClone;
    private float _timeToNextWave;

    private TextMesh _waveProgression;
    private TextMesh _waveProgressionClone;

    private PlayerStatistics _player;

    //When the player loses
    public void OnGameLose()
    {
        _remaininglives = _player.Lives;
        _lives.text = $"Lives: {_remaininglives}";
        _livesClone.text = _lives.text;
        enabled = false;
    }


    // Start is called before the first frame update
    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _timeToNextWave = TimeBetweenWaves;
        _player = Player.instance.gameObject.GetComponent<PlayerStatistics>();

        //Scoreboard Front
        //Score
        var scoreText = gameObject.transform.Find("ScoreText").gameObject;
        _score = scoreText.GetComponent<TextMesh>();
        _score.fontSize = 30;
        _score.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        _score.anchor = TextAnchor.MiddleLeft;
        _score.transform.localEulerAngles = new Vector3(-90, 0, 0);
        _score.transform.localPosition = new Vector3(-0.01f, -0.0115f, -0.002f);

        //Time
        var timerText = gameObject.transform.Find("TimerText").gameObject;
        _timer = timerText.GetComponent<TextMesh>();
        _timer.fontSize = 30;
        _timer.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        _timer.anchor = TextAnchor.MiddleLeft;
        _timer.transform.localEulerAngles = new Vector3(-90, 0, 0);
        _timer.transform.localPosition = new Vector3(-0.01f, -0.0115f, 0.002f);

        //Time to next wave
        var timerToNextWaveText = gameObject.transform.Find("TimeToNextWaveText").gameObject;
        _timerToNextWave = timerToNextWaveText.GetComponent<TextMesh>();
        _timerToNextWave.fontSize = 30;
        _timerToNextWave.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        _timerToNextWave.anchor = TextAnchor.MiddleLeft;
        _timerToNextWave.transform.localEulerAngles = new Vector3(-90, 0, 0);
        _timerToNextWave.transform.localPosition = new Vector3(-0.008f, -0.0115f, 0.005f);

        //Wave Progression
        var waveProgressionText = gameObject.transform.Find("WaveProgressionText").gameObject;
        _waveProgression = waveProgressionText.GetComponent<TextMesh>();
        _waveProgression.fontSize = 30;
        _waveProgression.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        _waveProgression.anchor = TextAnchor.MiddleLeft;
        _waveProgression.transform.localEulerAngles = new Vector3(-90, 0, 0);
        _waveProgression.transform.localPosition = new Vector3(-0.004f, -0.0115f, -0.005f);

        //Remaining lives
        var remainingLivesText = gameObject.transform.Find("LivesText").gameObject;
        _lives = remainingLivesText.GetComponent<TextMesh>();
        _lives.fontSize = 30;
        _lives.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        _lives.anchor = TextAnchor.MiddleRight;
        _lives.transform.localEulerAngles = new Vector3(-90, 0, 0);
        _lives.transform.localPosition = new Vector3(0.01f, -0.0115f, -0.002f);

        //Credits
        var creditsText = gameObject.transform.Find("CreditText").gameObject;
        _credits = creditsText.GetComponent<TextMesh>();
        _credits.fontSize = 30;
        _credits.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        _credits.anchor = TextAnchor.MiddleRight;
        _credits.transform.localEulerAngles = new Vector3(-90, 0, 0);
        _credits.transform.localPosition = new Vector3(0.01f, -0.0115f, 0.002f);

        //Scoreboard back
        //Score
        _scoreClone = Instantiate(_score, transform);
        _scoreClone.transform.localScale = new Vector3(-0.0005f, 0.0005f, 0.0005f);
        _scoreClone.transform.localPosition = new Vector3(0.01f, 0.0115f, -0.002f);

        //Time
        _timerClone = Instantiate(_timer, transform);
        _timerClone.transform.localScale = new Vector3(-0.0005f, 0.0005f, 0.0005f);
        _timerClone.transform.localPosition = new Vector3(0.01f, 0.0115f, 0.002f);

        //Time to next wave
        _timerToNextWaveClone = Instantiate(_timerToNextWave, transform);
        _timerToNextWaveClone.transform.localScale = new Vector3(-0.0005f, 0.0005f, 0.0005f);
        _timerToNextWaveClone.transform.localPosition = new Vector3(0.008f, 0.0115f, 0.005f);

        //Wave progression
        _waveProgressionClone = Instantiate(_waveProgression, transform);
        _waveProgressionClone.transform.localScale = new Vector3(-0.0005f, 0.0005f, 0.0005f);
        _waveProgressionClone.transform.localPosition = new Vector3(0.004f, 0.0115f, -0.005f);

        //Remaining lives
        _livesClone = Instantiate(_lives, transform);
        _livesClone.transform.localScale = new Vector3(-0.0005f, 0.0005f, 0.0005f);
        _livesClone.transform.localPosition = new Vector3(-0.01f, 0.0115f, -0.002f);

        //Credits
        _creditsClone = Instantiate(_credits, transform);
        _creditsClone.transform.localScale = new Vector3(-0.0005f, 0.0005f, 0.0005f);
        _creditsClone.transform.localPosition = new Vector3(-0.01f, 0.0115f, 0.002f);
    }

    // Update is called once per frame
    private void Update()
    {
        //Score update
        _score.text = $"Score: {Score}";
        _scoreClone.text = _score.text;

        //Time update
        Timer += Time.deltaTime;
        var minutes = Mathf.Floor(Timer / 60).ToString("00");
        var seconds = Mathf.Floor(Timer % 60).ToString("00");
        _timer.text = $"Time: {minutes}:{seconds}";
        _timerClone.text = _timer.text;

        //Time until next wave update
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
                    _timerToNextWave.text = "No new waves.";
                    _timerToNextWaveClone.text = _timerToNextWave.text;
                    return;
                }

                _timeToNextWave = TimeBetweenWaves;
            }
            var minutesToNextWave = Mathf.Floor(_timeToNextWave / 60).ToString("00");
            var secondsToNextWave = Mathf.Ceil(_timeToNextWave % 60).ToString("00");
            _timerToNextWave.text = $"Time to next wave: {minutesToNextWave}:{secondsToNextWave}";
            _timerToNextWaveClone.text = _timerToNextWave.text;
        }

        //Wave progression update
        _waveProgression.text = $"Wave: {WaveNumber} / {TotalNumberOfWaves}";
        _waveProgressionClone.text = _waveProgression.text;

        //Lifes update
        _remaininglives = _player.Lives;
        _lives.text = $"Lives: {_remaininglives}";
        _livesClone.text = _lives.text;

        //Credit update
        _playercredits = _player.Funds;
        _credits.text = $"Credits: {_playercredits}";
        _creditsClone.text = _credits.text;
    }

    //Points gained when enemy dies
    public void PointGain(float points)
    {
        Score += points;
    }
}