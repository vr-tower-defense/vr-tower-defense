using UnityEngine.EventSystems;


interface IOnGameWinTarget : IEventSystemHandler
{
    void OnGameWin();
}

interface IOnGameLossTarget : IEventSystemHandler
{
    void OnGameLoss();
}
