using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public abstract class WaveStep : ScriptableObject
{
    public abstract IEnumerator Run();
}

