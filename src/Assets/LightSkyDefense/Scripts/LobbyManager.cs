using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class LobbyManager : MonoBehaviour
{
    public float PauseMenuHintTimeout = 2.5f;

    public SteamVR_Action_Boolean MenuButtonClickAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("MenuButtonClick");
    public SteamVR_Action_Boolean TriggerClickAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("TriggerClick");
    public SteamVR_Action_Boolean DialClickAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("DialClick");

    public string MenuButtonClickActionHint = "Open pauze scherm";
    public string TriggerClickActionHint = "Verwijder een toren";
    public string DialClickActionHint = "Kies een toren";

    /// <summary>
    /// Start routine that is used to show the hints
    /// </summary>
    private void Start()
    {
        StartCoroutine(ShowHints());
    }

    /// <summary>
    /// Stop routine and hide hints
    /// </summary>
    private void OnDestroy()
    {
        StopAllCoroutines();

        if (Player.instance == null)
        {
            return;
        }

        ControllerButtonHints.HideAllTextHints(Player.instance.leftHand);
        ControllerButtonHints.HideAllTextHints(Player.instance.rightHand);
    }

    private void Update()
    {
        var menuButtonClick = MenuButtonClickAction.GetStateDown(SteamVR_Input_Sources.Any);
        var triggerClick = TriggerClickAction.GetStateDown(SteamVR_Input_Sources.Any);
        var dialClick = DialClickAction.GetStateDown(SteamVR_Input_Sources.Any);

        if (menuButtonClick)
            ControllerButtonHints.HideTextHint(Player.instance.leftHand, MenuButtonClickAction);

        if (triggerClick)
            ControllerButtonHints.HideTextHint(Player.instance.rightHand, TriggerClickAction);

        if (dialClick)
            ControllerButtonHints.HideTextHint(Player.instance.rightHand, DialClickAction);
    }

    /// <summary>
    /// Show hints for pause menu, tower placement and tower remove
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowHints()
    {
        yield return new WaitForSeconds(PauseMenuHintTimeout);

        ControllerButtonHints.ShowTextHint(Player.instance.leftHand, MenuButtonClickAction, MenuButtonClickActionHint);
        ControllerButtonHints.ShowTextHint(Player.instance.rightHand, TriggerClickAction, TriggerClickActionHint);
        ControllerButtonHints.ShowTextHint(Player.instance.rightHand, DialClickAction, DialClickActionHint);
    }
}
