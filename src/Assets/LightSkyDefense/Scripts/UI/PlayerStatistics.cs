using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.InteractionSystem;

public class PlayerStatistics : MonoBehaviour
{

    private int _lives;

    public int StartLives = 5;
    public int StartCredits = 20;

    public float Credits { get; set; } = 20;

    public int Lives
    {
        get => _lives;
        set
        {
            _lives = value;

            if (_lives > 0 || _lost)
                return;

            _lost = true;
            GameObject[] targets = gameObject.scene.GetRootGameObjects();

            targets.ForEach(target => 
                ExecuteEvents.Execute<IOnGameLossTarget>(
                    target,
                    null, 
                    (handler, _) => handler.OnGameLoss()
                )
            );
        }
    }

    private bool _lost = false;

    public void Start()
    {
        Lives = StartLives;
        Credits = StartCredits;
    }
}
