using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderableColliders : MonoBehaviour
{
    private Collider[] _colliders;

    private static Mesh _cubeMesh;
    private Mesh CubeMesh
    {
        get
        {
            if (_cubeMesh == null)
            {
                var primativeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _cubeMesh = primativeObject.GetComponent<MeshFilter>().sharedMesh;
                Destroy(primativeObject);
            }
            return _cubeMesh;
        }
        set => _cubeMesh = value;
    }

    private static Mesh _sphereMesh;
    public static Mesh SphereMesh
    {
        get
        {
            if (_sphereMesh == null)
            {
                var primativeObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                _sphereMesh = primativeObject.GetComponent<MeshFilter>().sharedMesh;
                Destroy(primativeObject);
            }
            return _sphereMesh;
        }
        set => _sphereMesh = value;
    }

    private static Mesh _capsuleMesh;
    public static Mesh CapsuleMesh
    {
        get
        {
            if (_capsuleMesh == null)
            {
                var primativeObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                _capsuleMesh = primativeObject.GetComponent<MeshFilter>().sharedMesh;
                Destroy(primativeObject);
            }
            return _capsuleMesh;
        }
        set => _capsuleMesh = value;
    }

    private static Material _colliderMat;
    public static Material ColliderMat
    {
        get
        {
            return _colliderMat ?? (_colliderMat = Resources.Load<Material>("Materials/ColliderRenderMat"));
        }
        set => _colliderMat = value;
    }
  
    void Start()
    {
        _colliders = GetComponentsInChildren<Collider>();

        foreach(Collider collider in _colliders)
        {
            var meshGameObject = new GameObject(collider.ToString());

            var meshFilter = meshGameObject.AddComponent<MeshFilter>();
            var meshRenderer = meshGameObject.AddComponent<MeshRenderer>();

            meshRenderer.material = ColliderMat;

            switch(collider)
            {
                case MeshCollider meshCollider:
                    meshFilter.mesh = meshCollider.sharedMesh;
                    meshGameObject.transform.localScale = collider.transform.localScale;
                    break;
                case BoxCollider boxCollider:
                    meshFilter.mesh = CubeMesh;
                    meshGameObject.transform.localScale = boxCollider.size;
                    break;
                case SphereCollider sphereCollider:
                    meshFilter.mesh = SphereMesh;
                    var largestLocalScaleComponent = Mathf.Max(Mathf.Max(transform.localScale.x, transform.localScale.y), transform.localScale.z);
                    var worldSpaceScale = sphereCollider.radius * 2 * largestLocalScaleComponent;
                    meshGameObject.transform.localScale = new Vector3(worldSpaceScale / transform.localScale.x, worldSpaceScale / transform.localScale.y, worldSpaceScale / transform.localScale.z);
                    break;
                case CapsuleCollider capsuleCollider:
                    meshFilter.mesh = CapsuleMesh;
                    meshGameObject.transform.localScale = new Vector3(capsuleCollider.radius * 2, capsuleCollider.height / 2, capsuleCollider.radius * 2);
                    break;
            }

            meshGameObject.transform.SetParent(transform, false);
        }
    }
}
