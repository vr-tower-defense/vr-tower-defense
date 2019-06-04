using UnityEngine;
using Valve.VR.InteractionSystem;

class LoseState : GameState
{
#pragma warning disable 0649
    public GameObject Prefab;
#pragma warning restore 0649

    public Vector3 Offset = new Vector3(0, 0, 1.5f);

    private GreyscaleAfterEffect _greyScaleEffect;

    private void Awake()
    {
        _greyScaleEffect = Camera.main.GetComponent<GreyscaleAfterEffect>();
    }

    private void OnEnable()
    {
        if (_greyScaleEffect != null)
        {
            _greyScaleEffect.Active = true;
        }

        var player = Player.instance.headCollider.transform;

        Instantiate(
            Prefab,
            player.position + (player.rotation * Offset),
            player.rotation
        );
    }
}
