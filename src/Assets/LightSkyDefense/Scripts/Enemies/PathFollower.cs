using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Vector3 OffsetTranslation { get; private set; }

    [HideInInspector]
    public int PathPointIndex { get; private set; }

    [HideInInspector]
    public Vector3 PreviousPosition { get; private set; }

    private static readonly float _distance = 0.00808f;
    private static readonly float _pathThickness = .08f;

    void Start()
    {
        transform.parent = null;
        transform.position = Path.Instance[0];

        OffsetTranslation = new Vector3(
            Random.Range(-_pathThickness, _pathThickness),
            Random.Range(-_pathThickness, _pathThickness),
            Random.Range(-_pathThickness, _pathThickness)
        );
    }

    void FixedUpdate()
    {
        var newPosition = Vector3.MoveTowards(transform.position, Path.Instance[PathPointIndex], 0.002f);

        PreviousPosition = transform.position;
        transform.position = newPosition;

        if (Vector3.Distance(transform.position, Path.Instance[PathPointIndex]) < _distance)
        {
            PathPointIndex = Mathf.Clamp(
                PathPointIndex + 1,
                0,
                Path.Instance.PointCount - 1
            );
        }
    }

    public void UpdatePathPointIndex(int newIndex)
    {
        transform.position = Path.Instance[newIndex];
        OffsetTranslation = new Vector3();

        PathPointIndex = newIndex;
    }
}
