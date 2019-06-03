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
            var activeScript = Dial.DialOptionInstances[i].GetComponent<SpawnDialOption>();

            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                activeScript.OnPressStart(new SteamVR_Action_Vector2());
            }

            if (Input.GetKeyUp(KeyCode.Alpha1 + i))
            {
                activeScript.OnRelease(new SteamVR_Action_Vector2());
            }
        }
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