using UnityEngine;
using Valve.VR.InteractionSystem;

public class Tower : MonoBehaviour
{
    public int Cost;

    public void Start()
    {
        var creditOwner = Player.instance.GetComponent<CreditOwner>();

        if (creditOwner.Credits < Cost)
        {
            Destroy(gameObject);
            return;
        }

        // TODO Play tower build sound effect
        creditOwner.Credits -= Cost;
    }
}

