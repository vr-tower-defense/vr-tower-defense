using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(Interactable))]
public class Repairable : MonoBehaviour
{
    public float HealPerSecond = .5f;

    public float HealCheckInterval = .2f;

    public float HapticPulseDuration = .1f;

    public float HapticPulseFrequency = 35f;

    private Damageable _damageable;

    public void Start()
    {
        _damageable = GetComponent<Damageable>();
    }

    private void OnHandHoverBegin(Hand hand)
    {
        StartCoroutine(HealDamageable(hand));
    }

    private void OnHandHoverEnd(Hand hand)
    {
        StopAllCoroutines();
    }

    private IEnumerator HealDamageable(Hand hand)
    {
        // No need to repair when tower is full health
        if (_damageable.Health >= _damageable.MaxHealth)
        {
            // Invoke this method recursively
            yield return new WaitForSeconds(HealCheckInterval);
            yield return HealDamageable(hand);

        }

        // We need to divide by 100 to get the health that should be added every 100ms
        _damageable.UpdateHealth(HealPerSecond * HealCheckInterval);

        // Apply haptic feedback during heal process
        hand.TriggerHapticPulse(HapticPulseDuration, HapticPulseFrequency, 1);

        // Invoke this method recursively
        yield return new WaitForSeconds(HealCheckInterval);
        yield return HealDamageable(hand);
    }
}
