using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    private readonly float _rotationSpeed = 2f;

    //public Rigidbody LinkedTransform;

    [HideInInspector]
    public Vector3 offsetTranslation { get; private set; }

    private Path _path;

    public int PathPointIndex { get; set; }

    [HideInInspector]
    public Vector3 PreviousPosition { get; private set; }

    private static float _distance = 0.00808f;

    void Start()
    {
        transform.parent = null;
        _path = GameManager.Instance.Path;
        //LinkedTransform.position = _path.PathPoints[0].Position;
        transform.position = _path.PathPoints[0].Position;
        offsetTranslation = new Vector3(Random.Range(-.08f, .08f), Random.Range(-.08f, .08f), Random.Range(-.08f, .08f));
    }
    
    void FixedUpdate()
    {
        //if(LinkedTransform==null)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        Vector3 f = FollowPathPoint.Calculate(transform, 0, 0, _path.PathPoints[PathPointIndex]);

        PreviousPosition = transform.position;
        transform.position = f;
        if(Vector3.Distance(transform.position, _path.PathPoints[PathPointIndex].Position) < _distance)
        {
            PathPointIndex = Mathf.Clamp(PathPointIndex+1, 0, _path.PathPoints.Length-1);
        }
        //LinkedTransform.position = transform.position - offsetTranslation;
    }

}
