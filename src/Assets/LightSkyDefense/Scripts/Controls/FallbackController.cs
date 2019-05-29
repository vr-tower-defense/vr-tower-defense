using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
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
                _preview = Instantiate(
                    Dial.DialOptions[i].Preview
                );

                _preview.AddComponent<Interactable>();
                // Use AttachObject because otherwise the hand indicator will interfere.
                _hand.AttachObject(_preview, GrabTypes.None);
            }

            if (Input.GetKeyUp(KeyCode.Alpha1 + i))
            {
                // Destroy clone and replace with "real" instance
                _hand.DetachObject(_preview);

                var buildable = _preview.GetComponent<Buildable>();

                // Create final instance when position is valid
                if (!buildable.IsPositionValid)
                {
                    return;
                }

                buildable.SendMessage(
                    "OnBuild",
                    _hand.transform,
                    SendMessageOptions.RequireReceiver
                );

                // Destroy clone and replace with "real" instance
                Destroy(_preview);
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
    ///                    :
    /// </para>
    /// </summary>
    private string CreateDialOptionString(SpawnDialOption[] dialOptions)
    {
        var dialOptionBuilder = new StringBuilder();
        var dialIndexBuilder = new StringBuilder();

        for (int i = 0; i < dialOptions.Length; i++)
        {
            dialOptionBuilder.Append("[");
            dialOptionBuilder.Append(dialOptions[i].name);
            dialOptionBuilder.Append("], ");

            dialIndexBuilder.Insert(dialIndexBuilder.Length, " ", dialOptions[i].name.Length / 2 + 1);
            dialIndexBuilder.Append(i + 1);
            dialIndexBuilder.Insert(dialIndexBuilder.Length, " ", dialOptions[i].name.Length / 2 + 3);
        }

        dialOptionBuilder.Remove(dialOptionBuilder.Length - 2, 2);
        dialOptionBuilder.Append("\n");
        dialOptionBuilder.AppendLine(dialIndexBuilder.ToString());
        dialOptionBuilder.Append("\n");

        return dialOptionBuilder.ToString();
    }
}