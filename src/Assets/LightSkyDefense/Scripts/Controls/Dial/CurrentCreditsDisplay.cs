using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CurrentCreditsDisplay : MonoBehaviour
{
    [Tooltip("Credit text prefab")]
    public GameObject CurrentCreditText;

    private GameObject _textMesh;

    void Start()
    {
        var costMesh = CurrentCreditText.GetComponent<TextMesh>();
        costMesh.text = "Currency:";

        _textMesh = Instantiate(CurrentCreditText,
            gameObject.transform.position + new Vector3(0, 0, -0.2f), // Small offset towards wrist
            gameObject.transform.rotation);
    }

    void Update()
    {
        _textMesh.transform.rotation = Player.instance.headCollider.transform.rotation;
    }
}
