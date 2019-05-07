using UnityEngine;

public class Scoreboard : MonoBehaviour, IOnGameLossTarget
{
    public float Score;
    public float Timer;
    public float TimeBetweenWaves = 0;
    public float WaveNumber;
    public float TotalNumberOfWaves = 15;
    public AudioClip BossSpawnedClip;

    private float _remaininglives;
    private float _playercredits;
    private float _timeToNextWave;
    private AudioSource _source;
    private TextMesh _score;
    private TextMesh _timer;
    private TextMesh _timerToNextWave;
    private TextMesh _lives;
    private TextMesh _waveProgression;
    private TextMesh _credits;


    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _timeToNextWave = TimeBetweenWaves;
        //SetTextToScoreboard(_score, 30, 0.0005f, 0.0005f, 0.0005f, TextAnchor.MiddleLeft, -0.005f, -0.011f, -0.005f);


        GameObject scoreText = new GameObject();
        _score = scoreText.AddComponent<TextMesh>();
        _score.fontSize = 30;
        _score.transform.SetParent(this.transform);
        _score.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        _score.anchor = TextAnchor.MiddleLeft;
        _score.transform.localPosition = new Vector3(-0.01f, -0.011f, -0.002f);
        //_score.transform.localEulerAngles += new Vector3(0, 0, 0);
        //_score.font = Resources.Load<Font>("Fonts/Potra");
        Instantiate(scoreText, new Vector3(-0.01f, -0.011f, -0.002f), Quaternion.Euler(0, 0, 180), this.transform);


        GameObject timerText = new GameObject();
        _timer = timerText.AddComponent<TextMesh>();
        _timer.fontSize = 30;
        //_score.font = Resources.Load<Font>("Fonts/Potra");
        _timer.transform.SetParent(this.transform);
        _timer.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        _timer.anchor = TextAnchor.MiddleLeft;
        //_timer.transform.localEulerAngles += new Vector3(90, 0, 0);
        _timer.transform.localPosition = new Vector3(-0.01f, -0.011f, 0.002f);

        GameObject timerToNextWaveText = new GameObject();
        _timerToNextWave = timerToNextWaveText.AddComponent<TextMesh>();
        _timerToNextWave.fontSize = 30;
        //_score.font = Resources.Load<Font>("Fonts/Potra");
        _timerToNextWave.transform.SetParent(this.transform);
        _timerToNextWave.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        _timerToNextWave.anchor = TextAnchor.MiddleLeft;
        //_timer.transform.localEulerAngles += new Vector3(90, 0, 0);
        _timerToNextWave.transform.localPosition = new Vector3(-0.008f, -0.011f, 0.005f);

        GameObject waveProgressionText = new GameObject();
        _waveProgression = waveProgressionText.AddComponent<TextMesh>();
        _waveProgression.fontSize = 30;
        //_score.font = Resources.Load<Font>("Fonts/Potra");
        _waveProgression.transform.SetParent(this.transform);
        _waveProgression.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        _waveProgression.anchor = TextAnchor.MiddleLeft;
        //_timer.transform.localEulerAngles += new Vector3(90, 0, 0);
        _waveProgression.transform.localPosition = new Vector3(-0.004f, -0.011f, -0.005f);

        GameObject remainingLivesText = new GameObject();
        _lives = remainingLivesText.AddComponent<TextMesh>();
        _lives.fontSize = 30;
        //_score.font = Resources.Load<Font>("Fonts/Potra");
        _lives.transform.SetParent(this.transform);
        _lives.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        _lives.anchor = TextAnchor.MiddleRight;
        //_timer.transform.localEulerAngles += new Vector3(90, 0, 0);
        _lives.transform.localPosition = new Vector3(0.01f, -0.011f, -0.002f);

        GameObject creditsText = new GameObject();
        _credits = creditsText.AddComponent<TextMesh>();
        _credits.fontSize = 30;
        //_score.font = Resources.Load<Font>("Fonts/Potra");
        _credits.transform.SetParent(this.transform);
        _credits.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        _credits.anchor = TextAnchor.MiddleRight;
        //_timer.transform.localEulerAngles += new Vector3(90, 0, 0);
        _credits.transform.localPosition = new Vector3(0.01f, -0.011f, 0.002f);
    }

    //void SetTextToScoreboard(TextMesh text, int fontsize, 
    //    float scaleX, float scaleY, float scaleZ, TextAnchor anchor,
    //    float positionX, float positionY, float positionZ)
    //{
    //    GameObject textObject = new GameObject();
    //    TextMesh _text = text;
    //    _text = textObject.AddComponent<TextMesh>();
    //    _text.fontSize = fontsize;
    //    _text.transform.SetParent(this.transform);
    //    _text.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    //    _text.anchor = anchor;
    //    _text.transform.localPosition = new Vector3(positionX, positionY, positionZ);
    //}

    // Update is called once per frame
    void Update()
    {
        //Score update
        _score.text = $"Score: ";

        //Time update
        Timer += Time.deltaTime;
        string minutes = Mathf.Floor(Timer / 60).ToString("00");
        string seconds = Mathf.Floor(Timer % 60).ToString("00");
        _timer.text = $"Time: {minutes}:{seconds}";

        //Time until next wave update
        if (WaveNumber != TotalNumberOfWaves)
        {
            _timeToNextWave -= Time.deltaTime;
        }

        if (_timeToNextWave <= 0.0f && WaveNumber != TotalNumberOfWaves)
        {
            WaveNumber++;
            if (WaveNumber % 5 == 0)
            {
                _source?.PlayOneShot(BossSpawnedClip);
            }
            else
            {
                _source.Play();
            }

            if (WaveNumber == TotalNumberOfWaves)
            {
                _timerToNextWave.text = "No new waves.";
                return;
            }

            _timeToNextWave = TimeBetweenWaves;
        }

        if (WaveNumber != TotalNumberOfWaves)
        {
            string minutesToNextWave = Mathf.Floor(_timeToNextWave / 60).ToString("00");
            string secondsToNextWave = Mathf.Ceil(_timeToNextWave % 60).ToString("00");
            _timerToNextWave.text = $"Time to next wave: {minutesToNextWave}:{secondsToNextWave}";
        }

        //Wave progression update
        _waveProgression.text = $"Wave: {WaveNumber} / {TotalNumberOfWaves}"; ;

        //Lifes update
        _remaininglives = GameObject.Find("Player").GetComponent<PlayerStats>().Lives;
        _lives.text = $"Lives: {_remaininglives}";

        //Credit update
        _playercredits = GameObject.Find("Player").GetComponent<PlayerStats>().Credits;
        _credits.text = $"Credits: {_playercredits}";
    }

    public void OnGameLoss()
    {
        _remaininglives = GameObject.Find("Player").GetComponent<PlayerStats>().Lives;
        _lives.text = $"Lives: {_remaininglives}";
        enabled = false;
    }

    public void PointGain(float points)
    {
        Score += points;
    }
}
