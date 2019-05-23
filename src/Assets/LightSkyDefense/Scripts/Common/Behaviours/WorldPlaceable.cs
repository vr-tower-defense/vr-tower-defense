using UnityEngine;

public class WorldPlaceable : MonoBehaviour
{
    public SphereCollider ObjectCollider;

    [Header("True makes the object InvalidColor when it's colliding, false will remove the object when it's not allowed")]
    public bool isVisual = true;

    public Color InvalidColor = new Color(0.2f, 0.2f, 0.2f);

    public bool isPlaceValid { get; private set; }

    /// <summary>
    /// Keep track of the original material so we can revert to it.
    /// </summary>
    private Material _originalPrefabMat;

    /// <summary>
    /// Store the gray material so we don't have to recreate it every time we need it.
    /// </summary>
    private Material _grayMaterial;

    private static Layers _layerMask = Layers.Path|Layers.Enemies|Layers.Towers;

    void Awake()
    {
        if (isVisual)
            return;

        foreach (Collider collider in Physics.OverlapSphere(ObjectCollider.transform.position, ObjectCollider.radius, (int)_layerMask))
        {
            if(collider.transform != ObjectCollider.transform)
            {
                Destroy(gameObject);
                return;
            }
        }
        Destroy(this);
    }

    void Start()
    {
        // Store original material
        _originalPrefabMat = GetComponentInChildren<Renderer>().material;

        // Clone original material
        _grayMaterial = new Material(_originalPrefabMat);

        // Set color to gray
        _grayMaterial.color = InvalidColor;

        //Do the check once to initialize isPlaceValid correctly

        if (Physics.CheckSphere(ObjectCollider.transform.position, ObjectCollider.radius, (int)_layerMask))
        {
            GetComponentInChildren<Renderer>().material = _grayMaterial;
            isPlaceValid = false;
        }
        else
        {
            GetComponentInChildren<Renderer>().material = _originalPrefabMat;
            isPlaceValid = true;
        }

    }

    void FixedUpdate()
    {
        if (ObjectCollider == null)
            return;

        if (Physics.CheckSphere(ObjectCollider.transform.position, ObjectCollider.radius, (int)_layerMask) && isPlaceValid)
        {
            GetComponentInChildren<Renderer>().material = _grayMaterial;
            isPlaceValid = false;
        }
        else if(!isPlaceValid)
        {
            GetComponentInChildren<Renderer>().material = _originalPrefabMat;
            isPlaceValid = true;
        }
    }
}
