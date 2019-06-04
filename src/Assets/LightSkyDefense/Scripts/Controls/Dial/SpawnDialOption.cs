using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SpawnDialOption : DialOption
{
    public Buildable Preview;

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
        if (_playerStatistics.Funds < Preview.Price)
        {
            return;
        }

        _preview = Instantiate(
            Preview.gameObject,
            transform.position,
            transform.rotation,
            transform
        );
    }

    /// <summary>
    /// Detach object from hand
    /// </summary>
    public override void OnRelease(SteamVR_Action_Vector2 action)
    {
        if (_preview == null)
        {
            return;
        }

        var buildable = _preview.GetComponent<Buildable>();

        // Destroy clone and replace with "real" instance
        Destroy(_preview);

        // Create final instance when position is valid
        if (!buildable.IsPositionValid)
        {
            return;
        }

        buildable.SendMessage(
            "OnBuild",
            transform,
            SendMessageOptions.RequireReceiver
        );
    }

    private void SetTextMesh()
    {
        var costMesh = DialCostText.GetComponent<TextMesh>();
        costMesh.text = Preview.Price.ToString();

        _textMesh = Instantiate(DialCostText, gameObject.transform);
    }
}
