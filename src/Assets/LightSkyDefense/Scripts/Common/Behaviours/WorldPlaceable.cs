using UnityEngine;

public class WorldPlaceable : MonoBehaviour
{
    public BoxCollider ObjectCollider;

    [Header("True makes the object InvalidColor when it's colliding, false will remove the object when it's not allowed")]
    public bool isVisual = true;

    public Color InvalidColor = new Color(0.2f, 0.2f, 0.2f);

    /// <summary>
    /// Keep track of the original material so we can revert to it
    /// </summary>
    private Material _originalPrefabMat;

    private static Layers _layerMask = Layers.Path|Layers.Enemies|Layers.Towers;

    void Awake()
    {
        if (!isVisual)
        {
            foreach (Collider collider in Physics.OverlapBox(ObjectCollider.transform.position, ObjectCollider.size / 2f, transform.rotation, (int)_layerMask))
            {
                if(collider.transform != ObjectCollider.transform)
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (ObjectCollider == null)
            return;

        if (Physics.CheckBox(ObjectCollider.transform.position, ObjectCollider.size / 2f, transform.rotation, (int)_layerMask))
        {
            if (_originalPrefabMat == null)
            {
                // Store original material
                _originalPrefabMat = GetComponentInChildren<Renderer>().material;
                Material grayMaterial = new Material(_originalPrefabMat);

                // Set color to gray
                grayMaterial.color = InvalidColor;
                GetComponentInChildren<Renderer>().material = grayMaterial;
            }
        }
        else
        {
            if (_originalPrefabMat != null)
            {
                // Restore original material
                GetComponentInChildren<Renderer>().material = _originalPrefabMat;

                _originalPrefabMat = null;
            }
        }
    }
}
