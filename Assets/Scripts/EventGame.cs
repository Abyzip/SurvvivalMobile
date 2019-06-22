using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EventGame
{
    public float duration;
    public string nameEvent;
    public string difficulty;
    public float rate;
    public int nbBomb;
    public List<SpawnBombInformations> spawnBombInformations = new List<SpawnBombInformations>();
    public int nbSpawners;
    public GameObject[] spawners;
    public GameObject[] spawnersProjectile;

    public GameObject spawnerTempete;
    public List<SpawnApocalypseInformations> spawnApocalypseInformations = new List<SpawnApocalypseInformations>();
    public float xMinFxShield, xMaxFxShield, zMinFxShield, zMaxFxShield;
    public SpawnVortexInformations spawnVortexInformations;
    public GameObject[] spawnersVortex;
    public int[] spawnOrderProjectile;
    public EventGame(string nameEvent, string difficulty, int nbSpawners, GameObject[] spawners)
    {
        this.duration = 10;
        this.nameEvent = nameEvent;
        this.difficulty = difficulty;
        this.nbSpawners = nbSpawners;
        this.spawners = spawners;
        initSpawnBombInformations();
    }
    public EventGame(string nameEvent, GameObject[] spawnersProjectile)
    {
        this.duration = 0;
        this.nameEvent = nameEvent;
        this.spawnersProjectile = spawnersProjectile;
        initSpawnProjectile();
    }

    public EventGame(string nameEvent, string difficulty, GameObject spawnerTempete)
    {
        this.duration = 20;
        this.nameEvent = nameEvent;
        this.difficulty = difficulty;
        this.spawnerTempete = spawnerTempete;
    }

    public EventGame(string nameEvent, string difficulty, GameObject[] spawnersVortex, int nbSpawners)
    {
        this.duration = 15;
        this.nameEvent = nameEvent;
        this.difficulty = difficulty;
        this.spawnersVortex = spawnersVortex;
        this.nbSpawners = nbSpawners;
        initSpawnVortexInformations();
    }

    public EventGame(string nameEvent, string difficulty, float xMinFxShield, float xMaxFxShield, float zMinFxShield, float zMaxFxShield)
    {
        this.duration = 5;
        this.nameEvent = nameEvent;
        this.difficulty = difficulty;
        this.xMinFxShield = xMinFxShield;
        this.xMaxFxShield = xMaxFxShield;
        this.zMinFxShield = zMinFxShield;
        this.zMaxFxShield = zMaxFxShield;
        initSpawnApocalypseInformations();
    }

    public void initSpawnProjectile()
    {

        int i = 0;
        spawnOrderProjectile = new int[500];
        float comptTime = 0;
        while (i < 500)
        {
            
            spawnOrderProjectile[i] = Random.Range(0, spawnersProjectile.Length);
            /* if (i < 60)
                 comptTime += 0.25f;
             else if (i < 60)
                 comptTime += 0.5f;
             else
                 comptTime += 1f;*/
            i++;
        }
    }
    public void initSpawnApocalypseInformations()
    {
        int nbSafeZones = 0;
        if (difficulty == "LOW")
        {
            nbSafeZones = 4;
        }
        else if (difficulty == "MEDIUM")
        {
            nbSafeZones = 3;
        }
        else if (difficulty == "HARD")
        {
            nbSafeZones = 2;
        }
        else if (difficulty == "EXTREME")
        {
            nbSafeZones = 1;
        }

        int i = 0;
        spawnApocalypseInformations = new List<SpawnApocalypseInformations>();
        while (i < nbSafeZones)
        {
            spawnApocalypseInformations.Add(new SpawnApocalypseInformations(new Vector3(Random.Range(xMinFxShield, xMaxFxShield), 0.5f, Random.Range(zMinFxShield, zMaxFxShield))));
            i++;
        }
    }

    public void initSpawnVortexInformations()
    {
        int power = 0;
        if (difficulty == "LOW")
        {
            power = 5;
        }
        else if (difficulty == "MEDIUM")
        {
            power = 10;
        }
        else if (difficulty == "HARD")
        {
            power = 15;
        }
        else if (difficulty == "EXTREME")
        {
            power = 20;
        }


        spawnVortexInformations = new SpawnVortexInformations(power, Random.Range(0, nbSpawners)) ;
        

    }

    public void initSpawnBombInformations()
    {

        if (difficulty == "LOW")
        {
            this.rate = 2.5f;
            this.nbBomb = 4;
        }
        else if (difficulty == "MEDIUM")
        {
            this.rate = 1f;
            this.nbBomb = 10;
        }
        else if (difficulty == "HARD")
        {
            this.rate = 0.5f;
            this.nbBomb = 20;
        }
        else if (difficulty == "EXTREME")
        {
            this.rate = 0.25f;
            this.nbBomb = 40;
        }

        int currentNbBomb = 0;
        while (currentNbBomb != nbBomb)
        {
            Debug.Log(nbSpawners);
            int rand = Random.Range(0, nbSpawners);
            SpawnBombInformations currentSBI = new SpawnBombInformations(spawners[rand].GetComponent<BombSpawnerScript>().GenerateAleaForce(), rand);
            spawnBombInformations.Add(currentSBI);
            currentNbBomb++;
        }
    }
    public string Play()
    {
        if (nameEvent == "eventBomb")
            return "bombCoroutine";
        else if (nameEvent == "eventTempete")
            return "tempeteCoroutine";
        else if (nameEvent == "eventApocalypse")
            return "apocalypseCoroutine";
        else if (nameEvent == "eventVortex")
            return "vortexCoroutine";
        else if (nameEvent == "eventProjectile")
            return "projectileCoroutine";
        else
            return "";
    }
    

    
}