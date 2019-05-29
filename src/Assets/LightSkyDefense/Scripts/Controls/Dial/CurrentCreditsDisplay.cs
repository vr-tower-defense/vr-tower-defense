using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CurrentCreditsDisplay : MonoBehaviour
{
    [Tooltip("Credit text prefab")]
    public GameObject CreditTextPrefab;

    [Tooltip("Small offset towards wrist, from middle of dial")]
    public float CreditsDialOffset = -0.2f;

    private GameObject _creditsText;

    void Start()
    {
        _creditsText = Instantiate(
            CreditTextPrefab,
            gameObject.transform.position + new Vector3(0, 0, CreditsDialOffset),
            gameObject.transform.rotation
        );
    }

    void Update()
    {
        _creditsText.transform.rotation = Player.instance.headCollider.transform.rotation;
    }

    void OnPlayerStatisticsUpdate(PlayerStatistics playerStatistics)
    {
        var textMesh = _creditsText.GetComponent<TextMesh>();
        textMesh.text = "Currency:\n" + playerStatistics.Funds;
    }
}
