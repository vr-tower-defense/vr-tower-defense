using UnityEngine;

public class CurrentCreditsDisplay : MonoBehaviour
{
    [Tooltip("Credit text prefab")]
    public TextMesh TextMesh;

    [Multiline]
    public string Prefix = "Credits: ";

    [Multiline]
    public string Suffix = string.Empty;

    void OnPlayerStatisticsUpdate(PlayerStatistics playerStatistics)
    {
        TextMesh.text = $"{Prefix}{playerStatistics.Funds}{Suffix}";
    }
}
