using UnityEngine.EventSystems;


interface IOnGameWinTarget : IEventSystemHandler
{
    void OnGameWin();
}

interface IOnGameLoseTarget : IEventSystemHandler
{
    void OnGameLose();
}
