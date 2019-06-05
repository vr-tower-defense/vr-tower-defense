using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialHandler : MonoBehaviour
{
    public Transform VideoPlayerParent;
    public Canvas ButtonCanvas;
    public Button NextButton;

    private VideoPlayer _videoPlayer;

    private int _tutorialIndex;

    private Queue<IEnumerator> _videoActions = new Queue<IEnumerator>(4);

    void Start()
    {
        _videoPlayer = VideoPlayerParent.GetComponentInChildren<VideoPlayer>(true);
        _videoPlayer?.Prepare();
        StartCoroutine(ProcessQueue(_videoActions));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnTutorialClick();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnStartClick();
        }
    }

    public void OnTutorialClick()
    {
        if (_videoPlayer == null)
            return;

        ButtonCanvas?.gameObject.SetActive(false);

        switch (_tutorialIndex)
        {
            case 0:
                //Show "You" slide
                _tutorialIndex++;
                VideoPlayerParent.gameObject.SetActive(true);
                NextButton.gameObject.SetActive(false);
                _videoActions.Enqueue(PlayForSec(_videoPlayer, 3.5f, () => NextButton.gameObject.SetActive(true)));
                break;
            case 1:
                //Show "Enemy" slide
                _tutorialIndex++;
                NextButton.gameObject.SetActive(false);
                _videoActions.Enqueue(PlayForSec(_videoPlayer, 3.8f, () => NextButton.gameObject.SetActive(true)));
                break;
            case 2:
                //Show "Path" slide
                _tutorialIndex++;
                NextButton.gameObject.SetActive(false);
                _videoActions.Enqueue(PlayForSec(_videoPlayer, 4.5f, () => NextButton.gameObject.SetActive(true)));
                break;
            case 3:
                //Show "Path Game Preview" slide
                _tutorialIndex++;
                NextButton.gameObject.SetActive(false);
                _videoActions.Enqueue(SeekSec(_videoPlayer, 15.0f));
                _videoActions.Enqueue(PlayForSec(_videoPlayer, 3.5f, () => NextButton.gameObject.SetActive(true)));
                break;
            case 4:
                //Show "Tower Placement" slide
                _tutorialIndex++;
                NextButton.gameObject.SetActive(false);
                _videoActions.Enqueue(SeekSec(_videoPlayer, 33.0f));
                _videoActions.Enqueue(PlayForSec(_videoPlayer, 26.8f, () => NextButton.gameObject.SetActive(true)));
                break;
            default:
                //End presetation, show lobby menu again
                _tutorialIndex = 0;
                NextButton.gameObject.SetActive(false);
                VideoPlayerParent.gameObject.SetActive(false);
                ButtonCanvas?.gameObject.SetActive(true);
                break;
        }
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("LSDScene");
    }

    public IEnumerator ProcessQueue(Queue<IEnumerator> queue)
    {
        while(true)
        {
            if (queue.Count > 0)
            {
                yield return StartCoroutine(queue.Dequeue());
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public IEnumerator PlayForSec(VideoPlayer videoPlayer, float playingTime, UnityAction callback = null)
    {
        videoPlayer.Play();
        yield return new WaitForSeconds(playingTime);
        videoPlayer.Pause();

        callback?.Invoke();
    }

    public IEnumerator RepeatSec(VideoPlayer videoPlayer, float playingTime)
    {
        double startTime = videoPlayer.time;

        while (true)
        {
            videoPlayer.Play();
            yield return new WaitForSeconds(playingTime);
            videoPlayer.time = startTime;
        }
    }

    public IEnumerator SeekSec(VideoPlayer videoPlayer, double seekTime, UnityAction callback = null)
    {
        videoPlayer.time = seekTime;
        videoPlayer.Play();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        videoPlayer.Pause();

        callback?.Invoke();
    }
}
