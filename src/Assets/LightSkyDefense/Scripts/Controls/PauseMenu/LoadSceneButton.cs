using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class LoadSceneButton : MenuButton
{
    public string SceneName;
    public LoadSceneMode LoadSceneMode;

    public override void OnClick(Hand hand)
    {
        SteamVR_LoadLevel.Begin(SceneName);
    }
}
