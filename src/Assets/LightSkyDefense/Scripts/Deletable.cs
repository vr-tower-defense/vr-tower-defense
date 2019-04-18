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
    private bool _eventSet = false;
    public AudioClip DestroySound;

    private static GameObject _currentHover;
    public SteamVR_Action_Boolean TriggerClickAction = SteamVR_Input.GetBooleanAction("TriggerClick");
    /// <summary>
    /// Valdiate properties
    /// </summary>
    public void Start()
    {
        // Validate actions
        if (TriggerClickAction == null)
            Debug.LogError("`TriggerClick` action has not been set on this component.");

        //used to check if we need to add to the event
        else if (!_eventSet)
        {
            TriggerClickAction.onStateDown += delegate { DestroyCurrentHover(); };
            _eventSet = true;
        }
    }

    private void DestroyCurrentHover()
    {
        if (_currentHover != null)
        {
            AudioSource.PlayClipAtPoint(DestroySound, this.gameObject.transform.position);
            Destroy(_currentHover);
        }
    }

    /// <summary>
    /// If Hovering begins, and the TriggerClickAction State is true, the tower gets destroyed
    /// Otherwise it becomes the _currentHover object
    /// </summary>
    /// <param name="hand"></param>
    private void OnHandHoverBegin(Hand hand)
    {
        if (TriggerClickAction.state)
        {
            AudioSource.PlayClipAtPoint(DestroySound, this.gameObject.transform.position);
            Destroy(this.gameObject);
        }
        else
            _currentHover = this.gameObject;
        
    }

    /// <summary>
    /// IF the _currentHover is the gameObject, remove it.
    /// </summary>
    /// <param name="hand"></param>
    private void OnHandHoverEnd(Hand hand)
    {
        if (_currentHover == this.gameObject)
            _currentHover = null;
    }

}
