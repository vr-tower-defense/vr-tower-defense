using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class FallbackController : MonoBehaviour
{
    public Dial Dial;

    private Hand _hand;

    private GameObject _preview;

    void Start()
    {
        _hand = GetComponent<Hand>();

        var overlayCanvasText = Player.instance.GetComponentInChildren<Text>();
        overlayCanvasText.text = CreateDialOptionString(Dial.DialOptions);
    }

    void Update()
    {
        for (int i = 0; i < Dial.DialOptions.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                OnPressStart(Dial.DialOptionInstances[i]);
            }

            if (Input.GetKeyUp(KeyCode.Alpha1 + i))
            {
                OnRelease(Dial.DialOptionInstances[i]);
            }
        }
    }

    private void OnPressStart(GameObject gameObject)
    {
        var spawnDialOption = gameObject.GetComponent<SpawnDialOption>();

        var playerStatistics = Player.instance.GetComponent<PlayerStatistics>();
        var fallbackHand = Player.instance.GetHand(0);

        if (playerStatistics.Funds < spawnDialOption.Preview.Price)
        {
            return;
        }

        _preview = Instantiate(spawnDialOption.Preview.gameObject);

        fallbackHand.AttachObject(_preview, GrabTypes.None);
    }

    private void OnRelease(GameObject gameObject)
    {
        if (_preview == null)
        {
            return;
        }

        var buildable = _preview.GetComponent<Buildable>();

        // Destroy clone and replace with "real" instance
        Destroy(_preview);

        // Create final instance when position is valid
        if (!buildable.IsPositionValid)
        {
            return;
        }

        buildable.SendMessage(
            "OnBuild",
            transform,
            SendMessageOptions.RequireReceiver
        );
    }

    /// <summary>
    /// <para>
    /// This function uses two StringBuilders to make a list
    /// of the names of the dialoptions and a padded list of the indexed.
    /// The result is combined and returned with an extra newline.
    /// </para>
    /// <para>
    /// ex:
    /// (dialOptionBuilder): [first option], [second option]
    /// (dialIndexBuilder) :       1                2
    /// </para>
    /// </summary>
    private string CreateDialOptionString(SpawnDialOption[] dialOptions)
    {
        var dialInfo = "";

        for (int i = 0; i < dialOptions.Length; i++)
        {
            dialInfo += $"{dialOptions[i].name}: {i} \n";
        }

        return dialInfo;
    }
}