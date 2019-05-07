using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;


interface IOnGameEndTarget : IEventSystemHandler
{
    void OnGameWin();
    void OnGameLoss();
}

