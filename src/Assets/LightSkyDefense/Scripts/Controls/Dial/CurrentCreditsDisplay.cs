using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CurrentCreditsDisplay : MonoBehaviour
{
    [Tooltip("Credit text prefab")]
    public GameObject CreditTextPrefab;

    [Tooltip("Small offset towards wrist, from middle of dial")]
    public Vector3 CreditsDialOffset = new Vector3(0, 0, -0.175f);

    private TextMesh _textMesh;

    void Start()
    {
        var prefabInstance = Instantiate(
            CreditTextPrefab,
            gameObject.transform.position + CreditsDialOffset,
            gameObject.transform.rotation,
            gameObject.transform
        );

        _textMesh = prefabInstance.GetComponentInChildren<TextMesh>();
    }

    void OnPlayerStatisticsUpdate(PlayerStatistics playerStatistics)
    {
        _textMesh.text = $"Currency: \n {playerStatistics.Funds}";
    }
}
