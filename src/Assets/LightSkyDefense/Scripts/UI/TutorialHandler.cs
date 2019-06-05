using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TutorialHandler : MonoBehaviour
{
    public Transform VideoPlayerParent;

    [Header("Button hints")]
    public float ButtonHintTimeout = 2.5f;
    public SteamVR_Action_Boolean PlayTutorialAction = SteamVR_Input.GetBooleanAction("PlayTutorial");
    public SteamVR_Input_Sources Hand = SteamVR_Input_Sources.LeftHand;
    public string PlayTutorialActionHint = "Start tutorial";
    public string SkipTutorialActionHint = "Go to next slide";

    private VideoPlayer _videoPlayer;
    private readonly Queue<IEnumerator> _videoActions = new Queue<IEnumerator>(4);

    private IEnumerator _tutorialEnumerator;

    void Start()
    {
        _videoPlayer = VideoPlayerParent.GetComponentInChildren<VideoPlayer>(true);
        _videoPlayer?.Prepare();

        StartCoroutine(ProcessQueue(_videoActions));
        StartCoroutine(InitializeTutorial());
    }

    void Update()
    {
        if (!PlayTutorialAction.GetStateDown(SteamVR_Input_Sources.Any))
        {
            return;
        }

        _tutorialEnumerator?.MoveNext();
    }

    private IEnumerator InitializeTutorial()
    {
        yield return new WaitForSeconds(ButtonHintTimeout);

        _tutorialEnumerator = CreateTutorial();
        _tutorialEnumerator.MoveNext();
    }

    public IEnumerator CreateTutorial()
    {
        var hand = Player.instance.GetHand(Hand);

        // Show start hints
        ControllerButtonHints.HideTextHint(hand, PlayTutorialAction);
        ControllerButtonHints.ShowTextHint(hand, PlayTutorialAction, PlayTutorialActionHint);

        yield return null;

        // Show skip hints
        ControllerButtonHints.HideTextHint(hand, PlayTutorialAction);
        ControllerButtonHints.ShowTextHint(hand, PlayTutorialAction, SkipTutorialActionHint);

        //Show "You" slide
        _videoActions.Enqueue(PlayForSec(_videoPlayer, 3.5f));
        yield return null;

        //Show "Enemy" slide
        _videoActions.Enqueue(PlayForSec(_videoPlayer, 3.8f));
        yield return null;

        //Show "Path" slide
        _videoActions.Enqueue(PlayForSec(_videoPlayer, 4.5f));
        yield return null;

        //Show "Path Game Preview" slide
        _videoActions.Enqueue(SeekSec(_videoPlayer, 15.0f));
        _videoActions.Enqueue(PlayForSec(_videoPlayer, 3.5f));
        yield return null;

        _videoActions.Enqueue(SeekSec(_videoPlayer, 33.0f));
        _videoActions.Enqueue(PlayForSec(_videoPlayer, 26.8f));
        yield return null;
    }

    public IEnumerator ProcessQueue(Queue<IEnumerator> queue)
    {
        while (true)
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
