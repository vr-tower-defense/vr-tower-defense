using UnityEngine;
using Valve.VR.InteractionSystem;

public class Tower : MonoBehaviour
{
    public int Cost;
    public AudioClip BuildSound;
    public void Start()
    {
        var creditOwner = Player.instance.GetComponent<CreditOwner>();

        if (creditOwner.Credits < Cost)
        {
            Destroy(gameObject);
        }
        else
        {
            AudioSource.PlayClipAtPoint(BuildSound, gameObject.transform.position);
            creditOwner.Credits -= Cost;
        }
    }
}

