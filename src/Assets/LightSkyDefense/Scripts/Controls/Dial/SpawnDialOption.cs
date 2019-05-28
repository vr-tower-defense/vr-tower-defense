using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SpawnDialOption : DialOption
{
    public GameObject Preview;
    public GameObject Final;

    public float Cost = 0;
    public GameObject DialCostText;

    private PlayerStatistics _playerStatistics;
    private GameObject _textMesh;

    /// <summary>
    /// Instance that is currently being placed
    /// </summary>
    private GameObject _preview;

    private void Awake()
    {
        _playerStatistics = Player.instance.GetComponent<PlayerStatistics>();
        SetTextMesh();  
    }

    private void FixedUpdate()
    {
        _textMesh.transform.rotation = Player.instance.headCollider.transform.rotation;
    }

    /// <summary>
    /// Create new instance of `prefab` and attach it to player hand
    /// </summary>
    public override void OnPressStart(SteamVR_Action_Vector2 action)
    {
        if (_playerStatistics.Funds < Cost)
        {
            return;
        }

        var handTransform = Player.instance.rightHand.transform;

        _preview = Instantiate(
            Preview,
            handTransform.position,
            handTransform.rotation,
            Player.instance.rightHand.transform
        );
    }

    /// <summary>
    /// Detach object from hand
    /// </summary>
    public override void OnPressUp(SteamVR_Action_Vector2 action)
    {
        if (_playerStatistics.UpdateFunds(Cost) == false)
        {
            return;
        }

        var handTransform = Player.instance.rightHand.transform;

        // Destroy clone and replace with "real" instance
        Destroy(_preview);
        Instantiate(Final, handTransform.position, handTransform.rotation);
    }

    // Applies an upwards force to all rigidbodies that enter the trigger.
    void OnTriggerStay(Collider collider)
    {
        //Debug.Log("bAammm collision" + collider.name);
    }

    // Applies an upwards force to all rigidbodies that enter the trigger.
    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("bAammm collision enter" + collider.name);
    }

    private void SetTextMesh()
    {
        var costMesh = DialCostText.GetComponent<TextMesh>();
        costMesh.text = Cost.ToString();

        _textMesh = Instantiate(DialCostText, gameObject.transform);
    }
}
