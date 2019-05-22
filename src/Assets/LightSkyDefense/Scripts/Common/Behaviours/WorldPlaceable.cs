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

    private static LayerMask _layerMask = 0;

    void Awake()
    {
        if (!isVisual && Physics.CheckBox(ObjectCollider.transform.position, ObjectCollider.size / 2f, transform.rotation, _layerMask.value))
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (_layerMask == 0)
        {
            _layerMask = LayerMask.GetMask("Path", "Enemies");
        }
    }

    void FixedUpdate()
    {
        if (ObjectCollider == null)
            return;

        if (Physics.CheckBox(ObjectCollider.transform.position, ObjectCollider.size / 2f, transform.rotation, _layerMask.value))
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
