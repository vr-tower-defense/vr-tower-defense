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
        }
        else
        {
            //constructie geluid hier
            creditOwner.Credits -= Cost;
        }
    }
}

