using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PauseMenuBehaviour : MonoBehaviour
{
    public SteamVR_Action_Boolean DialClickAction = SteamVR_Input.GetBooleanAction("MenuButtonClick");

    public GameObject Prefab;
    public Vector3 Offset = new Vector3(0, 0, 1.5f);

    private GameObject _menuInstance;

    private Camera _camera;
    
    public void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialClickAction.stateDown)
        {
            return;
        }

        if(_menuInstance != null)
        {
            // Emit OnResumeGame message to all game objects
            foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
            {
                go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
            }

            Time.timeScale = 1;

            Destroy(_menuInstance);
            return;
        }

        // Emit OnPauseGame message to all game objects
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
        }

        Time.timeScale = 0;

        var playerTransform = Player.instance.headCollider.transform;
        _menuInstance = Instantiate(
            Prefab,
            playerTransform.position + (playerTransform.rotation * Offset),
            Quaternion.Euler(new Vector3(0, _camera.gameObject.transform.rotation.y, 0))
        );
    }
}
