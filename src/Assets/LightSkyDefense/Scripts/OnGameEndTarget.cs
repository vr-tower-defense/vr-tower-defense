using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;


interface IOnGameWinTarget : IEventSystemHandler
{
    void OnGameWin();
}

interface IOnGameLossTarget : IEventSystemHandler
{
    void OnGameLoss();
}
