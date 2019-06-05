using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PauseMenuBehaviour : MonoBehaviour
{
    public SteamVR_Action_Boolean DialClickAction = SteamVR_Input.GetBooleanAction("MenuButtonClick");

    public GameObject Prefab;

    public Vector3 Offset = new Vector3(0, 0, 1.5f);

    private GameObject _instance;

    // Update is called once per frame
    void Update()
    {
        if (!DialClickAction.stateDown)
        {
            return;
        }

        // Destroy pause menu when there's already a pause menu instance
        if (_instance != null)
        {
            // Emit OnResumeGame message to all game objects
            foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
            {
                go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
            }

            Destroy(_instance);
            return;
        }

        // Emit OnPauseGame message to all game objects
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
        }

        var camera = Camera.main;
        var yRotation = Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0);

        _instance = Instantiate(
            Prefab,
            camera.transform.position + (yRotation * Offset),
            yRotation
        );
    }
}
