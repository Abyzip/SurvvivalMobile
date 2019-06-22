using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SpawnBombInformations 
{
    public Vector3 force;
    public int spawnerNum;

    public SpawnBombInformations(Vector3 force, int spawnerNum)
    {
        this.force = force;
        this.spawnerNum = spawnerNum;
    }
}   
