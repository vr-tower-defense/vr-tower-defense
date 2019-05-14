using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Vector3 offsetTranslation { get; private set; }

    [HideInInspector]
    public int PathPointIndex { get; private set; }

    [HideInInspector]
    public Vector3 PreviousPosition { get; private set; }

    private Path _path;

    private static readonly float _distance = 0.00808f;
    private static readonly float _pathThickness = .08f;

    void Start()
    {
        _path = GameManager.Instance.Path;
        transform.parent = null;

        transform.position = _path.PathPoints[0];
        offsetTranslation = new Vector3(Random.Range(-_pathThickness, _pathThickness), Random.Range(-_pathThickness, _pathThickness), Random.Range(-_pathThickness, _pathThickness));
    }

    void FixedUpdate()
    {
        Vector3 f = Vector3.MoveTowards(transform.position, _path.PathPoints[PathPointIndex], 0.002f);

        PreviousPosition = transform.position;
        transform.position = f;

        if (Vector3.Distance(transform.position, _path.PathPoints[PathPointIndex]) < _distance)
        {
            PathPointIndex = Mathf.Clamp(PathPointIndex + 1, 0, _path.PathPoints.Length - 1);
        }
    }

    public void UpdatePathPointIndex(int newIndex)
    {
        transform.position = _path.PathPoints[newIndex];
        offsetTranslation = new Vector3();

        PathPointIndex = newIndex;
    }
}