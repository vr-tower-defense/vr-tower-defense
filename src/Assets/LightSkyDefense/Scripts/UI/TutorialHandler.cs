using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TutorialHandler : MonoBehaviour
{
    public Transform VideoPlayerParent;

    private VideoPlayer _videoPlayer;

    private int _tutorialIndex = 0;

    private Queue<IEnumerator> _videoActions = new Queue<IEnumerator>(4);

    void Start()
    {
        _videoPlayer = VideoPlayerParent.GetComponentInChildren<VideoPlayer>(true);
        print(_videoPlayer);
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

        Canvas canvas = GetComponentInChildren<Canvas>();

        canvas?.gameObject.SetActive(false);

        switch (_tutorialIndex)
        {
            case 0:
                //Show you
                _tutorialIndex++;
                VideoPlayerParent.gameObject.SetActive(true);
                _videoActions.Enqueue(PlayForSec(_videoPlayer, 3.5f));
                break;
            case 1:
                //Show Enemy
                _tutorialIndex++;
                _videoActions.Enqueue(PlayForSec(_videoPlayer, 3.8f));
                break;
            case 2:
                //Show path
                _tutorialIndex++;
                _videoActions.Enqueue(PlayForSec(_videoPlayer, 4.5f));
                break;
            case 3:
                //Show path in game preview
                _tutorialIndex++;
                _videoActions.Enqueue(SeekSec(_videoPlayer, 15.0f));
                _videoActions.Enqueue(PlayForSec(_videoPlayer, 3.5f));
                break;
            case 4:
                //Show tower placement
                _videoActions.Enqueue(SeekSec(_videoPlayer, 33.0f));
                _videoActions.Enqueue(PlayForSec(_videoPlayer, 26.8f));
                break;
            default:
                _tutorialIndex = 0;
                VideoPlayerParent.gameObject.SetActive(false);
                canvas = GetComponentInChildren<Canvas>(true);
                canvas?.gameObject.SetActive(true);
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
                yield return StartCoroutine(queue.Peek());//todo test to dequeue directly?
                queue.Dequeue();
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
        print("repeatSec time :" + startTime);
        //double startTime = 15.0;
        while (true)
        {
            videoPlayer.Play();
            yield return new WaitForSeconds(playingTime);
            videoPlayer.time = startTime;
        }
    }

    public IEnumerator SeekSec(VideoPlayer videoPlayer, double seekTime, UnityAction callback = null)
    {

        print("starting seek:" + _videoPlayer.time +" and seeking to :"+ seekTime);
        //VideoPlayer.Play();
        //VideoPlayer.Pause();
        _videoPlayer.time = seekTime;
        _videoPlayer.Play();
        while (!videoPlayer.isPrepared)
        {
            print("!isPrepared");
            yield return null;
        }
        _videoPlayer.Pause();
        print("seeked! :" + _videoPlayer.time);
        callback?.Invoke();
    }
}
