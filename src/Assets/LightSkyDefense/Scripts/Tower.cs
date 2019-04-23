using UnityEngine;
using Valve.VR.InteractionSystem;

public class Tower : MonoBehaviour
{
    public int Cost;
    public AudioClip BuildSound;
    public void Start()
    {
        AudioSource source = gameObject.GetComponent<AudioSource>();
        var creditOwner = Player.instance.GetComponent<CreditOwner>();

        if (creditOwner.Credits < Cost)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            source?.PlayOneShot(BuildSound);
            creditOwner.Credits -= Cost;
        }
    }
}

