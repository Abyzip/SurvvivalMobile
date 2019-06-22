using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SpawnVortexInformations 
{
    public int force;
    public int spawnerNum;

    public SpawnVortexInformations(int force, int spawnerNum)
    {
        this.force = force;
        this.spawnerNum = spawnerNum;
    }
}   
