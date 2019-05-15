using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR.InteractionSystem;

public class FallbackController : MonoBehaviour
{
    private Player _player;
    private Hand _fallbackHand;

    private SpawnDialOption[] _dialOptions;

    private GameObject _preview;

    void Start()
    {
        _player = Player.instance;

        if (!XRSettings.enabled)
        {
            // No HDM found, put options in fallback overlay

            var overlayCanvasText = _player.GetComponentInChildren<Text>();
            var playerDial = _player.GetComponentInChildren<Dial>(true);

            _dialOptions = playerDial.DialOptions;

            var dailOptionBuilder = new StringBuilder();
            var dailIndexBuilder = new StringBuilder();

            for (int i = 0; i < playerDial.DialOptions.Length; i++)
            {
                dailOptionBuilder.Append("[");
                dailOptionBuilder.Append(playerDial.DialOptions[i].name);
                dailOptionBuilder.Append("], ");

                dailIndexBuilder.Insert(dailIndexBuilder.Length, " ", playerDial.DialOptions[i].name.Length / 2 + 1);
                dailIndexBuilder.Append(i + 1);
                dailIndexBuilder.Insert(dailIndexBuilder.Length, " ", playerDial.DialOptions[i].name.Length / 2 + 3);
            }
            dailOptionBuilder.Remove(dailOptionBuilder.Length - 2, 2);
            dailOptionBuilder.Append("\n");
            dailOptionBuilder.AppendLine(dailIndexBuilder.ToString());
            dailOptionBuilder.Append("\n");

            overlayCanvasText.text = dailOptionBuilder.ToString();
            
            foreach (Hand hand in _player.GetComponentsInChildren<Hand>())
            {
                if (hand.name.Equals("FallbackHand"))
                {
                    _fallbackHand = hand;
                }
            }
        }
        else
        {
            // Disable FallBackController if player is in actual VR
            enabled = false;
        }
    }

    void Update()
    {
        for (int i = 0; i < _dialOptions.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                _preview = Instantiate(
                    _dialOptions[i].Preview
                );

                _preview.AddComponent<Interactable>();
                // Use AttachObject because otherwise the hand indicator will interfere.
                _fallbackHand.AttachObject(_preview, GrabTypes.None);
            }

            if (Input.GetKeyUp(KeyCode.Alpha1 + i))
            {
                // Destroy clone and replace with "real" instance
                _fallbackHand.DetachObject(_preview);
                Destroy(_preview);
                Instantiate(_dialOptions[i].Final, _fallbackHand.hoverSphereTransform.position, _fallbackHand.hoverSphereTransform.rotation);
            }
        }
    }
}