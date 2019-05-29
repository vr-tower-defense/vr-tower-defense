using System;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Credit : MonoBehaviour
{
    [HideInInspector]
    public float Value;

    public AudioClip PickupSound;
    public Tuple<float, float> PitchRange = new Tuple<float, float>(.8f, 1.2f);

    public float RotationSpeed = 35;

    public void FixedUpdate()
    {
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// When the credit touches the hand, the credit gets added to the players total credit count
    /// </summary>
    /// <param name="hand"></param>
    private void OnHandHoverBegin(Hand hand)
    {
        var platerStatistics = Player
            .instance
            .GetComponent<PlayerStatistics>();

        platerStatistics?.UpdateFunds(Value);

        SoundUtil.PlayClipAtPointWithRandomPitch(
            PickupSound, 
            gameObject.transform.position, 
            PitchRange.Item1,
            PitchRange.Item2
        );

        Destroy(gameObject);
    }
}
