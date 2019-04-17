using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


[RequireComponent(typeof(Interactable))]
public class Deletable : MonoBehaviour
{
    public SteamVR_Action_Boolean TriggerClickAction = SteamVR_Input.GetBooleanAction("TriggerClick");

    /// <summary>
    /// Valdiate properties
    /// </summary>
    public void Start()
    {
        // Validate actions
        if (TriggerClickAction == null)
            Debug.LogError("`TriggerClick` action has not been set on this component.");

        if (TriggerClickAction != null)
            TriggerClickAction.onStateDown += (action, source) => deleteIfHover();

    }

    private void deleteIfHover()
    {
        if (gameObject.GetComponent<Interactable>().isHovering)
            Destroy(gameObject);
    }
}